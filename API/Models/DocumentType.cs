using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("Tb_M_DocumentType")]
    public class DocumentType
    {
        [Key][Required]
        public int TypeID { get; set; }
        [Required] 
        public string TypeName { get; set; }
        [JsonIgnore]
        public virtual ICollection<Request> Requests { get; set; }
    }
}
