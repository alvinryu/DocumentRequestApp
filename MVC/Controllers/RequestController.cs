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
using System.IO;
using Wkhtmltopdf.NetCore;
using System.Linq;

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
        public ViewResult RequestDocumentHR() => View();
        public ViewResult RequestDocumentRM() => View();

        public async override Task<JsonResult> Post(Request request)
        {
            var header = Request.Headers["Authorization"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);

            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Request", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<Request>>(apiResponse);

            using var responsePerson = await httpClient.GetAsync("Person/" + result.Data.PersonNIK);
            string apiResponsePerson = await response.Content.ReadAsStringAsync();
            var resultPerson = JsonConvert.DeserializeObject<ResponseVM<Person>>(apiResponse);

            using var rmResponse = await httpClient.GetAsync("Person/" + resultPerson.Data.Department.Manager_NIK);
            string rmApiResponse = await rmResponse.Content.ReadAsStringAsync();
            var rmResult = JsonConvert.DeserializeObject<ResponseVM<Person>>(rmApiResponse);
            var emailRM = rmResult.Data.Email;

            var mailSubject = "Document Request Notification";
            var mailBody = "<h2>Your Employee has been request a document.</h2>" + "<br><br>Please check this request.";
            var sendEmailVM = new SendEmailVM { Email = emailRM, MessageSubject = mailSubject, MessageBody = mailBody };
            var sendEmailResult = await SendEmail(sendEmailVM);

            return new JsonResult(result);
        }

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

        [HttpPost]
        public async Task<string> ApproveOrRejectByHR(ApproveOrRejectVM approveReject)
        {
            var header = Request.Headers["Authorization"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);

            StringContent content = new StringContent(JsonConvert.SerializeObject(approveReject), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("Request/ApproveOrRejectByHR", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<ApproveOrRejectVM>>(apiResponse);
            if (approveReject.Approve == 1)
            {
                var mailSubject = "Document Request Notification";
                var mailBody = "<h2>Your Document Has Been Approved</h2>" + "<br><br>You can download this document.";
                var sendEmailVM = new SendEmailVM { Email = approveReject.Email, MessageSubject = mailSubject, MessageBody = mailBody };
                var sendEmailResult = await SendEmail(sendEmailVM);

                return sendEmailResult.Status;
            }else if (approveReject.Approve == 0){
                var mailSubject = "Document Request Notification";
                var mailBody = "<h2>Your Document Has Been Rejected</h2>" + "<br><br>Please contact your Manager for details.";
                var sendEmailVM = new SendEmailVM { Email = approveReject.Email, MessageSubject = mailSubject, MessageBody = mailBody };
                var sendEmailResult = await SendEmail(sendEmailVM);

                return sendEmailResult.Status;
            }

            return "500";
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

        public async Task<FileStreamResult> Doc(int RequestID)
        {
            var header = Request.Headers["Authorization"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);

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
        public async Task<string> ApproveOrRejectByRM(ApproveOrRejectVM approveReject)
        {
            var header = Request.Headers["Authorization"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);

            StringContent content = new StringContent(JsonConvert.SerializeObject(approveReject), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("Request/ApproveOrRejectByRM", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<ApproveOrRejectVM>>(apiResponse);

            using var responsePerson = await httpClient.GetAsync("Person/" + approveReject.HR_NIK);
            string apiResponsePerson = await response.Content.ReadAsStringAsync();
            var resultPerson = JsonConvert.DeserializeObject<ResponseVM<Person>>(apiResponse);

            using var responseDepartment = await httpClient.GetAsync("Department/" + resultPerson.Data.DepartmentID);
            string apiResponseDepartment = await responseDepartment.Content.ReadAsStringAsync();
            var departmentResult = JsonConvert.DeserializeObject<ResponseVM<Department>>(apiResponseDepartment);

            using var responsePersonHR = await httpClient.GetAsync("Person/" + departmentResult.Data.HR_NIK);
            string apiResponsePersonHR = await response.Content.ReadAsStringAsync();
            var resultPersonHR = JsonConvert.DeserializeObject<ResponseVM<Person>>(apiResponse);

            var emailHR = resultPersonHR.Data.Email;

            if (approveReject.Approve == 1)
            {
                var mailSubject = "Document Request Notification";
                var mailBody = "<h2>A Manager Has Been Approved A Document Request</h2>" + "<br><br>Please check this document.";
                var sendEmailVM = new SendEmailVM { Email = emailHR, MessageSubject = mailSubject, MessageBody = mailBody };
                var sendEmailResult = await SendEmail(sendEmailVM);

                return sendEmailResult.Status;
            }
            else if (approveReject.Approve == 0)
            {
                var mailSubject = "Document Request Notification";
                var mailBody = "<h2>Your Document Has Been Rejected</h2>" + "<br><br>Please contact your Manager for details.";
                var sendEmailVM = new SendEmailVM { Email = approveReject.Email, MessageSubject = mailSubject, MessageBody = mailBody };
                var sendEmailResult = await SendEmail(sendEmailVM);

                return sendEmailResult.Status;
            }

            return "500";
        }
    }
}