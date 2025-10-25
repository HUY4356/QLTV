using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QLThuvien.Data;
using QLThuvien.Models;
using QLThuvien.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QLThuvien.BackgroundServices
{
    public class DichVuKiemTraNoShow : BackgroundService
    {
        private readonly ILogger<DichVuKiemTraNoShow> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _thoiGianKiemTra = TimeSpan.FromHours(1); // Kiểm tra mỗi giờ

        public DichVuKiemTraNoShow(
            ILogger<DichVuKiemTraNoShow> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Dịch vụ kiểm tra No-Show phòng đang khởi động. Sẽ kiểm tra sau mỗi {Interval}.", _thoiGianKiemTra);
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(45), stoppingToken); // Đợi ứng dụng khởi động
            }
            catch (TaskCanceledException) { return; }

            using var timer = new PeriodicTimer(_thoiGianKiemTra);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    _logger.LogInformation("Bắt đầu kiểm tra No-Show phòng lúc: {time}", DateTimeOffset.Now);
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ThuVienDbContext>();
                        var dichVuViPham = scope.ServiceProvider.GetRequiredService<IDichVuViPham>();
                        await KiemTraVaXuLyNoShowAsync(dbContext, dichVuViPham, stoppingToken);
                    }
                    _logger.LogInformation("Hoàn tất kiểm tra No-Show phòng.");
                }
                catch (OperationCanceledException) { break; } // Thoát nếu bị hủy
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi không mong muốn khi kiểm tra No-Show.");
                    try { await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); } catch (OperationCanceledException) { break; }
                }
            }
            _logger.LogInformation("Dịch vụ kiểm tra No-Show phòng đã dừng.");
        }

        private async Task KiemTraVaXuLyNoShowAsync(ThuVienDbContext dbContext, IDichVuViPham dichVuViPham, CancellationToken cancellationToken)
        {
            var thoiGianHienTaiUtc = DateTime.UtcNow;
            var thoiGianNguongUtc = thoiGianHienTaiUtc.AddHours(-25); // Kiểm tra các lượt đặt kết thúc trong 25h qua

            // Tìm các lượt đặt đã kết thúc nhưng trạng thái vẫn là "DaXacNhan"
            var potentialNoShows = await dbContext.DatPhongs
                .Where(dp => dp.GioKetThuc < thoiGianHienTaiUtc &&
                             dp.GioKetThuc >= thoiGianNguongUtc &&
                             dp.TrangThai == "DaXacNhan")
                .ToListAsync(cancellationToken);

            if (!potentialNoShows.Any())
            {
                _logger.LogDebug("Không tìm thấy lượt đặt phòng nào nghi ngờ No-Show.");
                return;
            }

            _logger.LogInformation("Tìm thấy {Count} lượt đặt phòng nghi ngờ No-Show.", potentialNoShows.Count);
            int processedCount = 0;
            List<Task> violationTasks = new List<Task>();

            foreach (var booking in potentialNoShows)
            {
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogWarning("Phát hiện No-Show: DatPhong ID {BookingId}, User ID {UserId}, Phòng ID {PhongId}",
                                   booking.Id, booking.UserId, booking.PhongId);

                // Ghi nhận vi phạm (có thể chạy song song)
                violationTasks.Add(dichVuViPham.GhiNhanViPhamAsync(booking.UserId, LoaiViPham.NoShowPhong));

                // Cập nhật trạng thái lượt đặt
                booking.TrangThai = "NoShow";
                dbContext.Entry(booking).State = EntityState.Modified;
                processedCount++;
            }

            // Đợi ghi nhận vi phạm xong
            // await Task.WhenAll(violationTasks);

            if (processedCount > 0)
            {
                try
                {
                    await dbContext.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Đã cập nhật trạng thái NoShow cho {Count} lượt đặt phòng.", processedCount);
                }
                catch (Exception ex) { _logger.LogError(ex, "Lỗi khi lưu thay đổi trạng thái NoShow."); }
            }
        }
    }
}

