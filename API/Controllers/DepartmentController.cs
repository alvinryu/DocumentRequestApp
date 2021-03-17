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
    public class DepartmentController : BaseController<Department, DepartmentRepository, string>
    {
        private readonly DepartmentRepository _departmentRepository;
        private readonly IConfiguration _configuration;
        public DepartmentController(DepartmentRepository departmentRepository, IConfiguration configuration) : base(departmentRepository)
        {
            _departmentRepository = departmentRepository;
            _configuration = configuration;
        }
    }
}