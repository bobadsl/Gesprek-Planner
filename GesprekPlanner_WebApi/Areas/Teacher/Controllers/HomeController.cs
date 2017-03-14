using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GesprekPlanner_WebApi.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Administrator,Teacher")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}