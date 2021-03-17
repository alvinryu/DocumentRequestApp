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
    public class RoleController : BaseController<Role, RoleRepository, int>
    {
        private readonly RoleRepository _roleRepository;
        private readonly IConfiguration _configuration;
        public RoleController(RoleRepository roleRepository, IConfiguration configuration) : base(roleRepository)
        {
            _roleRepository = roleRepository;
            _configuration = configuration;
        }
    }
}