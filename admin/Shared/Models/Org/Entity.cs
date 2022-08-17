namespace Application.Shared.Models.Org;


public class Entity : Detail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Code { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }
}