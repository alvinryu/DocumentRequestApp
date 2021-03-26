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
    public class RoleController : BaseController<Role, RoleRepository, int>
    {
        private readonly RoleRepository _roleRepository;
        private readonly IConfiguration _configuration;
        public RoleController(RoleRepository roleRepository, IConfiguration configuration) : base(roleRepository)
        {
            _roleRepository = roleRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public override ActionResult<Role> Get()
        {
            var result = _roleRepository.Get();

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