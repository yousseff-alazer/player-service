using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Player.BL.Services;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;
using System.Linq;
using System.Threading.Tasks;
using static Reservation.Portal.Controllers.Player_ProfileController;

namespace Player.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerEnergyPointController : Controller
    {
        private readonly playerContext _context;
        private readonly IBus _bus;

        public PlayerEnergyPointController(playerContext context, IBus bus)
        {
            _context = context;
            _bus = bus;
        }

        [HttpPost]
        [Route("ListEnergyPoints")]
        public ActionResult ListPlayerEnergyPoints([FromBody] PlayerEnergyPointRequest request)
        {
            request._context = _context;

            var PlayerEnergyPointsResponse = PlayerEnergyPointService.ListPlayerEnergyPoints(request);
            return Ok(new
            {
                PlayerEnergyPointsResponse
            });
        }

        [HttpPost]
        [Route("DeletePointFromPlayer")]
        public ActionResult DeletePlayerEnergyPoint([FromBody] PlayerEnergyPointRequest request)
        {
            request._context = _context;
            var model = request.PlayerEnergyPointRecord;
            
            var PlayerEnergyPointsResponse = PlayerEnergyPointService.DeletePlayerEnergyPoint(request);
            if (PlayerEnergyPointsResponse.Success)
            {
                model = PlayerEnergyPointsResponse.PlayerEnergyPointRecords.FirstOrDefault();
                PlayerInfoService.UpdatePlayerPoint(_context, model.PlayerId, (int)model.Points, true);

            }
            return Ok(new
            {
                PlayerEnergyPointsResponse
            });
        }

        [HttpPost]
        [Route("AddPointToPlayer")]
        public ActionResult AddPlayerEnergyPoint([FromBody] PlayerEnergyPointRequest request)
        {
            request._context = _context;
            var model = request.PlayerEnergyPointRecord;

            var PlayerEnergyPointsResponse = PlayerEnergyPointService.AddPlayerEnergyPoint(request);

            if (PlayerEnergyPointsResponse.Success)
                PlayerInfoService.UpdatePlayerPoint(_context, model.PlayerId, (int)model.Points, false);

            return Ok(new
            {
                PlayerEnergyPointsResponse
            });
        }

        [HttpPost]
        [Route("EditPointToPlayer")]
        public ActionResult EditPlayerEnergyPointAsync([FromBody] PlayerEnergyPointRequest request)
        {
            request._context = _context;
            var PlayerEnergyPointsResponse = PlayerEnergyPointService.EditPlayerEnergyPoint(request);
            return Ok(new
            {
                PlayerEnergyPointsResponse
            });
        }
    }
}
