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

namespace LaptopStore.Web.ApiController
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPut("Update")]
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
    }
}
