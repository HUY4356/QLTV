using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QL_ThuVien.Chucnangphanquyen;

namespace QL_ThuVien.Data
{
    public class QL_ThuvienContext : IdentityDbContext
    {
        public QL_ThuvienContext(DbContextOptions<QL_ThuvienContext> options)
            : base(options)
        {
        }

        public static async Task KhoiTaoDuLieu(IServiceProvider Dichvu)
        {
            var QL_Nguuoidung = Dichvu.GetService<UserManager<IdentityUser>>();
            var QL_Vaitro = Dichvu.GetService<RoleManager<IdentityRole>>();

            //thêm vai trò vào cơ sơ dữ liệu
            await QL_Vaitro.CreateAsync(new IdentityRole(PhanQuyen.Admin.ToString()));
            await QL_Vaitro.CreateAsync(new IdentityRole(PhanQuyen.User.ToString()));

            //tạo thông mặc định cho tài khoản admin
            var Quantri = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true //xác thực Email
            };

            var NguoiDungTrongCSDL = await QL_Nguuoidung.FindByEmailAsync(Quantri.Email);
            //Nếu tài khoàn admin không tồn tại trong CSDL
            if (NguoiDungTrongCSDL is null)
            {
                //Tạo tài khoản admin với mật khẩu là Abc.123
                await QL_Nguuoidung.CreateAsync(Quantri, "Abc.123");
                await QL_Nguuoidung.AddToRoleAsync(Quantri, PhanQuyen.Admin.ToString());
            }
        }
    }
}
