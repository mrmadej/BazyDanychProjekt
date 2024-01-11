using Microsoft.AspNetCore.Mvc;

namespace BazyDanychProjekt.Controllers
{
    public class AdminPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
