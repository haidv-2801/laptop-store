using LaptopStore.Core;
using LaptopStore.Data.ModelDTO;
using LaptopStore.Services.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStore.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            return Redirect("/Auth/Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ServiceResponse> Login([FromBody] AccountLoginDTO accountLoginDTO)
        {
            var res = new ServiceResponse();
            try
            {
                return res = await _authService.SignIn(accountLoginDTO);
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }
    }
}
