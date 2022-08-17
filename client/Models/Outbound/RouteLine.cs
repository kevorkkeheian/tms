namespace Application.Models.Outbound;


public class RouteLine : Detail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    public string? RouteHeaderId { get; set; }
    public RouteHeader? RouteHeader { get; set; }
    public int LineNo { get; set; }
    public string? StoreId { get; set; }
    public Store? Store { get; set; }
    public DateTime? ArrivedOn { get; set; }
    public DateTime? DepartedOn { get; set; }

}