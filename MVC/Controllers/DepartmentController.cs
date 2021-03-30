using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Base;

namespace MVC.Controllers
{
    public class DepartmentController : BaseController<Department, int>
    {
        public DepartmentController()
        {

        }

        public ViewResult Index()
        {
            return (HttpContext.Session.GetString("role") == "HR") ? View() : View("../Authorize/NotAuthorized");
        }
    }
}