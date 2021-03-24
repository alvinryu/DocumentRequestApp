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
    public class AccountRoleRepository : GeneralRepository<MyContext, AccountRole, string>
    {
        public IConfiguration _configuration;
        readonly DynamicParameters _parameters = new DynamicParameters();

        public AccountRoleRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            _configuration = configuration;
        }

        public AccountRole UpdateAccountRole(AccountRole accountRole)
        {
            var _accountRoleRepository = new GeneralDapperRepository<AccountRole>(_configuration);

            _parameters.Add("@NIK", accountRole.NIK);
            _parameters.Add("@RoleID", accountRole.RoleID);
            var result = _accountRoleRepository.Query("SP_UpdateAccountRole", _parameters);
            return result;
        }
    }
}
