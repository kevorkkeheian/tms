namespace Application.Shared.Models.Outbound;



public class LoadHeader : Detail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set ; }
    public string? DocumentNo { get; set; }
    public DateTime? LoadedOn { get; set; }
    public string? RouteHeaderId { get; set; }
    public RouteHeader? RouteHeader { get; set; }

    public LoadStatus LoadStatus { get; set; }

}