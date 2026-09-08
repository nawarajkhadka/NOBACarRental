namespace CarRental.Application.Exceptions;

/// <summary>Thrown for application-level validation failures (not domain invariants).</summary>
public class ApplicationValidationException : Exception
{
    public ApplicationValidationException(string message) : base(message)
    {
    }
}

/// <summary>Thrown when a referenced entity (booking, car, etc.) cannot be found.</summary>
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message)
    {
    }
}
