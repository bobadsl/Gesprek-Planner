using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GesprekPlanner_WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            var httpClient = new HttpClient();
            var jsonUsers = httpClient.GetStringAsync("http://localhost:5000/Api/Users").Result;
            var users = JsonConvert.DeserializeObject(jsonUsers, typeof(List<MinimalUser>));
            return View(users);
        }
    }
}
