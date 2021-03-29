using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class ApproveOrRejectVM
    {
        public int Approve { set; get; }
        public int RequestID { set; get; } 
        public int HR_NIK { set; get; } 
        public DateTime ApproveHRDate { set; get; }
        public DateTime ApproveRMDate { set; get; }
    }
}
