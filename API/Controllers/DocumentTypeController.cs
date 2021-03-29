using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Base.Controller;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "HR")]
    public class DocumentTypeController : BaseController<DocumentType, DocumentTypeRepository, int>
    {
        private readonly DocumentTypeRepository _documentTypeRepository;
        private readonly IConfiguration _configuration;
        public DocumentTypeController(DocumentTypeRepository documentTypeRepository, IConfiguration configuration) : base(documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public override ActionResult<DocumentType> Get()
        {
            var result = _documentTypeRepository.Get();

            if (result != null)
            {
                return Ok(new { status = HttpStatusCode.OK, data = result, message = "Data Ditemukan" });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, data = result, message = "Terjadi Kesalahan" }); ;
            }
        }
    }
}