using Microsoft.AspNetCore.Mvc;

namespace FEFU_Quest.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dormitory()
        {
            return View();
        }

        public IActionResult Fefu()
        {
            return View();
        }
    }
}
