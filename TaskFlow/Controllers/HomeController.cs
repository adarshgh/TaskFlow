using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Task");
            }
            return RedirectToAction("Register", "Account");
        }
    }
}
