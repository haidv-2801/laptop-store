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

namespace LaptopStore.Web.Controllers
{
    public class WarehouseExportController : Controller
    {
        private readonly ILogger<WarehouseExportController> _logger;
        private readonly IWarehouseExportService _WarehouseExportService;
        private readonly ApplicationDbContext _dbContext;

        public WarehouseExportController(ILogger<WarehouseExportController> logger, IWarehouseExportService WarehouseExportService, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _WarehouseExportService = WarehouseExportService;
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
            //ViewBag.Positions = await _dbContext.Set<Position>().AsNoTracking().ToListAsync();
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            ViewBag.Categories = await _dbContext.Set<ProductCategory>().AsNoTracking().ToListAsync();
            ViewBag.Positions = await _dbContext.Set<Position>().AsNoTracking().ToListAsync();
            var data = await _WarehouseExportService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetWarehouseExportPaging([FromBody] PagingRequest paging)
        {
            var response = new ServiceResponse();
            try
            {
                return Ok(response.OnSuccess(await _WarehouseExportService.GetWarehouseExportPaging(paging)));
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
                return res.OnSuccess(await _WarehouseExportService.SaveWarehouseExport(saveDTO));
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateWarehouseExport([FromRoute] string id, [FromBody] WarehouseExport saveDTO)
        {
            var res = new ServiceResponse();
            try
            {
                return res.OnSuccess(await _WarehouseExportService.UpdateWarehouseExport(id, saveDTO));
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
                return res.OnSuccess(await _WarehouseExportService.DeleteWarehouseExport(id));
            }
            catch (Exception ex)
            {
                return res.OnError(ex);
            }
        }
    }
}
