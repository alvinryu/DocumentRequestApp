using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Base.Controller;
using API.Models;
using API.ViewModels;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using API.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestController : BaseController<Request, RequestRepository, int>
    {
        private readonly RequestRepository _requestRepository;
        private readonly IConfiguration _configuration;

        public RequestController(RequestRepository requestRepository, IConfiguration configuration) : base(requestRepository)
        {
            _requestRepository = requestRepository;
            _configuration = configuration;
        }

        [Authorize(Roles = "HR")]
        [HttpGet("GetRequestForHR")]
        public IActionResult GetRequestForHR()
        {
            var result = _requestRepository.GetRequestForHR();

            if (result != null)
            {
                return Ok(new { status = HttpStatusCode.OK, data = result, message = "Data Ditemukan" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan", data = "" });
            }
        }

        [HttpGet("GetRequestForEmployee")]
        public IActionResult GetRequestForEmployee(string NIK)
        {
            var result = _requestRepository.GetRequestForEmployee(NIK);

            if (result != null)
            {
                return Ok(new { status = HttpStatusCode.OK, data = result, message = "Data Ditemukan" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan", data = "" });
            }
        }

        [Authorize(Roles = "RM")]
        [HttpGet("GetRequestForRM")]
        public IActionResult GetRequestForRM(int DepartmentID)
        {
            var result = _requestRepository.GetRequestForRM(DepartmentID);

            if (result != null)
            {
                return Ok(new { status = HttpStatusCode.OK, data = result, message = "Data Ditemukan" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan", data = "" });
            }
        }

        [HttpPut("ApproveOrRejectByHR")]
        [AllowAnonymous]
        public ActionResult ApproveOrRejectByHR(ApproveOrRejectVM approveReject)
        {
            var result = _requestRepository.ApproveOrRejectByHR(approveReject);
            if (result != null)
            {
                return Ok(new { status = HttpStatusCode.OK, data = "", Message = "Berhasil update data" });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Update Data", data = "" });
            }
        }

        [HttpPut("ApproveOrRejectByRM")]
        [AllowAnonymous]
        public ActionResult ApproveOrRejectByRM(ApproveOrRejectVM approveReject)
        {
            var result = _requestRepository.ApproveOrRejectByRM(approveReject);
            if (result != null)
            {
                return Ok(new { status = HttpStatusCode.OK, data = "", Message = "Berhasil update data" });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Update Data", data = "" });
            }
        }
    }
}