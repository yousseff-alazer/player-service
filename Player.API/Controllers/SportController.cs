using Microsoft.AspNetCore.Mvc;
using Player.CommonDefinitions.Requests;
using Player.BL.Services;
using Player.DAL.mysqlplayerDB;

namespace Player.API.Controllers
{
    public class SportController : Controller
    {
        private readonly playerContext _context;

        public SportController(playerContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("ListSports")]
        public ActionResult ListSports([FromBody] SportRequest request)
        {
            request._context = _context;

            var challengesResponse = SportService.ListSports(request);
            return Ok(new
            {
                challengesResponse
            });
        }
        [HttpGet]
        [Route("Test123")]
        public ActionResult Test123()
        {

            return Ok("Done");
        }
    }
}
