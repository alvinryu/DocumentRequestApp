using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MVC.Base;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class AccountController : BaseController<Account, string>
    {
        private readonly HttpClient httpClient;

        public AccountController()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44303/api/")
            };
        }

        public ViewResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Account/Login", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseTokenVM>(apiResponse);
            return new JsonResult(result);
        }

        //public ViewResult ForgotPassword() => View();

        //public ViewResult Register() => View();
    }
}