using Microsoft.EntityFrameworkCore;
using QLThuvien.Models;
using QLThuvien.Models.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Thêm using cho Logging

namespace QLThuvien.Services
{
    public class DichVuDatPhong : IDichVuDatPhong
    {
        private readonly ThuVienDbContext _context;
        private readonly ILogger<DichVuDatPhong> _logger; // Thêm Logger

        public DichVuDatPhong(ThuVienDbContext context, ILogger<DichVuDatPhong> logger) // Thêm Logger vào constructor
        {
            _context = context;
            _logger = logger; // Gán Logger
        }

        public async Task<(bool ThanhCong, string ThongBao, DatPhong? DatPhongMoi)> TaoDatPhongAsync(YeuCauDatPhongDto yeuCau, int userId)
        {
            _logger.LogInformation("Bắt đầu xử lý yêu cầu đặt phòng cho User ID: {UserId}, Phòng ID: {PhongId}", userId, yeuCau.PhongId);
            _logger.LogInformation("Thời gian yêu cầu (Input): Bắt đầu={StartTimeLocal}, Kết thúc={EndTimeLocal}", yeuCau.GioBatDau, yeuCau.GioKetThuc);

            // --- CHUẨN HÓA THỜI GIAN VỀ UTC ---
            DateTime gioBatDauLocal, gioKetThucLocal;
            try
            {
                // Gán Kind là Local để ToUniversalTime() hoạt động đúng
                gioBatDauLocal = DateTime.SpecifyKind(yeuCau.GioBatDau, DateTimeKind.Local);
                gioKetThucLocal = DateTime.SpecifyKind(yeuCau.GioKetThuc, DateTimeKind.Local);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "DateTime không hợp lệ nhận từ client.");
                return (false, "Định dạng thời gian không hợp lệ.", null);
            }

            // Chuyển đổi sang UTC để lưu trữ và so sánh
            var gioBatDauUtc = gioBatDauLocal.ToUniversalTime();
            var gioKetThucUtc = gioKetThucLocal.ToUniversalTime();
            _logger.LogInformation("Thời gian yêu cầu (UTC): Bắt đầu={StartTimeUtc}, Kết thúc={EndTimeUtc}", gioBatDauUtc, gioKetThucUtc);
            // ------------------------------------

            // 1. Kiểm tra User
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Không tìm thấy User ID: {UserId}", userId);
                return (false, "Người dùng không tồn tại.", null);
            }

            // 2. Kiểm tra Phòng
            var phong = await _context.Phongs.FindAsync(yeuCau.PhongId);
            if (phong == null)
            {
                _logger.LogWarning("Không tìm thấy Phòng ID: {PhongId}", yeuCau.PhongId);
                return (false, "Phòng không tồn tại.", null);
            }

            // 3. Kiểm tra Bảo trì
            if (phong.TrangThai == RoomStatus.BaoTri)
            {
                _logger.LogWarning("Phòng ID: {PhongId} đang bảo trì.", yeuCau.PhongId);
                return (false, "Phòng đang được bảo trì, không thể đặt.", null);
            }

            // 4. Kiểm tra thời gian hợp lệ
            if (gioBatDauUtc >= gioKetThucUtc)
            {
                _logger.LogWarning("Thời gian không hợp lệ: Kết thúc trước hoặc bằng bắt đầu.");
                return (false, "Thời gian kết thúc phải sau thời gian bắt đầu.", null);
            }
            // So sánh với thời gian hiện tại UTC, cho phép trễ 1 chút (ví dụ 1 phút)
            if (gioBatDauUtc < DateTime.UtcNow.AddMinutes(-1))
            {
                _logger.LogWarning("Thời gian không hợp lệ: Thời gian bắt đầu ở quá khứ.");
                return (false, "Thời gian bắt đầu không hợp lệ (không được ở quá khứ).", null);
            }

            // 5. Kiểm tra lịch có bị trùng không (SO SÁNH BẰNG UTC)
            _logger.LogInformation("Kiểm tra trùng lịch với UTC: Bắt đầu={StartTimeUtc}, Kết thúc={EndTimeUtc}", gioBatDauUtc, gioKetThucUtc);
            // Lấy ra lượt đặt bị trùng (nếu có) để có thể hiển thị thông tin chi tiết hơn
            var overlappingBooking = await _context.DatPhongs
                .AsNoTracking() // Dùng AsNoTracking để EF không theo dõi thực thể này, tránh lỗi khi Add sau đó
                .FirstOrDefaultAsync(dp =>
                    dp.PhongId == yeuCau.PhongId &&
                    dp.GioBatDau < gioKetThucUtc && // Đặt cũ BẮT ĐẦU (UTC) trước khi đặt mới KẾT THÚC (UTC)
                    dp.GioKetThuc > gioBatDauUtc    // Đặt cũ KẾT THÚC (UTC) sau khi đặt mới BẮT ĐẦU (UTC)
                );

            // *** SỬA LỖI LOGIC QUAN TRỌNG: RETURN NGAY KHI CÓ TRÙNG LẶP ***
            if (overlappingBooking != null)
            {
                _logger.LogWarning("Phát hiện trùng lịch với lượt đặt ID: {BookingId}, Time (UTC): {ExistingStart} - {ExistingEnd}",
                   overlappingBooking.Id, overlappingBooking.GioBatDau, overlappingBooking.GioKetThuc);
                // Cung cấp thông tin giờ địa phương cho người dùng dễ hiểu hơn
                var startTimeLocalConflict = overlappingBooking.GioBatDau.ToLocalTime();
                var endTimeLocalConflict = overlappingBooking.GioKetThuc.ToLocalTime();

                // TRẢ VỀ LỖI NGAY LẬP TỨC, KHÔNG LƯU GÌ CẢ
                return (false, $"Lịch bạn chọn bị trùng với một lượt đặt khác ({startTimeLocalConflict:HH:mm} - {endTimeLocalConflict:HH:mm}).", null);
            }
            // ***************************************************************
            _logger.LogInformation("Kiểm tra trùng lịch hoàn tất, không tìm thấy xung đột.");

            // 6. Nếu mọi thứ hợp lệ, tạo lượt đặt phòng mới (LƯU BẰNG UTC)
            var newBooking = new DatPhong
            {
                UserId = userId,
                PhongId = yeuCau.PhongId,
                GioBatDau = gioBatDauUtc, // Lưu UTC
                GioKetThuc = gioKetThucUtc, // Lưu UTC
                NgayDat = DateTime.UtcNow, // Lưu UTC
                TrangThai = "DaXacNhan"
            };

            try
            {
                _context.DatPhongs.Add(newBooking);
                await _context.SaveChangesAsync(); // Chỉ lưu khi không có lỗi trùng lặp
                _logger.LogInformation("Tạo lượt đặt phòng thành công, ID mới: {BookingId}", newBooking.Id);
                return (true, "Đặt phòng thành công!", newBooking);
            }
            catch (DbUpdateException dbEx) // Bắt lỗi cụ thể của EF Core
            {
                _logger.LogError(dbEx, "Lỗi DbUpdateException khi lưu lượt đặt phòng vào database.");
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return (false, $"Lỗi database khi lưu: {innerExceptionMessage}", null);
            }
            catch (Exception ex) // Bắt các lỗi khác
            {
                _logger.LogError(ex, "Lỗi không xác định khi lưu lượt đặt phòng vào database.");
                return (false, "Đã có lỗi xảy ra phía máy chủ khi lưu thông tin đặt phòng.", null);
            }
        }
    }
}