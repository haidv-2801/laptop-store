using LaptopStore.Core;
using LaptopStore.Core.Utilities;
using LaptopStore.Data.ModelDTO;
using LaptopStore.Data.Models;
using LaptopStore.Services.Services.BaseService;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Data.Context;

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
            
            return res.OnSuccess(account);
        }

        private async Task AddClaims(AccountLoginDTO accountLoginDTO)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, accountLoginDTO.UserName),
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "cookie"));

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
