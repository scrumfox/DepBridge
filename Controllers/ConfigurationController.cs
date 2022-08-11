using depBridger.Models;
using depBridger.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace depBridger.Controllers
{
    public class ConfigurationController : Controller
    {
        IConfigurationService configurationService;
        
        public ConfigurationController(IConfigurationService configService)
        {
            configurationService = configService;
        }

        public Task<IActionResult> Index()
        {
            return Save();
        }

        public async Task<IActionResult> Save()
        {
            var configuration = Get().Result;

            return View(new ConfigurationViewModel { ApiEndPoint = configuration.ApiEndPoint, ApiKey = configuration.ApiKey, ProjectUid = configuration.ProjectUid });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ConfigurationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                configurationService.Save(new Configuration { ApiEndPoint = model.ApiEndPoint, ApiKey = model.ApiKey, ProjectUid = model.ProjectUid });
               
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View();
        }

        private async Task<ConfigurationViewModel> Get()
        {
            var configuration = configurationService.ReadConfiguration();

            return new ConfigurationViewModel { ApiEndPoint = configuration.ApiEndPoint, ApiKey = configuration.ApiKey, ProjectUid = configuration.ProjectUid };
        }
    }
}
