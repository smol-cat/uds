using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("schedule")]
public class ScheduleModel
{
    [Key]
    public int Id { get; set; }

    public string SchedulePattern { get; set; }
}
