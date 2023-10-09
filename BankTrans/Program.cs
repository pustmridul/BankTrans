using BankTrans.Data;
using BankTrans.Models;
using BankTrans.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddSwaggerGen(c =>
{

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Bank Trans  Api",
        License = new OpenApiLicense()
        {

        }
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
               .AddJwtBearer(o =>
               {
                   o.RequireHttpsMetadata = false;
                   o.SaveToken = false;
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,
                       ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
                       ValidAudience = builder.Configuration["JWTSettings:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"]))
                   };

               });


//builder.Services.AddAuthentication("StaticTokenAuthentication")
//       .AddScheme<AuthenticationSchemeOptions, StaticTokenAuthenticationHandler>("StaticTokenAuthentication", options => { });


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


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
