using Authenticator_Application_Backend.Model;
using Authenticator_Application_Backend.Services;
using Authenticator_Application_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthenticatorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnectionString")));

builder.Services.AddSingleton(UrlEncoder.Default);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRegistration, UserRegistration>();
builder.Services.AddScoped<IGoogleAuthenticatorService, GoogleAuthenticatorService>();
builder.Services.AddScoped<IMicrosoftAuthenticatorService, MicrosoftAuthenticatorService>();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder
  .AllowAnyOrigin()
  .AllowAnyMethod()
  .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("corsapp");

app.Run();
