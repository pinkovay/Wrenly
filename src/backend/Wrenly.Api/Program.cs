using Microsoft.EntityFrameworkCore;
using Wrenly.Api.Extensions;
using Wrenly.Domain.Common.Email;
using Wrenly.Infrastructure.Auth.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AuthDb")
    );
});

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.GmailOptionskey));

builder.Services.AddControllers();

builder.Services.AddAuthConfiguration();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapAuthEndpoints();

app.MapGet("/health", () => Results.Ok(new {status = "ok"}))
    .WithName("healthCheck - Application in early development");

app.Run();
