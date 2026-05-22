using Microsoft.EntityFrameworkCore;
using SCDH.Data;
using SCDH.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=habitacao.db"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("SegurancaCaixa", policy =>
    {
        policy.WithOrigins("http://localhost:5100")
              .WithMethods("GET", "POST")
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<AuditoriaMiddleware>();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("SegurancaCaixa");
app.UseAuthorization();
app.MapControllers();

app.Run();