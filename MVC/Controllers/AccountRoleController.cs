using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MVC.Base;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class AccountRoleController : BaseController<AccountRole, string>
    {
        [HttpPost]
        public async Task<JsonResult> UpdateAccountRole(AccountRole accountRole)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(accountRole), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("AccountRole/UpdateAccountRole", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseVM<AccountRole>>(apiResponse);
            return new JsonResult(result);
        }
    }
}