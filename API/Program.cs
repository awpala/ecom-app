using API.Extensions;
using API.Middleware;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// N.B. cf. `/API/Extensions` for these extension methods
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors("CorsPolicy");

// N.B. In general, Authentication should be performed before Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// run migrations on app start
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var identityContext = services.GetRequiredService<AppIdentityDbContext>();
var userManager = services.GetRequiredService<UserManager<AppUser>>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
  await context.Database.MigrateAsync();
  await identityContext.Database.MigrateAsync();
  await StoreContextSeed.SeedAsync(context);
  await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
}
catch (Exception e)
{
  logger.LogError(e, "An error occurred during migration");
}

app.Run();
