using System.ComponentModel.DataAnnotations;

namespace CarRental.Domain.Entities;

public class Customer
{
    public int Id { get; set; }

    // PII: never log, expose beyond what's strictly needed, or include in exception messages.
    [Required(AllowEmptyStrings = false)]
    [StringLength(20)]
    public string SocialSecurityNumber { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(30)]
    public string? PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}
