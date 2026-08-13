using Inventory_Management_Platform.Application;
using Inventory_Management_Platform.Infrastructure;

using Inventory_Management_Platform.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddControllers();


}

// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
    await TestUserSeeder.SeedTestUsersAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
