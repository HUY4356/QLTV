using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QLThuvien.Models; // Namespace chứa DbContext và Models
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QLThuvien.BackgroundServices
{
    public class DichVuCapNhatTrangThaiPhong : BackgroundService
    {
        private readonly ILogger<DichVuCapNhatTrangThaiPhong> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _thoiGianKiemTra = TimeSpan.FromMinutes(1); // Kiểm tra mỗi phút

        public DichVuCapNhatTrangThaiPhong(
            ILogger<DichVuCapNhatTrangThaiPhong> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Dịch vụ cập nhật trạng thái phòng đang khởi động.");

            // Sử dụng PeriodicTimer để chạy định kỳ (cách làm hiện đại)
            using var timer = new PeriodicTimer(_thoiGianKiemTra);

            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Bắt đầu kiểm tra và cập nhật trạng thái phòng lúc: {time}", DateTimeOffset.Now);
                    await CapNhatTrangThaiPhongAsync(stoppingToken);
                    _logger.LogInformation("Hoàn tất kiểm tra và cập nhật trạng thái phòng.");
                }
                catch (OperationCanceledException)
                {
                    // Bắt lỗi khi dịch vụ bị dừng
                    _logger.LogInformation("Dịch vụ cập nhật trạng thái phòng đang dừng.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi không mong muốn xảy ra trong quá trình cập nhật trạng thái phòng.");
                    // Chờ một chút trước khi thử lại để tránh spam log nếu lỗi liên tục
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }
            _logger.LogInformation("Dịch vụ cập nhật trạng thái phòng đã dừng.");
        }

        private async Task CapNhatTrangThaiPhongAsync(CancellationToken cancellationToken)
        {
            // Tạo một scope riêng biệt để lấy DbContext (cần thiết cho Background Service)
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ThuVienDbContext>();

            var thoiGianHienTai = DateTime.UtcNow; // Dùng UtcNow cho nhất quán trên server

            // --- Logic 1: Chuyển phòng thành "Đang được thuê" ---
            // Lấy ID các phòng đang có lượt đặt hợp lệ (bắt đầu <= hiện tại < kết thúc)
            // và trạng thái phòng hiện tại là "Trống"
            var phongCanChuyenSangDuocThueIds = await dbContext.DatPhongs
                .Where(dp => dp.GioBatDau <= thoiGianHienTai && thoiGianHienTai < dp.GioKetThuc
                            // Chỉ xem xét các lượt đặt đã xác nhận (nếu có trạng thái đặt phòng)
                            // && dp.TrangThai == "DaXacNhan" 
                            && dp.Phong.TrangThai == RoomStatus.Trong) // Chỉ lấy phòng đang trống
                .Select(dp => dp.PhongId)
                .Distinct()
                .ToListAsync(cancellationToken);

            if (phongCanChuyenSangDuocThueIds.Any())
            {
                _logger.LogInformation("Tìm thấy {count} phòng cần cập nhật trạng thái sang 'Đang được thuê'.", phongCanChuyenSangDuocThueIds.Count);
                await dbContext.Phongs
                    .Where(p => phongCanChuyenSangDuocThueIds.Contains(p.Id))
                    .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.TrangThai, RoomStatus.DuocThue), cancellationToken);
            }

            // --- Logic 2: Chuyển phòng về lại "Trống" ---
            // Lấy ID các phòng đang ở trạng thái "Được thuê"
            var phongDangDuocThueIds = await dbContext.Phongs
                .Where(p => p.TrangThai == RoomStatus.DuocThue)
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);

            if (phongDangDuocThueIds.Any())
            {
                // Với mỗi phòng đang thuê, kiểm tra xem còn lượt đặt nào đang diễn ra không
                var phongCanChuyenSangTrongIds = new List<int>();
                foreach (var phongId in phongDangDuocThueIds)
                {
                    bool conLuotDatDangDienRa = await dbContext.DatPhongs
                        .AnyAsync(dp => dp.PhongId == phongId &&
                                       dp.GioBatDau <= thoiGianHienTai &&
                                       thoiGianHienTai < dp.GioKetThuc,
                                  cancellationToken);

                    // Nếu không còn lượt đặt nào đang diễn ra
                    if (!conLuotDatDangDienRa)
                    {
                        phongCanChuyenSangTrongIds.Add(phongId);
                    }
                }

                if (phongCanChuyenSangTrongIds.Any())
                {
                    _logger.LogInformation("Tìm thấy {count} phòng cần cập nhật trạng thái về 'Trống'.", phongCanChuyenSangTrongIds.Count);
                    await dbContext.Phongs
                        .Where(p => phongCanChuyenSangTrongIds.Contains(p.Id))
                        // Đảm bảo không ghi đè trạng thái Bảo trì
                        .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.TrangThai, RoomStatus.Trong), cancellationToken);
                }
            }

            // Logic 3: Có thể thêm logic kiểm tra các lượt đặt "No Show" (không đến) và hủy/cập nhật trạng thái phòng sau một khoảng thời gian chờ.

            // Logic 4: Các phòng Bảo trì (BaoTri) sẽ không bị ảnh hưởng bởi logic trên.
        }
    }
}
