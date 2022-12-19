using FEFU_Quest.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FEFU_Quest.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IUser _user;

        public ProfileController(ILogger<ProfileController> logger, IUser user)
        {
            _logger = logger;
            _user = user;
        }

        public IActionResult Index()
        {
            var userName = User?.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                _logger.LogWarning("Опять эти куки пытаются не существующего пользователя куда-то отправить");
                return RedirectToAction("Log", "Account");
            }

            var curUser = _user.Get(userName);

            if(curUser == null)
            {
                return RedirectToAction("Log", "Account");
            }

            return View("Index", curUser);
        }
    }
}
