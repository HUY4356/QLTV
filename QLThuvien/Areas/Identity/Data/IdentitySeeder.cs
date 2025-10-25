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

        // --- BƯỚC 1: TẠO CÁC VAI TRÒ TRONG CẢ HỆ THỐNG IDENTITY & APP ---
        string[] roles = new[] { "Admin", "ThuThu", "DocGia" };
        foreach (var roleName in roles)
        {
            // Tạo role trong Identity nếu chưa có
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Tạo role trong bảng Role của app nếu chưa có
            if (!await thuVienDbContext.Roles.AnyAsync(r => r.Name == roleName))
            {
                thuVienDbContext.Roles.Add(new Role { Name = roleName });
            }
        }

        if (thuVienDbContext.ChangeTracker.HasChanges())
        {
            await thuVienDbContext.SaveChangesAsync();
        }

        // --- BƯỚC 2: TẠO ADMIN USER TRONG IDENTITY ---
        var adminEmail = config["AdminUser:Email"] ?? "taone@gmail.com";
        var adminPass = config["AdminUser:Password"] ?? "Admin!1234";

        var adminIdentityUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminIdentityUser == null)
        {
            adminIdentityUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminIdentityUser, adminPass);
            if (!createResult.Succeeded)
            {
                var errorMsg = string.Join("; ", createResult.Errors.Select(e => e.Description));
                throw new Exception($"Không thể tạo admin user trong Identity: {errorMsg}");
            }
        }

        // Gán role "Admin" nếu chưa có
        if (!await userManager.IsInRoleAsync(adminIdentityUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminIdentityUser, "Admin");
        }

        // --- BƯỚC 3: ĐỒNG BỘ ADMIN VÀO BẢNG 'User' CỦA APP ---
        bool adminExistsInApp = await thuVienDbContext.Users.AnyAsync(u => u.Email == adminEmail);
        if (!adminExistsInApp)
        {
            var appAdminRole = await thuVienDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (appAdminRole != null)
            {
                var appAdminUser = new User
                {
                    Fullname = "Administrator",
                    Email = adminEmail,
                    Password = "hashed_by_identity", // Không dùng để đăng nhập
                    Phone = "0123456789",
                    RoleId = appAdminRole.Id,
                    CreatedAt = DateTime.UtcNow
                };

                thuVienDbContext.Users.Add(appAdminUser);
                await thuVienDbContext.SaveChangesAsync();
            }
        }

        // --- BƯỚC 4: GHI LOG (TÙY CHỌN) ---
        Console.WriteLine($"✅ Đã seed tài khoản admin: {adminEmail}");
    }
}