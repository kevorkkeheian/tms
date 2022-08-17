namespace Application.Shared.Models.Outbound;



public class LoadLine : Detail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    public string? LoadHeaderId { get; set; }
    public LoadHeader? LoadHeader { get; set; }

    public int LineNo { get; set; }

    // public string? ConsolidationId { get; set; }
    // public Consolidation? Consolidation { get; set; }

    public string? LicensePlateId { get; set; }
    public LicensePlate? LicensePlate { get; set; }
    public LoadStatus LoadStatus { get; set; }

}