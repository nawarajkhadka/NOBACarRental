namespace CarRental.Domain.Entities;

public class Customer
{
    public int Id { get; set; }

    // PII: never log, expose beyond what's strictly needed, or include in exception messages.
    public string SocialSecurityNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}
