using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class AuthorizeController : Controller
    {
        public ViewResult NotAuthorized() => View();
    }
}