using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Healthy_Mink.Models
{
    public class Employee
    {
        //Поставил автоинкремент для удобства работы 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MinLength(1, ErrorMessage = "Name's too short")]
        [MaxLength(50, ErrorMessage = "Name's too long")]
        public string FirstName { get; set; } = null!;
        [MinLength(1, ErrorMessage = "Last name's too short")]
        [MaxLength(50, ErrorMessage = "Last name's too long")]
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public int RoleId { get; set; } 
        
    }
}
