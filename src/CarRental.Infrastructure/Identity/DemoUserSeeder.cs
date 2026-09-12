using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarRental.Infrastructure.Identity;

/// <summary>Seeds one demo user per role on application startup, for local/dev login only.</summary>
public class DemoUserSeeder : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DemoUserSeeder(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await EnsureUserAsync(userManager, "agent", "agent@carrental.local", "Demo", "Agent", "Agent123!", Roles.Agent);
        await EnsureUserAsync(userManager, "manager", "manager@carrental.local", "Demo", "Manager", "Manager123!", Roles.Manager);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string userName,
        string email,
        string firstName,
        string lastName,
        string password,
        string role)
    {
        if (await userManager.FindByNameAsync(userName) is not null)
        {
            return;
        }

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            FirstName = firstName,
            LastName = lastName
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
