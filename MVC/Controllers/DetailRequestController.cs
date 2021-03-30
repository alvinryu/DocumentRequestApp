using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Base;


namespace MVC.Controllers
{
    public class DetailRequestController : BaseController<DetailRequest, int>
    {
        public DetailRequestController()
        {

        }

        public ViewResult Index()
        {
            return (HttpContext.Session.GetString("role") == "HR") ? View() : View("../Authorize/NotAuthorized");
        }
    }
}