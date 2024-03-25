using LaptopStore.Services.Services.PositionService;
using LaptopStore.Web.Models;
using LaptopStore.Data.ModelDTO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LaptopStore.Core;

namespace LaptopStore.Web.Controllers
{
    public class PositionController : Controller
    {
        private readonly ILogger<PositionController> _logger;
        private readonly IPositionService _positionService;
        private readonly ServiceResponse _serviceResponse;

        public PositionController(ILogger<PositionController> logger, IPositionService positionService)
        {
            _logger = logger;
            _positionService = positionService;
            _serviceResponse = new ServiceResponse();
        }

        public async Task<IActionResult> Index()
        {
            //var data = await _positionService.GetAll();  
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            var data = await _positionService.GetById(id);
            return View(data);
        }

        public async Task<IActionResult> GetDetail(string id)
        {
            // Xử lý logic để lấy chi tiết bản ghi theo id
            var model = await _positionService.GetById(id);// Lấy dữ liệu từ cơ sở dữ liệu hoặc từ các nguồn khác

            // Trả về PartialView chứa dữ liệu chi tiết
            return PartialView("_PositionDetailPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> GetPositionPaging([FromBody] PagingRequest paging)
        {
            try
            {
                return Ok(_serviceResponse.OnSuccess(await _positionService.GetPositionPaging(paging)));
            }
            catch (Exception ex)
            {
                return BadRequest(_serviceResponse.OnError(ex));
            }
        }

        [HttpPost]
        public async Task<ServiceResponse> SavePosition([FromBody]PositionSaveDTO positionSaveDTO)
        {
            try
            {
                var existsProductCategory = await _positionService.CheckDuplicateName(positionSaveDTO.Name);
                if (existsProductCategory)
                {
                    return _serviceResponse.ResponseData("Đã tồn tại vị trí này", null);
                }
                var data = await _positionService.SavePosition(positionSaveDTO);
                return _serviceResponse.OnSuccess(data);
            }
            catch (Exception ex)
            {
                return _serviceResponse.OnError(ex);
            }
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdatePosition([FromRoute] string id, [FromBody] PositionSaveDTO positionSaveDTO)
        {
            try
            {
                var existsProductCategory = await _positionService.CheckDuplicateName(positionSaveDTO.Name);
                if (existsProductCategory)
                {
                    return _serviceResponse.ResponseData("Đã tồn tại vị trí này", null);
                }
                var data = await _positionService.UpdatePosition(id, positionSaveDTO);
                return _serviceResponse.OnSuccess(data);
            }
            catch (Exception ex)
            {
                return _serviceResponse.OnError(ex);
            }
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeletePosition([FromRoute] string id)
        {
            try
            {
                var existsProduct = await _positionService.CheckExistsProduct(id);
                if (existsProduct)
                {
                    return _serviceResponse.ResponseData("Tồn tại sản phẩm. Không thể xóa", null);
                }
                var data = await _positionService.DeletePosition(id);
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
