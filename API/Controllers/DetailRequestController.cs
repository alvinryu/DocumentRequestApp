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
    public class DetailRequestController : BaseController<DetailRequest, DetailRequestRepository, int>
    {
        private readonly DetailRequestRepository _detailRequestRepository;
        private readonly IConfiguration _configuration;
        public DetailRequestController(DetailRequestRepository detailRequestRepository, IConfiguration configuration) : base(detailRequestRepository)
        {
            _detailRequestRepository = detailRequestRepository;
            _configuration = configuration;
        }
    }
}