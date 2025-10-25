using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QLThuvien.Data;
using QLThuvien.Models;
using System;
using System.Threading.Tasks;

namespace QLThuvien.Services
{
    public class DichVuViPham : IDichVuViPham
    {
        private readonly ThuVienDbContext _context;
        private readonly ILogger<DichVuViPham> _logger;

        public DichVuViPham(ThuVienDbContext context, ILogger<DichVuViPham> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task GhiNhanViPhamAsync(int userId, LoaiViPham loaiViPham)
        {
            _logger.LogInformation("Bắt đầu ghi nhận vi phạm loại {LoaiViPham} cho User ID: {UserId}", loaiViPham, userId);
            try
            {
                var viPhamUser = await _context.ViPhamUsers.FirstOrDefaultAsync(v => v.UserId == userId);

                if (viPhamUser == null)
                {
                    bool userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                    if (!userExists)
                    {
                        _logger.LogError("Không thể ghi nhận vi phạm vì User ID: {UserId} không tồn tại.", userId);
                        return;
                    }
                    _logger.LogInformation("Tạo bản ghi ViPhamUser mới cho User ID: {UserId}", userId);
                    viPhamUser = new ViPhamUser { UserId = userId, CountNoShow = 0, CountTreHan = 0 }; // Khởi tạo 0
                    _context.ViPhamUsers.Add(viPhamUser);
                }

                switch (loaiViPham)
                {
                    case LoaiViPham.NoShowPhong:
                        viPhamUser.CountNoShow++; // Tăng trực tiếp vì đã là int
                        _logger.LogInformation("Tăng CountNoShow cho User ID: {UserId}. Giá trị mới: {Count}", userId, viPhamUser.CountNoShow);
                        break;
                    case LoaiViPham.TreHanSach:
                        viPhamUser.CountTreHan++; // Tăng trực tiếp
                        _logger.LogInformation("Tăng CountTreHan cho User ID: {UserId}. Giá trị mới: {Count}", userId, viPhamUser.CountTreHan);
                        break;
                    default:
                        _logger.LogWarning("Loại vi phạm không xác định: {LoaiViPham}", loaiViPham);
                        return;
                }

                viPhamUser.LastUpdated = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Ghi nhận vi phạm thành công cho User ID: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi ghi nhận vi phạm cho User ID: {UserId}, Loại: {LoaiViPham}", userId, loaiViPham);
            }
        }
    }
}

