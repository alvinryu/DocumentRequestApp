﻿using System;
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
    [Authorize]
    public class PersonController : BaseController<Person, PersonRepository, string>
    {
        private readonly PersonRepository _personRepository;
        private readonly IConfiguration _configuration;

        public PersonController(PersonRepository personRepository, IConfiguration configuration) : base(personRepository)
        {
            _personRepository = personRepository;
            _configuration = configuration;
        }

        [HttpPost("CheckEmail")]
        [AllowAnonymous]
        public IActionResult CheckEmail(string Email)
        {
            var result = _personRepository.CheckEmail(Email);

            if (result != null)
            {
                return Ok(new { status = HttpStatusCode.OK, data = result, message = "Data Ditemukan" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan", data = "" });
            }
        }

        [HttpPost("CheckKTP")]
        [Authorize]
        public IActionResult CheckKTP(string KTP)
        {
            var result = _personRepository.CheckKTP(KTP);

            if (result != null)
            {
                return Ok(new { status = HttpStatusCode.OK, data = result, message = "Data Ditemukan" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan", data = "" });
            }
        }
    }
}