using API.Models;
using MVC.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.ViewModels;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace MVC.Controllers
{
    public class RequestController : BaseController<Request, int>
    {
        public RequestController()
        {

        }

        public ViewResult RequestDocumentEmployee() => View();
        public ViewResult RequestDocumentHR() => View();
        public ViewResult RequestDocumentRM() => View();

        public async Task<JsonResult> GetRequestDocumentEmployee(string NIK)
        {

            using var response = await httpClient.GetAsync("Request/GetRequestForEmployee/?NIK="+NIK);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<RequestVM>>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<JsonResult> GetRequestForHR()
        {

            using var response = await httpClient.GetAsync("Request/GetRequestForHR");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<RequestVM>>>(apiResponse);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> ApproveOrRejectByHR(ApproveOrRejectVM approveReject)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(approveReject), Encoding.UTF8, "application/json");
            
            var response = await httpClient.PutAsync("Request/ApproveOrRejectByHR", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<ApproveOrRejectVM>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<JsonResult> GetRequestForRM(int DepartmentID)
        {
            
            using var response = await httpClient.GetAsync("Request/GetRequestForRM/?DepartmentID="+ DepartmentID);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<RequestVM>>>(apiResponse);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> ApproveOrRejectByRM(ApproveOrRejectVM approveReject)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(approveReject), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("Request/ApproveOrRejectByRM", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<ApproveOrRejectVM>>(apiResponse);
            return new JsonResult(result);
        }
    }
}