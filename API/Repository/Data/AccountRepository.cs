using API.Context;
using API.Models;
using API.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        public IConfiguration _configuration;
        readonly DynamicParameters _parameters = new DynamicParameters();

        public AccountRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            _configuration = configuration;
        }

        public LoginVM Login(LoginVM loginVM)
        {
            var _userRepository = new GeneralDapperRepository<LoginVM>(_configuration);

            _parameters.Add("@Email", loginVM.Email);
            _parameters.Add("@Password", loginVM.Password);
            var result = _userRepository.Query("SP_RetrieveAccount", _parameters);
            return result;
        }
    }
}
