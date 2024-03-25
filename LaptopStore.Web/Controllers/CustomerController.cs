using LaptopStore.Services.Services.CustomerService;
using LaptopStore.Web.Models;
using LaptopStore.Data.ModelDTO.Customer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LaptopStore.Core;

namespace LaptopStore.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        private readonly ServiceResponse _serviceResponse;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
            _serviceResponse = new ServiceResponse();
        }

        public async Task<IActionResult> Index()
        {
            //var data = await _customerService.GetAll();  
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            var data = await _customerService.GetById(id);
            return View(data);
        }

        public async Task<IActionResult> GetDetail(string id)
        {
            // Xử lý logic để lấy chi tiết bản ghi theo id
            var model = await _customerService.GetById(id);// Lấy dữ liệu từ cơ sở dữ liệu hoặc từ các nguồn khác

            // Trả về PartialView chứa dữ liệu chi tiết
            return PartialView("_CustomerDetailPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> GetCustomerPaging([FromBody] PagingRequest paging)
        {
            try
            {
                return Ok(_serviceResponse.OnSuccess(await _customerService.GetCustomerPaging(paging)));
            }
            catch (Exception ex)
            {
                return BadRequest(_serviceResponse.OnError(ex));
            }
        }

        [HttpPost]
        public async Task<ServiceResponse> SaveCustomer([FromBody]CustomerSaveDTO customerSaveDTO)
        {
            try
            {
                var data = await _customerService.SaveCustomer(customerSaveDTO);
                return _serviceResponse.OnSuccess(data);
            }
            catch (Exception ex)
            {
                return _serviceResponse.OnError(ex);
            }
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateCustomer([FromRoute] string id, [FromBody] CustomerSaveDTO customerSaveDTO)
        {
            try
            {
                var data = await _customerService.UpdateCustomer(id, customerSaveDTO);
                return _serviceResponse.OnSuccess(data);
            }
            catch (Exception ex)
            {
                return _serviceResponse.OnError(ex);
            }
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeleteCustomer([FromRoute] string id)
        {
            try
            {
                var data = await _customerService.DeleteCustomer(id);
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
