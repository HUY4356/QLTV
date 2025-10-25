using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QLThuvien.Data;
using QLThuvien.Models;
using System;
using System.Threading.Tasks;

namespace QLThuvien.Services
{
    public class DichVuMuonTra : IDichVuMuonTra
    {
        private readonly ThuVienDbContext _context;
        private readonly IDichVuViPham _dichVuViPham;
        private readonly ILogger<DichVuMuonTra> _logger;

        public DichVuMuonTra(
            ThuVienDbContext context,
            IDichVuViPham dichVuViPham,
            ILogger<DichVuMuonTra> logger)
        {
            _context = context;
            _dichVuViPham = dichVuViPham;
            _logger = logger;
        }

        public async Task<(bool ThanhCong, string ThongBao)> TraSachAsync(int muonTraId)
        {
            _logger.LogInformation("Bắt đầu xử lý trả sách cho MuonTra ID: {MuonTraId}", muonTraId);

            // Tải bản ghi MuonTra cùng với The (để lấy UserId) và BanTheSach (để cập nhật tình trạng)
            var muonTra = await _context.MuonTras
                                .Include(mt => mt.The)
                                .Include(mt => mt.BanTheSach)
                                .FirstOrDefaultAsync(mt => mt.Id == muonTraId);

            if (muonTra == null)
            {
                _logger.LogWarning("Không tìm thấy MuonTra ID: {MuonTraId}", muonTraId);
                return (false, "Không tìm thấy thông tin lượt mượn sách này.");
            }

            if (muonTra.TrangThai == "DaTra" || muonTra.TrangThai == "DaThanhLy")
            {
                _logger.LogInformation("Sách đã được xử lý trước đó (MuonTra ID: {MuonTraId}, Trạng thái: {TrangThai}).", muonTraId, muonTra.TrangThai);
                return (false, $"Sách này đã được '{muonTra.TrangThai}'.");
            }

            var ngayTraThucTeUtc = DateTime.UtcNow;
            muonTra.NgayTra = ngayTraThucTeUtc;
            muonTra.TrangThai = "DaTra";

            // Cập nhật tình trạng bản thẻ sách
            if (muonTra.BanTheSach != null)
            {
                muonTra.BanTheSach.TinhTrang = "SanSang"; // Hoặc trạng thái phù hợp
                _logger.LogInformation("Cập nhật BanTheSach ID {BanTheSachId} thành 'SanSang'.", muonTra.BanTheSachId);
            }
            else
            {
                _logger.LogWarning("Không tìm thấy BanTheSach liên kết với MuonTra ID {MuonTraId}.", muonTraId);
            }

            _logger.LogInformation("Cập nhật MuonTra ID {MuonTraId}: NgayTra={NgayTra}, TrangThai=DaTra", muonTraId, ngayTraThucTeUtc);

            // Kiểm tra trễ hạn và tính phạt
            bool treHan = false;
            DateTime dueDateUtc = DateTime.MinValue;
            decimal tienPhat = 0m; // Khởi tạo tiền phạt

            try
            {
                // Chuẩn hóa DueDate về UTC để so sánh
                dueDateUtc = muonTra.DueDate.Kind == DateTimeKind.Unspecified
                           ? DateTime.SpecifyKind(muonTra.DueDate, DateTimeKind.Local).ToUniversalTime()
                           : muonTra.DueDate.ToUniversalTime();

                // Chỉ tính trễ nếu ngày trả > ngày hạn trả
                treHan = ngayTraThucTeUtc.Date > dueDateUtc.Date;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý DueDate cho MuonTra ID: {MuonTraId}. DueDate gốc: {DueDate}", muonTraId, muonTra.DueDate);
                treHan = false; // Mặc định không trễ hạn nếu có lỗi
            }

            if (treHan)
            {
                _logger.LogWarning("Phát hiện trả sách trễ hạn: MuonTra ID {MuonTraId}, DueDate (UTC): {DueDateUtc}, NgayTra (UTC): {NgayTraUtc}", muonTraId, dueDateUtc, ngayTraThucTeUtc);

                if (muonTra.The == null)
                {
                    _logger.LogError("Không thể ghi nhận vi phạm/phạt trễ hạn vì không load được thông tin thẻ (The) cho MuonTra ID: {MuonTraId}", muonTraId);
                }
                else
                {
                    int userId = muonTra.The.UserId;
                    // Ghi nhận vi phạm
                    await _dichVuViPham.GhiNhanViPhamAsync(userId, LoaiViPham.TreHanSach);

                    // Tính tiền phạt
                    try
                    {
                        var thamSoPhat = await _context.ThamSoHeThongs.AsNoTracking().FirstOrDefaultAsync(ts => ts.Ten == "MucPhatTreHanMotNgay");
                        if (thamSoPhat != null && decimal.TryParse(thamSoPhat.GiaTri, out decimal mucPhatMoiNgay) && mucPhatMoiNgay > 0)
                        {
                            int soNgayTre = (ngayTraThucTeUtc.Date - dueDateUtc.Date).Days;
                            if (soNgayTre > 0)
                            {
                                tienPhat = soNgayTre * mucPhatMoiNgay;
                                _logger.LogInformation("Tính tiền phạt cho MuonTra ID {MuonTraId}: {SoNgayTre} ngày * {MucPhat} = {TienPhat}", muonTraId, soNgayTre, mucPhatMoiNgay, tienPhat);
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Không có cấu hình phạt trễ hạn ('MucPhatTreHanMotNgay').");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi khi tính tiền phạt cho MuonTra ID: {MuonTraId}", muonTraId);
                    }
                }
            }
            else
            {
                _logger.LogInformation("Sách được trả đúng hạn hoặc sớm hạn (MuonTra ID: {MuonTraId}).", muonTraId);
            }

            // Cập nhật tiền phạt (kể cả khi đúng hạn thì cũng về 0)
            muonTra.TienPhat = tienPhat;

            // Lưu thay đổi
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Lưu thông tin trả sách thành công cho MuonTra ID: {MuonTraId}", muonTraId);
                return (true, treHan ? "Trả sách thành công (Trễ hạn)!" : "Trả sách thành công!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lưu thay đổi trả sách cho MuonTra ID: {MuonTraId}", muonTraId);
                return (false, "Đã có lỗi xảy ra khi lưu thông tin trả sách.");
            }
        }
    }
}

