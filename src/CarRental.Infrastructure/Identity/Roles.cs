namespace CarRental.Infrastructure.Identity;

public static class Roles
{
    public const string Agent = "Agent";
    public const string Manager = "Manager";

    public static readonly string[] All = { Agent, Manager };
}
