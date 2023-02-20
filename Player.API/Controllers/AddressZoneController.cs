using Microsoft.AspNetCore.Mvc;
using Player.BL.Services;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;

namespace Player.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressZoneController : Controller
    {
        private readonly playerContext _context;

        public AddressZoneController(playerContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("ListAddressZones")]
        public ActionResult ListAddressZones([FromBody] AddressZoneRequest request)
        {
            request._context = _context;
            var AddressZonesResponse = AddressZoneService.ListAddressZones(request);
            return Ok(new
            {
                AddressZonesResponse
            });
        }

        [HttpPost]
        [Route("DeleteAddressZone")]
        public ActionResult DeleteAddressZone([FromBody] AddressZoneRequest request)
        {
            request._context = _context;
            var AddressZonesResponse = AddressZoneService.DeleteAddressZone(request);
            return Ok(new
            {
                AddressZonesResponse
            });
        }

        [HttpPost]
        [Route("AddAddressZone")]
        public ActionResult AddAddressZone([FromBody] AddressZoneRequest request)
        {
            request._context = _context;
            var AddressZonesResponse = AddressZoneService.AddAddressZone(request);

            return Ok(new
            {
                AddressZonesResponse
            });
        }

        [HttpPost]
        [Route("EditAddressZone")]
        public ActionResult EditAddressZone([FromBody] AddressZoneRequest request)
        {
            request._context = _context;
            var AddressZonesResponse = AddressZoneService.EditAddressZone(request);
            return Ok(new
            {
                AddressZonesResponse
            });
        }
    }
}
