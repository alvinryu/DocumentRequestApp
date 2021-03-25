using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}