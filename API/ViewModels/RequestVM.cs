using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class RequestVM
    {
        public int RequestID { set; get; }
        public DateTime RequestDate { set; get; }
        public string Name { set; get; }
        public int TypeID { set; get; }
        public string TypeName { set; get; }
        public int ApproveRM { set; get; }
        public int ApproveHR { set; get; }
        public string HR_NIK { set; get; }
        public string HR_Name { set; get; }
        public DateTime ApproveRMDate { set; get; }
        public DateTime ApproveHRDate { set; get; }
        public string Email { set; get; }
    }
}
