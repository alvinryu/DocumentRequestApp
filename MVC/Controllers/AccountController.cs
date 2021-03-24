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

        [HttpPost]
        public async Task<string> ForgotPassword(string Email)
        {
            var emailStatus = await CheckPersonByEmail(Email);
            var statusCode = emailStatus.Status;
            if (emailStatus.Status == "200")
            {
                var password = Guid.NewGuid().ToString();
                var _response = await Put(new Account { NIK = emailStatus.Data.NIK, Password = password });
                var _apiResponse = JsonConvert.SerializeObject(_response.Value); 
                var _result = JsonConvert.DeserializeObject<ResponseVM<SendEmailVM>>(_apiResponse);
                
                if (_result.Status == "200")
                {
                    var mailSubject = "Forgot Password Notification";
                    var mailBody = "<h2>Your Password was changed!</h2> Use this temporary password to login : " + password + "<br><br>We recommend you to change the temporary password in HRIS immediately";

                    var sendEmailVM = new SendEmailVM { Email = Email, MessageSubject = mailSubject, MessageBody = mailBody };

                    StringContent content = new StringContent(JsonConvert.SerializeObject(sendEmailVM), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("Account/SendEmail", content);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResponseVM<SendEmailVM>>(apiResponse);
                    statusCode = result.Status;
                }
                else
                {
                    statusCode = "500";
                }
            }

            return statusCode;
        }

        public async Task<ResponseVM<Person>> CheckPersonByEmail(string Email)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(Email), Encoding.UTF8, "application/json");

            using var response = await httpClient.PostAsync("Person/CheckEmail/?Email=" + Email, content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<Person>>(apiResponse);
            return result;
        }

        //public ViewResult ForgotPassword() => View();

        //public ViewResult Register() => View();
    }
}