using DeafX.Richter.Web.Filters;
using DeafX.Richter.Web.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Web.Controllers
{
    [Route("api/[controller]")]
    [ModelValidation]
    public class AccountController : Controller
    {

        private ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginViewModel model)
        {
            if(model.username == "Harp" && model.password == "Darp")
            {
                return Ok();
            }

            return BadRequest();
        }

    }
}
