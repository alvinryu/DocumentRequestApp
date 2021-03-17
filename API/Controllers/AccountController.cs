using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Base.Controller;
using API.Models;
using API.Repository.Data;
using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        public AccountController(AccountRepository accountRepository, IConfiguration configuration) : base(accountRepository)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginVM login)
        {
            var result = _accountRepository.Login(login);
            if (result != null)
            {
                var jwt = new JwtServices(_configuration);
                var token = jwt.GenerateSecurityToken(result);
                return Ok(new { status = HttpStatusCode.OK, Message = "You have successfully Sign In", Token = token });
            }
            else
            {
                return new OkObjectResult(new { Status = HttpStatusCode.Unauthorized, ErrorMessage = "Unauthorized Access" });
            }
        }
    }
}