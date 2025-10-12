// IdentitySeeder.cs
using Microsoft.AspNetCore.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        //Tạo các vai trò mặc định
        string[] roles = new[] { "Admin", "ThuThu", "DocGia" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        //Tạo admin mặc định
        var adminEmail = config["AdminUser:Email"] ?? "admin@thuvien.local";
        var adminPass = config["AdminUser:Password"] ?? "Admin@12345";

        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var create = await userManager.CreateAsync(admin, adminPass);
            if (!create.Succeeded)
                throw new Exception("Cannot create admin user: " + string.Join("; ", create.Errors.Select(e => e.Description)));
        }
        if (!await userManager.IsInRoleAsync(admin, "Admin"))
            await userManager.AddToRoleAsync(admin, "Admin");
    }
}
