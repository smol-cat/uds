using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uds.Models.Database;

[Table("status")]
public class StatusModel
{
    [Key]
    public int Id { get; set; }

    public string Description { get; set; }
}
