using API.Context;
using API.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class DashboardRepository
    {
        public IConfiguration _configuration;

        public DashboardRepository(MyContext myContext, IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<ChartVM> Chart()
        {
            var _chartRepository = new GeneralDapperRepository<ChartVM>(_configuration);

            var result = _chartRepository.Get("SP_RetrieveType");
            return result;
        }
    }
}
