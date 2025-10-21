using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Data;
using QLThuvien.Models;
using QLThuvien.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddDbContext<ThuVienDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ThuVienDbContext")));

// --- CÁC DỊCH VỤ CHO API ---
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IDichVuDatPhong, DichVuDatPhong>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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

var culture = "vi-VN";
var locOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(culture)
    .AddSupportedCultures(culture)
    .AddSupportedUICultures(culture);

app.UseRequestLocalization(locOptions);
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// --- LOGIC SEED DỮ LIỆU ---
// Gọi IdentitySeeder để tạo role và user admin
await IdentitySeeder.SeedAsync(app.Services);

// Khối using này chỉ dùng để seed phòng, không tự động migrate
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Dòng 'await db.Database.MigrateAsync();' đã được XÓA BỎ khỏi đây

    // Seed 6 phòng mẫu (nếu database chưa có)
    var roomService = services.GetRequiredService<IRoomService>();
    await roomService.SeedRoomsAsync();
}

app.Run();

