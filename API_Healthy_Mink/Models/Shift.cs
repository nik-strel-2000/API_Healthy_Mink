using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Healthy_Mink.Models
{
    public class Shift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartShift { get; set; } 
        public DateTime? EndShift { get; set; } 
        public double? NumberHours { get; set; }
    }
}
