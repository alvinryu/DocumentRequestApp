using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("Tb_M_Person")]
    public class Person
    {
        [Key][Required]
        public string NIK { get; set; }
        [Required] 
        public string KTP { get; set; }
        [Required] 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required] 
        public string Phone { get; set; }
        [Required] 
        public DateTime BirthDate { get; set; }
        [Required] 
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required] 
        public string Address { get; set; }
        [Required] 
        public DateTime JoinDate { get; set; }
        [Required]
        public int DepartmentID { get; set; }
        //[JsonIgnore]
        public virtual Account Account { get; set; }
        //[JsonIgnore]
        public virtual Department Department { get; set; }
        [JsonIgnore]
        public virtual ICollection<Request> Requests { get; set; }

    }
}
