using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("Tb_T_Request")]
    public class Request
    {
        [Key][Required]
        public int RequestID { get; set; }
        [Required]
        public string NIK { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        [JsonIgnore]
        public virtual Person Person { get; set; }
        [JsonIgnore]
        public virtual DetailRequest DetailRequest { get; set; }
        [JsonIgnore]
        public virtual DocumentType DocumentType { get; set; }
    }
}
