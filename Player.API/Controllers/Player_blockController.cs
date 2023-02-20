using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Player.DAL.mysqlplayerDB;
using Reservation.BL.Services;
using Reservation.CommonDefinitions.MysqlRequests;

namespace Reservation.Portal.Controllers
{
    [Route("api/PlayerBlock")]
    public class Player_blockController : Controller
    {
        private readonly playerContext _context;
        private readonly IConfiguration configuration;

        public Player_blockController(playerContext context,
        IConfiguration _configuration)
        {
            _context = context;
            configuration = _configuration;

        }


        [HttpPost]
        [Route("ListBlocked")]
        public ActionResult List([FromBody] player_blockRequest request)
        {
            request._context = _context;
            var player_blockResponse = Player_blockService.Listplayer_block(request);
            return Ok(new
            {
                player_blockResponse
            });

        }


        [Route("AddOrEditBlock")]
        [HttpPost]
        public ActionResult AddOrEditBlock([FromBody] player_blockRequest request)
        {
            request._context = _context;
            var player_blockResponse = Player_blockService.Addplayer_block(request);
            return Ok(new
            {
                player_blockResponse
            });
        }
    }
}