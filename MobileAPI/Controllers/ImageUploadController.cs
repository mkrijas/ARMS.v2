using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly string _mainProjectWwwRoot;

        public ImageUploadController(IConfiguration configuration)
        {
            _mainProjectWwwRoot = configuration["MainProjectSettings:WwwRootPath"];
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Generate a unique filename
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Construct the full path
            var path = Path.Combine(_mainProjectWwwRoot, "ArmsStaticFiles", "Gc", "Acknowledgement", uniqueFileName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            // Save the file
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { FileName = uniqueFileName, FilePath = path });
        }
    }
}
