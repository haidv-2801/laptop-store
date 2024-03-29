using LaptopStore.Services.Services.AccountService;
using LaptopStore.Web.Models;
using LaptopStore.Data.ModelDTO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LaptopStore.Core;
using LaptopStore.Services.Services.WarehouseExportService;
using Microsoft.EntityFrameworkCore;
using LaptopStore.Data.Models;
using LaptopStore.Data.Context;
using System.Net.NetworkInformation;
using LaptopStore.Data.ModelDTO.WarehouseExport;
using LaptopStore.Data.ModelDTO.Receipt;
using Microsoft.AspNetCore.Authorization;

namespace LaptopStore.Web.Controllers
{
    [Authorize]
    public class WarehouseExportController : Controller
    {
        private readonly ILogger<WarehouseExportController> _logger;
        private readonly IWarehouseExportService _warehouseExportService;
        private readonly ApplicationDbContext _dbContext;

        public WarehouseExportController(ILogger<WarehouseExportController> logger, IWarehouseExportService WarehouseExportService, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _warehouseExportService = WarehouseExportService;
            _dbContext = dbContext; 
        }

        public async Task<IActionResult> Index()
        {
            await Task.CompletedTask;
            return View();
        }

        public async Task<IActionResult> Detail(string id)
        {
            var model = await _warehouseExportService.GetById(id);// Lấy dữ liệu từ cơ sở dữ liệu hoặc từ các nguồn khác
            var warehouseExportDetails = from rcd in _dbContext.Set<WarehouseExportDetail>()
                                 join prod in _dbContext.Set<Product>() on rcd.ProductId equals prod.Id
                                 where rcd.WarehouseExportId == model.Id
                                 select new WarehouseExportProductViewDTO { Id = prod.Id, Name = prod.Name, Image = prod.Image, Quantity = rcd.Quantity, UnitPrice = rcd.UnitPrice };

            ViewBag.WarehouseExportDetails = warehouseExportDetails.ToList();

            return PartialView("_WarehouseExportDetailPartial", model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Customer = await _dbContext.Set<Customer>().AsNoTracking().ToListAsync();
            ViewBag.Account = await _dbContext.Set<Account>().AsNoTracking().ToListAsync();
            //ViewBag.Positions = await _dbContext.Set<Position>().AsNoTracking().ToListAsync();
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            ViewBag.Customer = await _dbContext.Set<Customer>().AsNoTracking().ToListAsync();
            ViewBag.Account = await _dbContext.Set<Account>().AsNoTracking().ToListAsync();
            var data = await _warehouseExportService.GetByIDIncludesDetail(id);
            /*var warehouseExportDetails = from rcd in _dbContext.Set<WarehouseExportDetail>()
                                         join prod in _dbContext.Set<Product>() on rcd.ProductId equals prod.Id
                                         where rcd.WarehouseExportId == data.Id
                                         select new WarehouseExportProductViewDTO { Id = prod.Id, Name = prod.Name, Image = prod.Image, Quantity = rcd.Quantity, UnitPrice = rcd.UnitPrice };*/
            return View(data);
        }

        public ServiceResponse GetProductsExport(string id)
        {
            var response = new ServiceResponse();
            var warehouseExportDetails = from rcd in _dbContext.Set<WarehouseExportDetail>()
                                 join prod in _dbContext.Set<Product>() on rcd.ProductId equals prod.Id
                                 where rcd.WarehouseExportId == id
                                 select new WarehouseExportProductViewDTO { Id = prod.Id, Name = prod.Name, Image = prod.Image ?? string.Empty, Quantity = rcd.Quantity, UnitPrice = rcd.UnitPrice };

            response.Data = new
            {
                WarehouseExportDetails = warehouseExportDetails.ToList(),
                TotalPrice = (warehouseExportDetails.ToList().Sum(x => x.Total)).ToString()
            };

            return response;
        }

        [HttpPost]
        public async Task<IActionResult> GetWarehouseExportPaging([FromBody] PagingRequest paging)
        {
            var response = new ServiceResponse();
            try
            {
                return Ok(response.OnSuccess(await _warehouseExportService.GetWarehouseExportPaging(paging)));
            }
            catch (Exception ex)
            {
                return BadRequest(response.OnError(ex));
            }
        }

        [HttpPost]
        public async Task<ServiceResponse> SaveWarehouseExport([FromBody] WarehouseExportSaveDTO saveDTO)
        {
            var res = new ServiceResponse();
            try
            {
                return res.OnSuccess(await _warehouseExportService.SaveWarehouseExport(saveDTO));
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateWarehouseExport([FromRoute] string id, [FromBody] WarehouseExportSaveDTO saveDTO)
        {
            var res = new ServiceResponse();
            try
            {
                return res.OnSuccess(await _warehouseExportService.UpdateWarehouseExport(id, saveDTO));
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeleteWarehouseExport([FromRoute] string id)
        {
            var res = new ServiceResponse();
            try
            {
                return res.OnSuccess(await _warehouseExportService.DeleteWarehouseExport(id));
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }
    }
}
