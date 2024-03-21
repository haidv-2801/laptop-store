using LaptopStore.Services.Services.AccountService;
using LaptopStore.Web.Models;
using LaptopStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        [HttpPost]
        public async Task<IActionResult> SaveCreate(Account account)
        {
            await _accountService.Create(account);
            return Redirect("Index");
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _accountService.Delete(id);
            return RedirectToAction("Index");
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
