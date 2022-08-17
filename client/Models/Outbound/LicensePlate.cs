namespace Application.Models.Outbound;

public class LicensePlate : Detail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public string? StoreId { get; set; }
    public Store? Store { get;  set; }
    public string? Code { get; set; }
    public LicensePlateStatus? LicensePlateStatus { get; set; }
}