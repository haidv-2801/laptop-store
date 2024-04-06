using LaptopStore.Services.Services.AccountService;
using LaptopStore.Web.Models;
using LaptopStore.Data.ModelDTO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LaptopStore.Core;
using Microsoft.AspNetCore.Authorization;

namespace LaptopStore.Web.Controllers
{
    public class DashboardController : Controller
    {

        public DashboardController()
        {
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
