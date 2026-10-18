using Microsoft.EntityFrameworkCore;
using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Repositories;
using ShopApp.Repositories.Interfaces;
using ShopApp.Services;
using System.Diagnostics;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

//tạo conection string dưới DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<ShopAppDbContext>(options => options.UseSqlServer(connectionString));

//giúp các class ví dụ có repository đươc inject vào tat cả các controller, service, v.v. mà không cần phải khai báo từng cái một
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repositorty<>));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwagger", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // Tắt tính năng tự động trả về 400 khi Model không hợp lệ
        options.SuppressModelStateInvalidFilter = true;
    });

//Đăng ký service =< để controller sử dụng nè
builder.Services.AddScoped<ISanPhamService, SanPhamService>();
builder.Services.AddScoped<IThongKeService, ThongKeService>();
builder.Services.AddTransient<IGuidService, GuidServiceTransient>();
builder.Services.AddScoped<IGuidService, GuidServiceScoped>();
builder.Services.AddSingleton<IGuidService, GuidServiceSingleton>();
//Repository
builder.Services.AddScoped<ISanPhamRepository, SanPhamRepository>();
builder.Services.AddScoped<IKhachHangRepository, KhachHangRepository>();

// Đăng ký các dịch vụ (Services) — học buổi 32
builder.Services.AddScoped<IRateLimitService, RateLimitService>();
builder.Services.AddScoped<IDonHangService, DonHangService>();
builder.Services.AddScoped<IDatHangService, DatHangService>();
builder.Services.AddScoped<IDanhGia, DanhGiaService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();
builder.Services.AddScoped<ISanPhamKhuyenMaiService, SanPhamKhuyenMaiService>();
builder.Services.AddScoped<IThongBaoService, ThongBaoConsoleService>();
builder.Services.AddScoped<IThongBaoService, ThongBaoLogService>();





var app = builder.Build();

app.Use(async (context, next) =>
{
    var rateLimitService =
        context.RequestServices.GetRequiredService<IRateLimitService>();

    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

    if (rateLimitService.KiemTraVuotGioiHan(ip))
    {
        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        await context.Response.WriteAsync(
            "Bạn đã vượt quá giới hạn request. Vui lòng thử lại sau."
        );

        return; // Không cho request chạy tiếp
    }

    rateLimitService.GhiNhanRequest(ip);

    await next(); // Cho request chạy tiếp
});


// Middleware Pipeline — học buổi 32-33
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    // Log thông tin request
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");

    await next.Invoke();

    // Log thông tin response
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});


app.Use(async (context, next) =>
{
    var stopwatch = Stopwatch.StartNew();
    var thoiGianGoi = DateTime.Now;
    var httpMethod = context.Request.Method;

    await next();

    stopwatch.Stop();

    var Status = context.Response.StatusCode;
    Console.WriteLine($"{thoiGianGoi:dd/MM/yyyy HH:mm:ss} Method: {httpMethod} Status: {Status} {stopwatch.ElapsedMilliseconds}ms");
});




app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowSwagger");
app.MapControllers();

app.Run();