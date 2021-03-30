using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
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

            if(result.Status == "200")
            {
                HttpContext.Session.SetString("token", result.Token);
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("token"));
                var role = jsonToken.Claims.First(claim => claim.Type == "role").Value;
                HttpContext.Session.SetString("role", role);
            }

            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<string> ForgotPassword(string Email)
        {
            var emailStatus = await CheckPersonByEmail(Email);
            string statusCode = "";
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

                    var sendEmailResult = await SendEmail(sendEmailVM);
                    statusCode = sendEmailResult.Status;
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

        public ViewResult ChangePassword() => View();

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePassword)
        {
            var _response = await GetById(changePassword.NIK);
            var _apiResponse = JsonConvert.SerializeObject(_response.Value);
            var _result = JsonConvert.DeserializeObject<ResponseVM<Account>>(_apiResponse);
            var result = new ResponseVM<Account>();

            if (_result.Data.Password == changePassword.OldPassword)
            {
                var updateAccount = new LoginVM { NIK = changePassword.NIK, Password = changePassword.NewPassword };

                StringContent content = new StringContent(JsonConvert.SerializeObject(updateAccount), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync("Account", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<ResponseVM<Account>>(apiResponse);
            }
            else
            {
                result.Status = "500";
            }

            return new JsonResult(result);
        }

        [HttpPost]
        public async override Task<JsonResult> Post(Account account)
        {
            var header = Request.Headers["Authorization"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);

            account.Password = Guid.NewGuid().ToString();
            StringContent content = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Account", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<Account>>(apiResponse);

            if (result.Status == "200")
            {
                var _response = await httpClient.GetAsync("Person/" + account.NIK);
                string _apiResponse = await _response.Content.ReadAsStringAsync();
                var _result = JsonConvert.DeserializeObject<ResponseVM<Person>>(_apiResponse);

                var password = account.Password;
                var mailSubject = "New Account Notification";
                var mailBody = "<h2>This email has been registered by our HR Division!</h2> Use this temporary password to login : " + password + "<br><br>We recommend you to change the temporary password in HRIS immediately";
                var sendEmailVM = new SendEmailVM { Email = _result.Data.Email, MessageSubject = mailSubject, MessageBody = mailBody };

                var sendEmailResult = await SendEmail(sendEmailVM);
                result.Status = sendEmailResult.Status;
            }

            return new JsonResult(result);
        }

        //public ViewResult ForgotPassword() => View();

    }
}