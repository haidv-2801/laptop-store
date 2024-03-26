using LaptopStore.Core;
using LaptopStore.Core.Utilities;
using LaptopStore.Data.ModelDTO;
using LaptopStore.Data.Models;
using LaptopStore.Services.Services.BaseService;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Data.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using AuthenticationProperties = Microsoft.AspNetCore.Authentication.AuthenticationProperties;
using Newtonsoft.Json;

namespace LaptopStore.Services.Services.AuthService
{
    public class AuthService : BaseService<Account>, IAuthService
    {
        public AuthService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public async Task<ServiceResponse> SignIn(AccountLoginDTO accountLoginDTO)
        {
            if(accountLoginDTO == null)
                throw new Exception($"Thông tin đăng nhập trống.");

            if (string.IsNullOrEmpty(accountLoginDTO.UserName))
                throw new Exception($"Tên tài khoản không được bỏ trống.");

            if (string.IsNullOrEmpty(accountLoginDTO.Password))
                throw new Exception($"Mật khẩu không được bỏ trống.");

            var res = new ServiceResponse();
            
            string hashedPassword = Hasher.MD5(accountLoginDTO.Password);
            var account = await dbSet.FirstOrDefaultAsync(f => f.Username == accountLoginDTO.UserName && f.Password == hashedPassword);
            if(account == null)
                throw new Exception($"Tài khoản hoặc mật khẩu không đúng.");
            await AddClaims(account);
            _httpContextAccessor.HttpContext.Session.SetString("UserLogin", JsonConvert.SerializeObject(account));
            return res.OnSuccess(account);
        }
        public async Task<ServiceResponse> SignOut()
        {
            var res = new ServiceResponse();
            await _httpContextAccessor.HttpContext.SignOutAsync(
            scheme: CookieAuthenticationDefaults.AuthenticationScheme);
            return res.OnSuccess(true);
        }

        private async Task AddClaims(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.AccountType.ToString()),
            };
            //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
            var principal = new ClaimsPrincipal(identity);
            //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = true
            });
            //return LocalRedirect(objLoginModel.ReturnUrl);
            //await HttpContext.SignInAsync(
            //scheme: "DemoSecurityScheme",
            //principal: principal,
            //properties: new AuthenticationProperties
            //{
            //    //IsPersistent = true, // for 'remember me' feature
            //    //ExpiresUtc = DateTime.UtcNow.AddMinutes(1)
            //});

        }
    }
}
