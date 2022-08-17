

namespace Application.Models;

public class Detail
{
    public DateTime? CreatedOn { get; set; } = DateTime.Now;
    public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    public ObjectStatus Status { get; set; }
}