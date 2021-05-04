using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ARMS.v2.Controllers
{
    [ApiController]
    [Route("Gc/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly ILogger<ContentController> _logger;
        private IContentService _service;

        public ContentController(ILogger<ContentController> logger, IContentService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<ContentModel> GetContents()
        {
            return _service.Select(null);
        }

        [HttpPost]
        public IActionResult Update([FromBody] ContentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{ContentID}/{UserID}")]
        public IActionResult Delete(int ContentID,string UserID)
        { 
            return Ok(_service.Delete(ContentID,UserID));
        }
    }
}
