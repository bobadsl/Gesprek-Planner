using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GesprekPlanner_WebApi.Areas.Schooladmin.Controllers
{
    [Authorize(Roles = "Eigenaar, Schooladmin")]
    [Area("Schooladmin")]
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
