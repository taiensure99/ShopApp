var builder = WebApplication.CreateBuilder(args);

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
    .ConfigureApiBehaviorOptions(options =>
    {
        // Tắt tính năng tự động trả về 400 khi Model không hợp lệ
        options.SuppressModelStateInvalidFilter = true;
    });

// Đăng ký các dịch vụ (Services) — học buổi 32
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware Pipeline — học buổi 32-33
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowSwagger");
app.MapControllers();

app.Run();