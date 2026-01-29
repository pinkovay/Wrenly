using Microsoft.EntityFrameworkCore;
using Wrenly.Api.Extensions;
using Wrenly.Infrastructure.Auth.Identity;
using Wrenly.Infrastructure.Email;
using Wrenly.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDb"));
});

// Configurações via Extensions
builder.Services.AddEmailInfrastructure(builder.Configuration);
builder.Services.AddSecurityInfrastructure(builder.Configuration);
builder.Services.AddAuthConfiguration().AddExternalProviders(builder.Configuration);
builder.Services.AddSwaggerConfiguration();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseCors();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.MapAuthEndpoints();

app.MapGet("/health", () => Results.Ok(new {status = "ok"}))
    .WithName("healthCheck - Application in early development");

app.Run();
