using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Base
{
    public class URL
    {
        public string GetProduction()
        {
            return "https://localhost:44361/api/";
        }

        public string GetDevelopment()
        {
            return "https://localhost:44361/api/";
        }
    }
}
