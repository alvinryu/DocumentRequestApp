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
    public class AccountRoleController : BaseController<AccountRole, AccountRoleRepository, string>
    {
        private readonly AccountRoleRepository _accountRoleRepository;
        private readonly IConfiguration _configuration;
        public AccountRoleController(AccountRoleRepository accountRoleRepository, IConfiguration configuration) : base(accountRoleRepository)
        {
            _accountRoleRepository = accountRoleRepository;
            _configuration = configuration;
        }

        [HttpPost("UpdateAccountRole")]
        public IActionResult UpdateAccountRole(AccountRole accountRole)
        {
            var result = _accountRoleRepository.UpdateAccountRole(accountRole);
            if (result != null)
            {
                return Ok(new { status = HttpStatusCode.OK, Message = "You have successfully Update AccountRole", data = "" });
            }
            else
            {
                return new OkObjectResult(new { Status = HttpStatusCode.Unauthorized, ErrorMessage = "Unauthorized Access" });
            }
        }
    }
}