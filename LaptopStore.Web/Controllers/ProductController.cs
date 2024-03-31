using LaptopStore.Services.Services.AccountService;
using LaptopStore.Web.Models;
using LaptopStore.Data.ModelDTO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LaptopStore.Core;
using LaptopStore.Services.Services.ProductService;
using Microsoft.EntityFrameworkCore;
using LaptopStore.Data.Models;
using LaptopStore.Data.Context;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Authorization;

namespace LaptopStore.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly ApplicationDbContext _dbContext;


        public ProductController(ILogger<ProductController> logger, IProductService accountService, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _productService = accountService;
            _dbContext = dbContext; 
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string id)
        {
            ViewBag.Categories = await _dbContext.Set<ProductCategory>().AsNoTracking().ToListAsync();
            ViewBag.Positions = await _dbContext.Set<Position>().AsNoTracking().ToListAsync();
            return View(await _productService.GetEntityByIDAsync(id));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _dbContext.Set<ProductCategory>().AsNoTracking().ToListAsync();
            ViewBag.Positions = await _dbContext.Set<Position>().AsNoTracking().ToListAsync();
            return View();
        }

        public async Task<IActionResult> ListAllProduct()
        {
            return Ok(await _dbContext.Set<Product>().AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Update(string id)
        {
            ViewBag.Categories = await _dbContext.Set<ProductCategory>().AsNoTracking().ToListAsync();
            ViewBag.Positions = await _dbContext.Set<Position>().AsNoTracking().ToListAsync();
            var data = await _productService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetProductPaging([FromBody] PagingRequest paging)
        {
            var response = new ServiceResponse();
            try
            {
                return Ok(response.OnSuccess(await _productService.GetProductPaging(paging)));
            }
            catch (Exception ex)
            {
                return BadRequest(response.OnError(ex));
            }
        }

        [HttpPost]
        public async Task<ServiceResponse> SaveProduct([FromBody] ProductSaveDTO saveDTO)
        {
            var res = new ServiceResponse();
            try
            {
                return res.OnSuccess(await _productService.SaveProduct(saveDTO));
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateProduct([FromRoute] string id, [FromBody] ProductSaveDTO saveDTO)
        {
            var res = new ServiceResponse();
            try
            {
                return res.OnSuccess(await _productService.UpdateProduct(id, saveDTO));
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeleteProduct([FromRoute] string id)
        {
            var res = new ServiceResponse();
            try
            {
                return res.OnSuccess(await _productService.DeleteProduct(id));
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }
    }
}
