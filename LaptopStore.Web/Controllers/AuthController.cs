﻿using LaptopStore.Core;
using LaptopStore.Data.ModelDTO;
using LaptopStore.Services.Services.AuthService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LaptopStore.Data.Models;
using Newtonsoft.Json;

namespace LaptopStore.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return Redirect("/Auth/Login");
        }
        
        public IActionResult Access()
        {
            return View();
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
                res = await _authService.SignIn(accountLoginDTO);
                return res;
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }
        [HttpGet]
        public async Task<ServiceResponse> Logout()
        {
            var res = new ServiceResponse();
            try
            {
                res = await _authService.SignOut();
                return res;
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }
    }
}
