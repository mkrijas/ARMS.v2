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
    [Route("Entity/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly ILogger<BankAccountController> _logger;
        private IBankAccountService _service;

        public BankAccountController(ILogger<BankAccountController> logger, IBankAccountService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{BankAccountID}")]
        public IEnumerable<BankAccountModel> GetBankAccounts(int BankAccountID = 0)
        {
            return _service.Select(BankAccountID);
        }

        [HttpPost]
        public IActionResult Update([FromBody] BankAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{BankAccountID}/{UserID}")]
        public IActionResult Delete(int BankAccountID,string UserID)
        { 
            return Ok(_service.Delete(BankAccountID,UserID));
        }
    }
}
