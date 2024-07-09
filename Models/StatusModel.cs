using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uds.Models;

[Table("status")]
public class StatusModel
{
    [Required, Key]
    public int Id { get; set; }

    [Required, StringLength(45)]
    public string Description { get; set; }
}
