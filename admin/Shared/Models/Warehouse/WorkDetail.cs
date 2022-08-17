namespace Application.Shared.Models.Warehouse;


public class WorkDetail : Detail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    public string WorkId { get; set; }

    public string DocumentNo { get; set; }

    public string StoreId { get; set; }
    public Store Store { get; set; }

    public string LicensePlate { get; set; }

    public LicensePlateStatus? LicensePlateStatus { get; set; }
}
