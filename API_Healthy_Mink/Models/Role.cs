using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Healthy_Mink.Models
{
    public class Role
    {
        //Из-за отсутствия таблицы сущностей для пояснения бд решил сделать через id 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartShift { get; set; }
        public DateTime EndShift { get; set; }
    }
}
