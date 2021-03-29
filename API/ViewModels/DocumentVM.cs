using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class DocumentVM
    {
        public string HR_NIK { get; set; }
        public string HR_Name { get; set; }
        public string HR_Role { get; set; }
        public string Employee_NIK { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_Role { get; set; }
        public string Employee_Department { get; set; }
        public string Employee_JoinDate { get; set; }
        public string ApprovalHRDate { get; set; }
        public string DocumentType { get; set; }
    }
}
