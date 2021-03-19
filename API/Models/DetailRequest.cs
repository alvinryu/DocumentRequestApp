using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("Tb_T_DetailRequest")]
    public class DetailRequest
    {
        [Key][Required]
        //public int DetailRequestID { get; set; }
        public int RequestID { get; set; }
        [Required] 
        public int ApproveRM { get; set; }
        [Required] 
        public int ApproveHR { get; set; }
        [Required] 
        public string HR_NIK { get; set; }
        [Required] 
        public DateTime ApproveRMDate { get; set; }
        [Required]
        public DateTime ApproveHRDate { get; set; }
        [JsonIgnore]
        public virtual Request Request { get; set; }
    }
}
