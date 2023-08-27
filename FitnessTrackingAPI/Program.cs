using ExpenseTrackingAPI.DataContext;
using ExpenseTrackingAPI.Helpers;
using ExpenseTrackingAPI.Interfaces;
using ExpenseTrackingAPI.Models;
using ExpenseTrackingAPI.DbModels;
using ExpenseTrackingAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string origins = builder.Configuration["Values:CorsAllowedOrigins"];
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey));

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins().AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
}));

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = false,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidateAudience = false,
                    ValidAudience = jwtSettings.ValidAudience,

                };
            });
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ExpenseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITransactions, TransactionsServ>();
builder.Services.AddScoped<IAccountServ, AccountServ>();
//builder.Services.AddScoped<ILoggerService, LogService>();
builder.Services.AddScoped<TokenServ>(provider =>
    new TokenServ(provider.GetRequiredService<IConfiguration>(), provider.GetRequiredService<ExpenseContext>()));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(configurePolicy =>
{
    configurePolicy.AllowAnyOrigin();
    configurePolicy.AllowAnyMethod();
    configurePolicy.AllowAnyHeader();
});
app.UseCors("AllowAngularApp");

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
