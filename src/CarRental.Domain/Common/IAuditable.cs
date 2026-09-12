namespace CarRental.Domain.Common;

public interface IAuditable
{
    DateTime CreatedDate { get; set; }
    int? CreatedBy { get; set; }
    DateTime? UpdatedDate { get; set; }
    int? UpdatedBy { get; set; }
}
