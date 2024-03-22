using LaptopStore.Services.Services.AccountService;
using LaptopStore.Web.Models;
using LaptopStore.Data.ModelDTO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LaptopStore.Core;

namespace LaptopStore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _accountService.GetAll();  
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            var data = await _accountService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<ServiceResponse> GetAccountPaging([FromBody] PagingRequest paging)
        {
            var response = new ServiceResponse();
            try
            {
                return response.OnSuccess(await _accountService.GetAccountPaging(paging));
            }
            catch (Exception ex)
            {
                return response.OnError(ex);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveAccount([FromBody]AccountSaveDTO accountSaveDTO)
        {
            try
            {
                var data = await _accountService.SaveAccount(accountSaveDTO);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }

        [HttpPut]
        public async Task<JsonResult> UpdateAccount([FromRoute] string id, [FromBody] AccountSaveDTO accountSaveDTO)
        {
            try
            {
                var data = await _accountService.UpdateAccount(id, accountSaveDTO);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteAccount([FromRoute] string id)
        {
            try
            {
                var data = await _accountService.DeleteAccount(id);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
