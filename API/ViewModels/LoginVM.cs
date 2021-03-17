using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class LoginVM
    {
        public string Email { set; get; }
        public string Password { set; get; }
        public string FullName { set; get; }
        public IEnumerable<string> Roles { set; get; }
        public string RoleName { set; get; }
    }
}
