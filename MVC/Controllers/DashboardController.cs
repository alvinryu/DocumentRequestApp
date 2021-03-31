using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Base;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class DashboardController : Controller
    {
        protected readonly HttpClient httpClient;

        public DashboardController()
        {
            URL url = new URL();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(url.GetDevelopment())
            };
        }

        public async Task<JsonResult> ChartRole()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            using var response = await httpClient.GetAsync("Dashboard/ChartRole");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<ChartVM>>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<JsonResult> ChartDocType()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            using var response = await httpClient.GetAsync("Dashboard/ChartDocType");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<ChartVM>>>(apiResponse);
            return new JsonResult(result);
        }
    }
}