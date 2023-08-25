using Microsoft.AspNetCore.Mvc;

namespace RWA_MVC_project.Controllers
{
    public class UnauthorizedController : Controller
    {
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
