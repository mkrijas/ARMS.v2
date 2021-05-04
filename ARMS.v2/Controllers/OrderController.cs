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
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private IOrderService _service;

        public OrderController(ILogger<OrderController> logger, IOrderService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{OrderID}")]
        public IEnumerable<OrderModel> GetOrders(int OrderID)
        {
            return _service.Select(OrderID);
        }

        [HttpPost]
        public IActionResult Update([FromBody] OrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{OrderID}/{UserID}")]
        public IActionResult Delete(int OrderID,string UserID)
        { 
            return Ok(_service.Delete(OrderID,UserID));
        }
    }
}
