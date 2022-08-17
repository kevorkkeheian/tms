using Application.Models.Warehouse;

namespace Application.Models.Outbound;



public class Consolidation : Detail
{
    public string? Id { get; set ; }
    public string? StoreId { get; set; }
    public Store? Store { get; set; }
    public string? WorkDetailId { get; set; }
    public WorkDetail? WorkDetail { get; set; }
    public string LicensePlateId { get; set; }
    public LicensePlate? LicensePlate { get; set; }

    public ConsolidationStatus ConsolidationStatus { get; set; }

}