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
        private readonly ServiceResponse _serviceResponse;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
            _serviceResponse = new ServiceResponse();
        }

        public async Task<IActionResult> Index()
        {
            //var data = await _accountService.GetAll();  
            return View();
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

        public async Task<IActionResult> GetDetail(string id)
        {
            // Xử lý logic để lấy chi tiết bản ghi theo id
            var model = await _accountService.GetById(id);// Lấy dữ liệu từ cơ sở dữ liệu hoặc từ các nguồn khác

            // Trả về PartialView chứa dữ liệu chi tiết
            return PartialView("_AccountDetailPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> GetAccountPaging([FromBody] PagingRequest paging)
        {
            var response = new ServiceResponse();
            try
            {
                return Ok(response.OnSuccess(await _accountService.GetAccountPaging(paging)));
            }
            catch (Exception ex)
            {
                return BadRequest(response.OnError(ex));
            }
        }

        [HttpPost]
        public async Task<ServiceResponse> SaveAccount([FromBody]AccountSaveDTO accountSaveDTO)
        {
            try
            {
                    var data = await _accountService.SaveAccount(accountSaveDTO);
                    return _serviceResponse.OnSuccess(data);
            }
            catch (Exception ex)
            {
                return _serviceResponse.OnError(ex);
            }
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateAccount([FromRoute] string id, [FromBody] AccountSaveDTO accountSaveDTO)
        {
            try
            {
                var data = await _accountService.UpdateAccount(id, accountSaveDTO);
                return _serviceResponse.OnSuccess(data);
            }
            catch (Exception ex)
            {
                return _serviceResponse.OnError(ex);
            }
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeleteAccount([FromRoute] string id)
        {
            try
            {
                var data = await _accountService.DeleteAccount(id);
                return _serviceResponse.OnSuccess(data);
            }
            catch (Exception ex)
            {
                return _serviceResponse.OnError(ex);
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
