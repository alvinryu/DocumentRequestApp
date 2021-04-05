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

        string MessageSubject;
        string MessageBody;

        public async Task ApprovalNotificationRMAsync(string emailRM, string EmployeeName, string RMName, string dateRequest, string typeDoc)
        {
            MessageSubject = "New Document Request by " + EmployeeName;
            MessageBody = "Dear Mr/Ms/Mrs " + RMName
                + "<br><br>We would like to inform you that " + EmployeeName
                + " has request a document for the purpose of " + typeDoc
                + "'s administration on " + dateRequest + ". <br>We would like you to respond regarding the request on the HRIS on the Approval Document RM Section."
                + " <br>Thank you<br><br>Best Regards<br>HRIS Administration";

            var sendEmailVM = new SendEmailVM { Email = emailRM, MessageSubject = MessageSubject, MessageBody = MessageBody };
            var sendEmailResult = await SendEmail(sendEmailVM);
        }

        public async Task ApprovalNotificationHRAsync(string emailHR, string EmployeeName, string RMName, string HRName, string dateRequest, string typeDoc)
        {
            MessageSubject = "New Document Request by " + EmployeeName;
            MessageBody = "Dear Mr/Ms/Mrs " + HRName
                + "<br><br>We would like to inform you that " + EmployeeName
                + " has request a document for the purpose of " + typeDoc
                + "'s administration on " + dateRequest
                + ". The request has been approved by " + RMName + " as requester's RM. <br>"
                + ". We would like you to respond regarding the request on the HRIS on the Approval Document HR Section."
                + " <br>Thank you<br><br>Best Regards<br>HRIS Administration";

            var sendEmailVM = new SendEmailVM { Email = emailHR, MessageSubject = MessageSubject, MessageBody = MessageBody };
            var sendEmailResult = await SendEmail(sendEmailVM);
        }

        public async Task ApprovalNotificationEmployeeAsync(string email, int isApproved, string EmployeeName, string Name, string dateRespond, string typeDoc)
        {
            MessageSubject = "Document Request " + ((isApproved == 0) ? "Rejected" : "Approved");
            MessageBody = "Dear Mr/Ms/Mrs " + EmployeeName
                + "<br><br>We would like to inform you that " + Name
                + " had " + ((isApproved == 0) ? "rejected" : "approved") + " a document for the purpose of " + typeDoc
                + "'s administration on " + dateRespond
                + ". " + ((isApproved == 0) ? "" : "Please check on the HRIS on the Document Request Section to download the document")
                + " <br>Thank you<br><br>Best Regards<br>HRIS Administration";

            var sendEmailVM = new SendEmailVM { Email = email, MessageSubject = MessageSubject, MessageBody = MessageBody };
            var sendEmailResult = await SendEmail(sendEmailVM);
        }


        public async override Task<JsonResult> Post(Request request)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Request", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<Request>>(apiResponse);

            using var responsePerson = await httpClient.GetAsync("Person/" + request.PersonNIK);
            string apiResponsePerson = await responsePerson.Content.ReadAsStringAsync();
            var resultPerson = JsonConvert.DeserializeObject<ResponseVM<Person>>(apiResponsePerson);

            using var rmResponse = await httpClient.GetAsync("Person/" + resultPerson.Data.Department.Manager_NIK);
            string rmApiResponse = await rmResponse.Content.ReadAsStringAsync();
            var rmResult = JsonConvert.DeserializeObject<ResponseVM<Person>>(rmApiResponse);
            var emailRM = rmResult.Data.Email;

            using var docTypeResponse = await httpClient.GetAsync("DocumentType/" + request.DocumentTypeTypeID);
            string docTypeApiResponse = await docTypeResponse.Content.ReadAsStringAsync();
            var docResult = JsonConvert.DeserializeObject<ResponseVM<DocumentType>>(docTypeApiResponse);

            await ApprovalNotificationRMAsync(emailRM, resultPerson.Data.FirstName+" "+resultPerson.Data.LastName, rmResult.Data.FirstName+rmResult.Data.LastName, 
                request.RequestDate.ToString(), docResult.Data.TypeName);

            return new JsonResult(result);
        }

        public async Task<JsonResult> GetRequestDocumentEmployee(string NIK)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            using var response = await httpClient.GetAsync("Request/GetRequestForEmployee/?NIK="+NIK);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<RequestVM>>>(apiResponse);
            return new JsonResult(result);
        }

        public async Task<JsonResult> GetRequestForHR(string NIK)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            using var response = await httpClient.GetAsync("Request/GetRequestForHR/?NIK="+NIK);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<IEnumerable<RequestVM>>>(apiResponse);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<string> ApproveOrRejectByHR(ApproveOrRejectVM approveReject)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            StringContent content = new StringContent(JsonConvert.SerializeObject(approveReject), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("Request/ApproveOrRejectByHR", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<ApproveOrRejectVM>>(apiResponse);

            using var responsePersonHR = await httpClient.GetAsync("Person/" + approveReject.HR_NIK);
            string apiResponsePersonHR = await responsePersonHR.Content.ReadAsStringAsync();
            var resultPersonHR = JsonConvert.DeserializeObject<ResponseVM<Person>>(apiResponsePersonHR);

            using var responseRequest = await httpClient.GetAsync("Request/" + approveReject.RequestID);
            string apiResponseRequest = await responseRequest.Content.ReadAsStringAsync();
            var resultRequest = JsonConvert.DeserializeObject<ResponseVM<Request>>(apiResponseRequest);

            //email, isApproved, EmployeeName, Name, dateRespond, typeDoc
            await ApprovalNotificationEmployeeAsync(approveReject.Email, approveReject.Approve, resultRequest.Data.Person.FirstName+" "+ resultRequest.Data.Person.LastName,
                resultPersonHR.Data.FirstName+" "+resultPersonHR.Data.LastName, resultRequest.Data.RequestDate.ToString(), 
                resultRequest.Data.DocumentType.TypeName);

            return "200";
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
        public async Task<string> ApproveOrRejectByRM(ApproveOrRejectVM approveReject)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            StringContent content = new StringContent(JsonConvert.SerializeObject(approveReject), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("Request/ApproveOrRejectByRM", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<ApproveOrRejectVM>>(apiResponse);

            using var responsePerson = await httpClient.GetAsync("Person/" + approveReject.HR_NIK);
            string apiResponsePerson = await responsePerson.Content.ReadAsStringAsync();
            var resultPerson = JsonConvert.DeserializeObject<ResponseVM<Person>>(apiResponsePerson);

            using var responsePersonHR = await httpClient.GetAsync("Person/" + resultPerson.Data.Department.HR_NIK);
            string apiResponsePersonHR = await responsePersonHR.Content.ReadAsStringAsync();
            var resultPersonHR = JsonConvert.DeserializeObject<ResponseVM<Person>>(apiResponsePersonHR);
            var emailHR = resultPersonHR.Data.Email;

            using var responseRequest = await httpClient.GetAsync("Request/" + approveReject.RequestID);
            string apiResponseRequest = await responseRequest.Content.ReadAsStringAsync();
            var resultRequest = JsonConvert.DeserializeObject<ResponseVM<Request>>(apiResponseRequest);

            using var responsePersonEmp = await httpClient.GetAsync("Person/" + resultRequest.Data.PersonNIK);
            string apiResponsePersonEmp = await responsePersonHR.Content.ReadAsStringAsync();
            var resultPersonEmp = JsonConvert.DeserializeObject<ResponseVM<Person>>(apiResponsePersonHR);

            if (approveReject.Approve == 1)
            {
                //emailHR, EmployeeName, RMName, HRName, dateRequest, typeDoc
                await ApprovalNotificationHRAsync(emailHR, resultPersonEmp.Data.FirstName+" "+ resultPersonEmp.Data.LastName, 
                    resultPerson.Data.FirstName + " " + resultPerson.Data.LastName, resultPersonHR.Data.FirstName + " " + resultPersonHR.Data.LastName, 
                    resultRequest.Data.RequestDate.ToString(), resultRequest.Data.DocumentType.TypeName);

                return "200";
            }
            else if (approveReject.Approve == 0)
            {
                //email, isApproved, EmployeeName, Name, dateRespond, typeDoc
                await ApprovalNotificationEmployeeAsync(resultRequest.Data.Person.Email, approveReject.Approve,
                   resultRequest.Data.Person.FirstName + " " + resultRequest.Data.Person.LastName, resultPerson.Data.FirstName+" "+resultPerson.Data.LastName,
                   resultRequest.Data.RequestDate.ToString(), resultRequest.Data.DocumentType.TypeName);

                return "200";
            }

            return "500";
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