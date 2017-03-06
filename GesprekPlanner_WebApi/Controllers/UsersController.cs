using System.Linq;
using GesprekPlanner_WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GesprekPlanner_WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UsersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/values
        [HttpGet]
        public string Get()
        {

            var allUsers = _dbContext.Users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                u.GroupId
            });
            return JsonConvert.SerializeObject(allUsers);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            var allUsers = _dbContext.Users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                u.GroupId
            }).Where(u => u.Id == id);
            return JsonConvert.SerializeObject(allUsers);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
