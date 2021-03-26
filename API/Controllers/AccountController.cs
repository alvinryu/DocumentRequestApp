using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using API.Base.Controller;
using API.Models;
using API.Repository.Data;
using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
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

        [HttpPut]
        [AllowAnonymous]
        public override ActionResult Put(Account account)
        {
            if (account == null)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Data Yang Di-Input Salah / Tidak Lengkap", data = "" });
            }

            var result = _accountRepository.Update(account);

            if (result > 0)
            {
                return Ok(new { status = HttpStatusCode.OK, data = "", message = "Berhasil Update Data" });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Update Data", data = "" });
            }
        }
    }
}