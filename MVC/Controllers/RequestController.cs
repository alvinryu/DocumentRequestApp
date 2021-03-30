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
using System.IO;
using Wkhtmltopdf.NetCore;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace MVC.Controllers
{
    public class RequestController : BaseController<Request, int>
    {
        private readonly IGeneratePdf _generatePdf;

        public RequestController(IGeneratePdf generatePdf)
        {
            _generatePdf = generatePdf;
        }

        public ViewResult RequestDocumentEmployee() => View();
        //public ViewResult RequestDocumentHR() => View();
        //public ViewResult RequestDocumentRM() => View();

        public async Task<JsonResult> GetRequestDocumentEmployee(string NIK)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            using var response = await httpClient.GetAsync("Request/GetRequestForEmployee/?NIK="+NIK);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<RequestVM>>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<JsonResult> GetRequestForHR()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            using var response = await httpClient.GetAsync("Request/GetRequestForHR");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<RequestVM>>>(apiResponse);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> ApproveOrRejectByHR(ApproveOrRejectVM approveReject)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            StringContent content = new StringContent(JsonConvert.SerializeObject(approveReject), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("Request/ApproveOrRejectByHR", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<ApproveOrRejectVM>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<JsonResult> GetRequestForRM(int DepartmentID)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            using var response = await httpClient.GetAsync("Request/GetRequestForRM/?DepartmentID="+ DepartmentID);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<RequestVM>>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<FileStreamResult> Doc(int RequestID)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            var requestResponse = await GetById(RequestID);
            var requestApiResponse = JsonConvert.SerializeObject(requestResponse.Value);
            var requestResult = JsonConvert.DeserializeObject<ResponseVM<Request>>(requestApiResponse);

            using var detailRequestResponse = await httpClient.GetAsync("DetailRequest/" + RequestID);
            string detailRequestApiResponse = await detailRequestResponse.Content.ReadAsStringAsync();
            var detailRequestResult = JsonConvert.DeserializeObject<ResponseVM<DetailRequest>>(detailRequestApiResponse);

            using var hrResponse = await httpClient.GetAsync("Person/" + detailRequestResult.Data.HR_NIK);
            string hrApiResponse = await hrResponse.Content.ReadAsStringAsync();
            var hrResult = JsonConvert.DeserializeObject<ResponseVM<Person>>(hrApiResponse);

            using var employeeResponse = await httpClient.GetAsync("Person/" + requestResult.Data.PersonNIK);
            string employeeApiResponse = await employeeResponse.Content.ReadAsStringAsync();
            var employeeResult = JsonConvert.DeserializeObject<ResponseVM<Person>>(employeeApiResponse);

            ICollection<AccountRole> hrRole = hrResult.Data.Account.AccountRoles.ToList();
            
            using var hrRoleResponse = await httpClient.GetAsync("Role/" + hrRole.FirstOrDefault().RoleID);
            string hrRoleApiResponse = await hrRoleResponse.Content.ReadAsStringAsync();
            var hrRoleResult = JsonConvert.DeserializeObject<ResponseVM<Role>>(hrRoleApiResponse);

            ICollection<AccountRole> employeeRole = employeeResult.Data.Account.AccountRoles.ToList();

            using var employeeRoleResponse = await httpClient.GetAsync("Role/" + employeeRole.FirstOrDefault().RoleID);
            string employeeRoleApiResponse = await employeeRoleResponse.Content.ReadAsStringAsync();
            var employeeRoleResult = JsonConvert.DeserializeObject<ResponseVM<Role>>(employeeRoleApiResponse);

            var Document = new DocumentVM
            {
                HR_NIK = hrResult.Data.NIK,
                HR_Name = hrResult.Data.FirstName + " " + hrResult.Data.LastName,
                HR_Role = hrRoleResult.Data.RoleName,
                Employee_NIK = employeeResult.Data.NIK,
                Employee_Name = employeeResult.Data.FirstName + " " + employeeResult.Data.LastName,
                Employee_Role = employeeRoleResult.Data.RoleName,
                Employee_Department = employeeResult.Data.Department.DepartmentName,
                Employee_JoinDate = employeeResult.Data.JoinDate.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID")),
                ApprovalHRDate = detailRequestResult.Data.ApproveHRDate.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID")),
                DocumentType = requestResult.Data.DocumentType.TypeName
            };

            byte[] test = await _generatePdf.GetByteArray("Views/Request/Document.cshtml", Document);

            MemoryStream stream = new MemoryStream(test);
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = "Sample.pdf"
            };
            
            return fileStreamResult;
        }

        [HttpPost]
        public async Task<JsonResult> ApproveOrRejectByRM(ApproveOrRejectVM approveReject)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            StringContent content = new StringContent(JsonConvert.SerializeObject(approveReject), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("Request/ApproveOrRejectByRM", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<ApproveOrRejectVM>>(apiResponse);
            return new JsonResult(result);
        }

        public ViewResult Index()
        {
            return (HttpContext.Session.GetString("role") == "HR") ? View() : View("../Authorize/NotAuthorized");
        }

        public ViewResult RequestDocumentHR()
        {
            return (HttpContext.Session.GetString("role") == "HR") ? View() : View("../Authorize/NotAuthorized");
        }

        public ViewResult RequestDocumentRM()
        {
            return (HttpContext.Session.GetString("role") == "RM") ? View() : View("../Authorize/NotAuthorized");
        }
    }
}