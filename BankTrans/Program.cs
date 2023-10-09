using BankTrans.Data;
using BankTrans.Models;
using BankTrans.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "",
            ValidAudience = "",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<ITransService, TransService>();

Log.Logger = new LoggerConfiguration()
       .MinimumLevel.Debug()
       .WriteTo.Logger(c => c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
       .WriteTo.File($"D:/Logs/serilog/DEBUG.log", rollingInterval: RollingInterval.Year))
       .WriteTo.Logger(c => c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
       .WriteTo.File($"D:/Logs/serilog/Info.log", rollingInterval: RollingInterval.Year))
       .WriteTo.Logger(c => c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
       .WriteTo.File($"D:/Logs/serilog/ERROR.log", rollingInterval: RollingInterval.Year))
       .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
