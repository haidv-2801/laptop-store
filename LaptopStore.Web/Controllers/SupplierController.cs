using LaptopStore.Services.Services.SupplierService;
using LaptopStore.Web.Models;
using LaptopStore.Data.ModelDTO.Supplier;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LaptopStore.Core;

namespace LaptopStore.Web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly ISupplierService _supplierService;
        private readonly ServiceResponse _serviceResponse;

        public SupplierController(ILogger<SupplierController> logger, ISupplierService supplierService)
        {
            _logger = logger;
            _supplierService = supplierService;
            _serviceResponse = new ServiceResponse();
        }

        public async Task<IActionResult> Index()
        {
            //var data = await _supplierService.GetAll();  
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            var data = await _supplierService.GetById(id);
            return View(data);
        }

        public async Task<IActionResult> GetDetail(string id)
        {
            // Xử lý logic để lấy chi tiết bản ghi theo id
            var model = await _supplierService.GetById(id);// Lấy dữ liệu từ cơ sở dữ liệu hoặc từ các nguồn khác

            // Trả về PartialView chứa dữ liệu chi tiết
            return PartialView("_SupplierDetailPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> GetSupplierPaging([FromBody] PagingRequest paging)
        {
            try
            {
                return Ok(_serviceResponse.OnSuccess(await _supplierService.GetSupplierPaging(paging)));
            }
            catch (Exception ex)
            {
                return BadRequest(_serviceResponse.OnError(ex));
            }
        }

        [HttpPost]
        public async Task<ServiceResponse> SaveSupplier([FromBody]SupplierSaveDTO supplierSaveDTO)
        {
            try
            {
                var existsProductCategory = await _supplierService.CheckDuplicateName(supplierSaveDTO.Name);
                if (existsProductCategory)
                {
                    return _serviceResponse.ResponseData("Đã tồn tại NCC này", null);
                }
                var data = await _supplierService.SaveSupplier(supplierSaveDTO);
                return _serviceResponse.OnSuccess(data);
            }
            catch (Exception ex)
            {
                return _serviceResponse.OnError(ex);
            }
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateSupplier([FromRoute] string id, [FromBody] SupplierSaveDTO supplierSaveDTO)
        {
            try
            {
                var existsProductCategory = await _supplierService.CheckDuplicateNameNotThis(id, supplierSaveDTO.Name);
                if (existsProductCategory)
                {
                    return _serviceResponse.ResponseData("Đã tồn tại NCC này", null);
                }
                var data = await _supplierService.UpdateSupplier(id, supplierSaveDTO);
                return _serviceResponse.OnSuccess(data);
            }
            catch (Exception ex)
            {
                return _serviceResponse.OnError(ex);
            }
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeleteSupplier([FromRoute] string id)
        {
            try
            {
                var data = await _supplierService.DeleteSupplier(id);
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
