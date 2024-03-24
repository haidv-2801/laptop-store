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

        public PositionController(ILogger<PositionController> logger, IPositionService positionService)
        {
            _logger = logger;
            _positionService = positionService;
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
            var response = new ServiceResponse();
            try
            {
                return Ok(response.OnSuccess(await _positionService.GetPositionPaging(paging)));
            }
            catch (Exception ex)
            {
                return BadRequest(response.OnError(ex));
            }
        }

        [HttpPost]
        public async Task<JsonResult> SavePosition([FromBody]PositionSaveDTO positionSaveDTO)
        {
            try
            {
                var data = await _positionService.SavePosition(positionSaveDTO);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }

        [HttpPut]
        public async Task<JsonResult> UpdatePosition([FromRoute] string id, [FromBody] PositionSaveDTO positionSaveDTO)
        {
            try
            {
                var data = await _positionService.UpdatePosition(id, positionSaveDTO);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }

        [HttpDelete]
        public async Task<JsonResult> DeletePosition([FromRoute] string id)
        {
            try
            {
                var data = await _positionService.DeletePosition(id);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
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
