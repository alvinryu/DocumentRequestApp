using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class SendEmailVM
    {
        [Required]
        public string NIK { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        public string MessageSubject { get; set; }
        public string MessageBody { get; set; }
    }
}
