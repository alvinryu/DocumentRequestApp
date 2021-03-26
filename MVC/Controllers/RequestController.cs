﻿using API.Models;
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
            var header = Request.Headers["Authorization"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);

            using var response = await httpClient.GetAsync("Request/GetRequestForEmployee/?NIK="+NIK);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<RequestVM>>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<JsonResult> GetRequestForHR()
        {
            var header = Request.Headers["Authorization"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);

            using var response = await httpClient.GetAsync("Request/GetRequestForHR");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<RequestVM>>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<JsonResult> GetRequestForRM(int DepartmentID)
        {
            var header = Request.Headers["Authorization"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);

            using var response = await httpClient.GetAsync("Request/GetRequestForRM/?DepartmentID="+ DepartmentID);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<RequestVM>>>(apiResponse);
            return new JsonResult(result);
        }

    }
}