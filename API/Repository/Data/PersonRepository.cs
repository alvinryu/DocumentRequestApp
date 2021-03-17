using API.Context;
using API.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class PersonRepository : GeneralRepository<MyContext, Person, string>
    {
        public IConfiguration _configuration;
        readonly DynamicParameters _parameters = new DynamicParameters();

        public PersonRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            _configuration = configuration;
        }

        public Person CheckEmail(string Email)
        {
            var _personRepository = new GeneralDapperRepository<Person>(_configuration);

            _parameters.Add("@Email", Email);
            var result = _personRepository.Query("SP_GetPersonByEmail", _parameters);
            return result;
        }
    }
}
