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

        public IEnumerable<ChartVM> ChartRole()
        {
            var _chartRepository = new GeneralDapperRepository<ChartVM>(_configuration);

            var result = _chartRepository.Get("SP_RetrieveRole");
            return result;
        }      
        
        public IEnumerable<ChartVM> ChartDocType()
        {
            var _chartRepository = new GeneralDapperRepository<ChartVM>(_configuration);

            var result = _chartRepository.Get("SP_RetrieveDocType");
            return result;
        }
    }
}
