using LaptopStore.Data.Models;
using LaptopStore.Services.Services.AccountService;
using LaptopStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LaptopStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountService _accountService;

        public HomeController(ILogger<HomeController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _accountService.GetAll();  
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SaveAccount([FromBody] AccountSaveDTO accountSaveDTO)
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
