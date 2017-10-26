using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TheCollection_Web.Controllers {

    [Authorize]
    public class HomeController : Controller {

        public IActionResult Index() {
            return View();
        }

        public IActionResult Error() {
            return View();
        }
    }
}
