using API.Models;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Base;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class PersonController : BaseController<Person, string>
    {
        public PersonController()
        {

        }

        [HttpPost]
        public async Task<JsonResult> CheckPersonByEmail(string Email)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            StringContent content = new StringContent(JsonConvert.SerializeObject(Email), Encoding.UTF8, "application/json");

            using var response = await httpClient.PostAsync("Person/CheckEmail/?Email=" + Email, content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<Person>>(apiResponse);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> CheckPersonByKTP(string KTP)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            StringContent content = new StringContent(JsonConvert.SerializeObject(KTP), Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync("Person/CheckKTP/?KTP=" + KTP, content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<Person>>(apiResponse);
            return new JsonResult(result);
        }

        public ViewResult Index() => View();
        public ViewResult Profile() => View();

        public async Task<JsonResult> GetAllRM()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            using var response = await httpClient.GetAsync("Person/GetAllRM");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<Person>>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<JsonResult> GetAllHR()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            using var response = await httpClient.GetAsync("Person/GetAllHR");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<Person>>>(apiResponse);
            return new JsonResult(result);
        }
    }
}