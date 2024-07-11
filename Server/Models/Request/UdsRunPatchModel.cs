using System.ComponentModel.DataAnnotations;

public class UdsRunPatchModel
{
    [Required]
    public int Id { get; set; }

    [Required, Range(1, 3)]
    public OrderAction Action { get; set; }
}
