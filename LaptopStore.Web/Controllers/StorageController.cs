using LaptopStore.Core;
using LaptopStore.Services.Services.StorageService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.WebSockets;

namespace LaptopStore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;
        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost("Image")]
        public async Task<IActionResult> SaveImageAsync([FromForm] IFormFile file)
        {
            var response = new ServiceResponse();
            try
            {
                return Ok(response.OnSuccess(await _storageService.SaveImageAsync(file)));
            }
            catch (Exception ex)
            {
                return BadRequest(response.OnError(ex));
            }
        }

        [HttpDelete("Image/{name}")]
        public async Task<IActionResult> DeleteImageAsync([FromRoute] string name)
        {
            var response = new ServiceResponse();
            try
            {
                return Ok(response.OnSuccess(await _storageService.DeleteImageAsync(name)));
            }
            catch (Exception ex)
            {
                return BadRequest(response.OnError(ex));
            }
        }

        [HttpGet("Image/{name}")]
        public async Task<ActionResult> GetImageAsync([FromRoute] string name)
        {
            try
            {
                var image = await _storageService.GetImageAsync(name);
                return new FileStreamResult(image, "image/jpeg");  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
