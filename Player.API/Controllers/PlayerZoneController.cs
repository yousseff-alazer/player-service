using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Player.BL.Services;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;

namespace Player.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerZoneController : Controller
    {
        private readonly playerContext _context;
        private readonly IOptions<AppSettingsRecord> appSettings;
        private readonly ILogger<PlayerZoneController> _logger;
        private readonly IBus _bus;

        public PlayerZoneController(playerContext context, IOptions<AppSettingsRecord> app, ILogger<PlayerZoneController> logger, IBus bus)
        {
            _context = context;
            appSettings = app;
            _logger = logger;
            _bus = bus;
        }

        [HttpPost]
        [Route("ListPlayerZones")]
        public ActionResult ListPlayerZones([FromBody] PlayerZoneRequest request)
        {
            request._context = _context;
            var PlayerZonesResponse = PlayerZoneService.ListPlayerZones(request);
            return Ok(new
            {
                PlayerZonesResponse
            });
        }

        [HttpPost]
        [Route("DeletePlayerZone")]
        public ActionResult DeletePlayerZone([FromBody] PlayerZoneRequest request)
        {
            request._context = _context;
            var PlayerZonesResponse = PlayerZoneService.DeletePlayerZone(request);

            return Ok(new
            {
                PlayerZonesResponse
            });
        }

        [HttpPost]
        [Route("AddPlayerZone")]
        public ActionResult AddPlayerZone([FromBody] PlayerZoneRequest request)
        {
            request._context = _context;
            var PlayerZonesResponse = PlayerZoneService.AddPlayerZone(request);

            return Ok(new
            {
                PlayerZonesResponse
            });
        }

        [HttpPost]
        [Route("EditPlayerZone")]
        public ActionResult EditPlayerZone([FromBody] PlayerZoneRequest request)
        {
            request._context = _context;
            var PlayerZonesResponse = PlayerZoneService.EditPlayerZone(request);
            
            return Ok(new
            {
                PlayerZonesResponse
            });
        }

        [HttpPost]
        [Route("ValidatePlayerZone")]
        public ActionResult ValidatePlayerZone([FromBody] PlayerZoneRequest request)
        {
            request._context = _context;
            var PlayerZonesResponse = PlayerZoneService.ValidatePlayerZone(request);

            return Ok(new
            {
                PlayerZonesResponse
            });
        }

    }
}
