using Application.Shared.Models.Enum;

namespace Application.Shared.Models;

public class Detail
{
    public DateTime? CreatedOn { get; set; } = DateTime.Now;
    public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    public ObjectStatus Status { get; set; }
}