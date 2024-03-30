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
using LaptopStore.Data.ModelDTO.Receipt;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Authorization;

namespace LaptopStore.Web.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Detail(string id)
        {
            var model = await _receiptService.GetById(id);// Lấy dữ liệu từ cơ sở dữ liệu hoặc từ các nguồn khác
            var receiptDetails = from rcd in _dbContext.Set<ReceiptDetail>()
                                 join prod in _dbContext.Set<Product>() on rcd.ProductId equals prod.Id
                                 where rcd.ReceiptId == model.Id
                                 select new ReceiptProductViewDTO { Id = prod.Id, Name = prod.Name, Image = prod.Image ?? string.Empty, Quantity = rcd.Quantity, UnitPrice = rcd.UnitPrice };

            ViewBag.ReceiptDetails = receiptDetails.ToList();
            ViewBag.TotalPrice = (receiptDetails.ToList().Sum(x => x.Total)).ToString();

            return PartialView("_ReceiptDetailPartial", model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _receiptService.GetById(id);// Lấy dữ liệu từ cơ sở dữ liệu hoặc từ các nguồn khác

            var receiptDetails = from rcd in _dbContext.Set<ReceiptDetail>()
                                 join prod in _dbContext.Set<Product>() on rcd.ProductId equals prod.Id
                                 where rcd.ReceiptId == model.Id
                                 select new ReceiptProductViewDTO { Id = prod.Id, Name = prod.Name, Image = prod.Image ?? string.Empty, Quantity = rcd.Quantity, UnitPrice = rcd.UnitPrice };

            ViewBag.ReceiptDetails = receiptDetails.ToList();
            ViewBag.TotalPrice = (receiptDetails.ToList().Sum(x => x.Total)).ToString();
            ViewBag.Supplier = _dbContext.Set<Supplier>().AsNoTracking().ToList();

            return View(model);
        }

        public ServiceResponse GetProductsReceipt(string id)
        {
            var response = new ServiceResponse();
            var receiptDetails = from rcd in _dbContext.Set<ReceiptDetail>()
                                 join prod in _dbContext.Set<Product>() on rcd.ProductId equals prod.Id
                                 where rcd.ReceiptId == id
                                 select new ReceiptProductViewDTO { Id = prod.Id, Name = prod.Name, Image = prod.Image ?? string.Empty, Quantity = rcd.Quantity, UnitPrice = rcd.UnitPrice };

            response.Data = new
            {
                ReceiptDetails = receiptDetails.ToList(),
                TotalPrice = (receiptDetails.ToList().Sum(x => x.Total)).ToString()
            };

            return response;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Customer = await _dbContext.Set<Customer>().AsNoTracking().ToListAsync();
            ViewBag.Account = await _dbContext.Set<Account>().AsNoTracking().ToListAsync();
            ViewBag.Supplier = await _dbContext.Set<Supplier>().AsNoTracking().ToListAsync();
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
        public async Task<ServiceResponse> SaveReceipt([FromBody] ReceiptSaveDTO saveDTO)
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
        public async Task<ServiceResponse> UpdateReceipt([FromRoute] string id, [FromBody] ReceiptSaveDTO saveDTO)
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
