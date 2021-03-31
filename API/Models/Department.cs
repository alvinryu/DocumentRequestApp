using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("Tb_M_Department")]
    public class Department
    {
        [Key][Required]
        public int DepartmentID { get; set; }
        [Required]
        public string DepartmentName { get; set; }
        [Required]
        public string Manager_NIK { get; set; }
        
        public string HR_NIK { get; set; }
        [JsonIgnore]
        public virtual ICollection<Person> People { get; set; }
    }
}
