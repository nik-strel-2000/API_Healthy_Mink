using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_Healthy_Mink.Models.ValidModel
{
    public class EmployeeValind
    {
        public int id {  get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
