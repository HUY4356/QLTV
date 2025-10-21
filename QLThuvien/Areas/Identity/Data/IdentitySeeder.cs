using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QLThuvien.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var thuVienDbContext = scope.ServiceProvider.GetRequiredService<ThuVienDbContext>();

        // --- BƯỚC 1: ĐẢM BẢO CÁC VAI TRÒ TỒN TẠI TRONG CẢ 2 HỆ THỐNG ---
        string[] roles = new[] { "Admin", "ThuThu", "DocGia" };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            if (!await thuVienDbContext.Roles.AnyAsync(r => r.Name == roleName))
            {
                thuVienDbContext.Roles.Add(new Role { Name = roleName });
            }
        }
        // Chỉ lưu một lần sau khi thêm tất cả các vai trò
        if (thuVienDbContext.ChangeTracker.HasChanges())
        {
            await thuVienDbContext.SaveChangesAsync();
        }

        // --- BƯỚC 2: TẠO ADMIN USER TRONG HỆ THỐNG IDENTITY ---
        var adminEmail = config["AdminUser:Email"] ?? "taone@gmail.com";
        var adminPass = config["AdminUser:Password"] ?? "Admin!1234";

        var adminIdentityUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminIdentityUser == null)
        {
            adminIdentityUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var createResult = await userManager.CreateAsync(adminIdentityUser, adminPass);
            if (!createResult.Succeeded)
            {
                throw new Exception("Không thể tạo admin user trong Identity: " + string.Join("; ", createResult.Errors.Select(e => e.Description)));
            }
        }

        // Đảm bảo user admin luôn có vai trò "Admin" trong Identity
        if (!await userManager.IsInRoleAsync(adminIdentityUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminIdentityUser, "Admin");
        }

        // --- BƯỚC 3 (ĐÃ SỬA LỖI): LUÔN KIỂM TRA VÀ ĐỒNG BỘ ADMIN VÀO BẢNG 'User' ---
        // Logic này được đưa ra ngoài để đảm bảo nó luôn chạy
        if (!await thuVienDbContext.Users.AnyAsync(u => u.Email == adminEmail))
        {
            var appAdminRole = await thuVienDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (appAdminRole != null)
            {
                var appAdminUser = new User
                {
                    Fullname = "Administrator",
                    Email = adminEmail,
                    Password = "hashed_by_identity", // Mật khẩu này không dùng để đăng nhập
                    Phone = "0123456789",
                    RoleId = appAdminRole.Id,
                    CreatedAt = DateTime.UtcNow
                };
                thuVienDbContext.Users.Add(appAdminUser);
                await thuVienDbContext.SaveChangesAsync();
            }
        }
    }
}

