using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MVC.Base
{
    public class BaseController<Entity, Key> : Controller
    {
        protected readonly HttpClient httpClient;

        public BaseController()
        {
            URL url = new URL();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(url.GetDevelopment())
            };
        }

        //public ViewResult Index() => View();

        public async Task<JsonResult> Get()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            
            using var response = await httpClient.GetAsync(typeof(Entity).Name);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<Entity>>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<JsonResult> GetById(Key key)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            var response = await httpClient.GetAsync(typeof(Entity).Name + "/" + key);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<Entity>>(apiResponse);
            return new JsonResult(result);
        }

        [HttpPost]
        public async virtual Task<JsonResult> Post(Entity entity)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(typeof(Entity).Name, content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Entity>(apiResponse);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> Put(Entity entity)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(typeof(Entity).Name, content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<Entity>>(apiResponse);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Key key)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            using var response = await httpClient.DeleteAsync(typeof(Entity).Name + '/' + key);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<Entity>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<ResponseVM<SendEmailVM>> SendEmail(SendEmailVM sendEmailVM)
        {
            //var mailSubject = "Forgot Password Notification";
            //var mailBody = "<h2>Your Password was changed!</h2> Use this temporary password to login : " + password + "<br><br>We recommend you to change the temporary password in HRIS immediately";
            //var sendEmailVM = new SendEmailVM { Email = Email, MessageSubject = mailSubject, MessageBody = mailBody };

            StringContent content = new StringContent(JsonConvert.SerializeObject(sendEmailVM), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(typeof(Entity).Name + "/SendEmail", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<SendEmailVM>>(apiResponse);
            return result;
        }
    }
}