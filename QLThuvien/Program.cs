using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Data;
using QLThuvien.Models;
using QLThuvien.Services;
using System.Text.Json.Serialization;
using QLThuvien.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// 🔗 Kết nối database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ThuVienDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ThuVienDbContext")));

// 🧠 Identity & Role
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 🌐 Cấu hình JSON & vòng lặp tham chiếu
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// 🛠 Dịch vụ ứng dụng
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IDichVuDatPhong, DichVuDatPhong>();
builder.Services.AddScoped<IDichVuViPham, DichVuViPham>();
builder.Services.AddScoped<IDichVuMuonTra, DichVuMuonTra>();

// 🔄 Dịch vụ chạy nền
builder.Services.AddHostedService<DichVuCapNhatTrangThaiPhong>();
builder.Services.AddHostedService<DichVuKiemTraNoShow>();
// builder.Services.AddHostedService<DichVuThongBaoSachDatTruoc>();

// 🔐 Logging & Swagger
builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🌍 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 🚀 Pipeline cấu hình
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// 🌐 Localization
var culture = "vi-VN";
var locOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(culture)
    .AddSupportedCultures(culture)
    .AddSupportedUICultures(culture);

app.UseRequestLocalization(locOptions);

// 🔐 Middleware
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 🧭 Routing
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// 🌱 Seed dữ liệu
await IdentitySeeder.SeedAsync(app.Services);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roomService = services.GetRequiredService<IRoomService>();
    await roomService.SeedRoomsAsync();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();