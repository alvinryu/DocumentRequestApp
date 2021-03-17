using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Base.Controller;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountRoleController : BaseController<AccountRole, AccountRoleRepository, string>
    {
        private readonly AccountRoleRepository _accountRoleController;
        private readonly IConfiguration _configuration;
        public AccountRoleController(AccountRoleRepository accountRoleController, IConfiguration configuration) : base(accountRoleController)
        {
            _accountRoleController = accountRoleController;
            _configuration = configuration;
        }
    }
}