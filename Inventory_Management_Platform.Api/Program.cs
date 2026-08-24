using Inventory_Management_Platform.Api.Extensions;
using Inventory_Management_Platform.Application;
using Inventory_Management_Platform.Infrastructure;
using Inventory_Management_Platform.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddSwaggerWithJwtAuth();
}

var app = builder.Build();
app.UseGlobalExceptionHandling();
app.UseCorrelationIdLogging();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
    await TestUserSeeder.SeedTestUsersAsync(scope.ServiceProvider);

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();