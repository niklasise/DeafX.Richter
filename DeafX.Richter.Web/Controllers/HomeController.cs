using DeafX.Richter.Web.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DeafX.Richter.Web.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> _logger;
        private VersionService _versionService;

        public HomeController(ILogger<HomeController> logger, VersionService versionService)
        {
            _logger = logger;
            _versionService = versionService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_versionService.Version as object);
        }

        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                _logger.LogError($"Unhandled exception from path '{exceptionFeature.Path}'", exceptionFeature.Error);
            }

            return View();
        }
    }
}
