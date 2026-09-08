namespace CarRental.Domain.Exceptions;

/// <summary>Thrown when a domain invariant is violated (e.g. invalid meter readings or dates).</summary>
public class DomainValidationException : Exception
{
    public DomainValidationException(string message) : base(message)
    {
    }
}
