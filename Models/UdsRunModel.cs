using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Uds.Models;

[Table("run")]
public class UdsRunModel
{
    [Required, Key]
    public int Id { get; set; }

    [Required, ForeignKey("fk_run_orderId")]
    public int OrderId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required, ForeignKey("fk_run_statusId")]
    [JsonIgnore]
    public int? StatusId { get; set; }

    public string Status { get; set; }

    public UdsRunModel() { }
}
