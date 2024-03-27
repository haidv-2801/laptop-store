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

namespace LaptopStore.Web.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly ILogger<ReceiptController> _logger;
        private readonly IReceiptService _receiptService;
        private readonly ApplicationDbContext _dbContext;

        public ReceiptController(ILogger<ReceiptController> logger, IReceiptService receiptService, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _receiptService = receiptService;
            _dbContext = dbContext; 
        }

        public async Task<IActionResult> Index()
        {
            await Task.CompletedTask;
            return View();
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Customer = await _dbContext.Set<Customer>().AsNoTracking().ToListAsync();
            ViewBag.Account = await _dbContext.Set<Account>().AsNoTracking().ToListAsync();
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            ViewBag.Categories = await _dbContext.Set<ProductCategory>().AsNoTracking().ToListAsync();
            ViewBag.Positions = await _dbContext.Set<Position>().AsNoTracking().ToListAsync();
            var data = await _receiptService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetReceiptPaging([FromBody] PagingRequest paging)
        {
            var response = new ServiceResponse();
            try
            {
                return Ok(response.OnSuccess(await _receiptService.GetReceiptPaging(paging)));
            }
            catch (Exception ex)
            {
                return BadRequest(response.OnError(ex));
            }
        }

        [HttpPost]
        public async Task<ServiceResponse> SaveReceipt([FromBody] Receipt saveDTO)
        {
            var res = new ServiceResponse();
            try
            {
                return res.OnSuccess(await _receiptService.SaveReceipt(saveDTO));
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateReceipt([FromRoute] string id, [FromBody] Receipt saveDTO)
        {
            var res = new ServiceResponse();
            try
            {
                return res.OnSuccess(await _receiptService.UpdateReceipt(id, saveDTO));
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeleteReceipt([FromRoute] string id)
        {
            var res = new ServiceResponse();
            try
            {
                return res.OnSuccess(await _receiptService.DeleteReceipt(id));
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }
    }
}
