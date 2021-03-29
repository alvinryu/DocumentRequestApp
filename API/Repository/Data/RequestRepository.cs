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
    public class RequestRepository : GeneralRepository<MyContext, Request, int>
    {
        public IConfiguration _configuration;
        readonly DynamicParameters _parameters = new DynamicParameters();

        public RequestRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            _configuration = configuration;
        }

        public IEnumerable<RequestVM> GetRequestForHR()
        {
            var _requestRepository = new GeneralDapperRepository<RequestVM>(_configuration);

            var result = _requestRepository.Get("SP_GetRequestForHR");
            return result;
        }

        public IEnumerable<RequestVM> GetRequestForEmployee(string NIK)
        {
            var _requestRepository = new GeneralDapperRepository<RequestVM>(_configuration);
            _parameters.Add("@NIK", NIK);

            var result = _requestRepository.Get("SP_GetRequestForEmployee", _parameters);
            return result;
        }

        public IEnumerable<RequestVM> GetRequestForRM(int DepartmentID)
        {
            var _requestRepository = new GeneralDapperRepository<RequestVM>(_configuration);
            _parameters.Add("@DepartmentID", DepartmentID);

            var result = _requestRepository.Get("SP_GetRequestForRM", _parameters);
            return result;
        }

        public ApproveOrRejectVM ApproveOrRejectByHR (ApproveOrRejectVM approveReject)
        {
            var _requestRepository = new GeneralDapperRepository<ApproveOrRejectVM>(_configuration);

            _parameters.Add("@Approve", approveReject.Approve);
            _parameters.Add("@RequestID", approveReject.RequestID);
            _parameters.Add("@HR_NIK", approveReject.HR_NIK);
            _parameters.Add("@ApproveHRDate", approveReject.ApproveHRDate);
            var result = _requestRepository.Query("SP_ApproveOrRejectByHR", _parameters);
            return result;
        }

        public ApproveOrRejectVM ApproveOrRejectByRM(ApproveOrRejectVM approveReject)
        {
            var _requestRepository = new GeneralDapperRepository<ApproveOrRejectVM>(_configuration);

            _parameters.Add("@Approve", approveReject.Approve);
            _parameters.Add("@RequestID", approveReject.RequestID);
            _parameters.Add("@ApproveRMDate", approveReject.ApproveRMDate);
            var result = _requestRepository.Query("SP_ApproveOrRejectByRM", _parameters);
            return result;
        }
    }
}
