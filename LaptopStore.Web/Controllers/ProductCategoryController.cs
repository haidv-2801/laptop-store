using LaptopStore.Services.Services.ProductCategoryService;
using LaptopStore.Web.Models;
using LaptopStore.Data.ModelDTO.ProductCategory;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LaptopStore.Core;

namespace LaptopStore.Web.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly ILogger<ProductCategoryController> _logger;
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(ILogger<ProductCategoryController> logger, IProductCategoryService productCategoryService)
        {
            _logger = logger;
            _productCategoryService = productCategoryService;
        }

        public async Task<IActionResult> Index()
        {
            //var data = await _productCategoryService.GetAll();  
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return PartialView("_ProductCategoryCreatePartial");
        }

        public async Task<IActionResult> Update(string id)
        {
            var data = await _productCategoryService.GetById(id);
            return View(data);
        }

        public async Task<IActionResult> GetDetail(string id)
        {
            // Xử lý logic để lấy chi tiết bản ghi theo id
            var model = await _productCategoryService.GetById(id);// Lấy dữ liệu từ cơ sở dữ liệu hoặc từ các nguồn khác

            // Trả về PartialView chứa dữ liệu chi tiết
            return PartialView("_ProductCategoryDetailPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> GetProductCategoryPaging([FromBody] PagingRequest paging)
        {
            var response = new ServiceResponse();
            try
            {
                return Ok(response.OnSuccess(await _productCategoryService.GetProductCategoryPaging(paging)));
            }
            catch (Exception ex)
            {
                return BadRequest(response.OnError(ex));
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveProductCategory([FromBody]ProductCategorySaveDTO productCategorySaveDTO)
        {
            try
            {
                var data = await _productCategoryService.SaveProductCategory(productCategorySaveDTO);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }

        [HttpPut]
        public async Task<JsonResult> UpdateProductCategory([FromRoute] string id, [FromBody] ProductCategorySaveDTO productCategorySaveDTO)
        {
            try
            {
                var data = await _productCategoryService.UpdateProductCategory(id, productCategorySaveDTO);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteProductCategory([FromRoute] string id)
        {
            try
            {
                var data = await _productCategoryService.DeleteProductCategory(id);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }
        [HttpPost]
        public async Task<JsonResult> CheckDuplicateName([FromBody] string name)
        {
            try
            {
                var data = await _productCategoryService.CheckDuplicateName(name);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }
        [HttpGet]
        public async Task<JsonResult> CheckExistsProduct([FromRoute] string id)
        {
            try
            {
                var data = await _productCategoryService.CheckExistsProduct(id);
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
