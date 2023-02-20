using Microsoft.AspNetCore.Mvc;
using Player.BL.Services;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;
using System.Threading.Tasks;

namespace Player.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly playerContext _context;

        public CountryController(playerContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("ListCountries")]
        public ActionResult ListCountrys([FromBody] CountryRequest request)
        {
            request._context = _context;
            //request._seriLogger = _seriLogger;

            var CountrysResponse = CountryService.ListCountries(request);
            return Ok(new
            {
                CountrysResponse
            });
        }

        

        [HttpPost]
        [Route("DeleteCountry")]
        public ActionResult DeleteCountry([FromBody] CountryRequest request)
        {
            request._context = _context;
            //request._seriLogger = _seriLogger;

            var CountrysResponse = CountryService.DeleteCountry(request);
            return Ok(new
            {
                CountrysResponse
            });
        }

        [HttpPost]
        [Route("AddCountry")]
        public ActionResult AddCountry([FromBody] CountryRequest request)
        {
            request._context = _context;
            //request._seriLogger = _seriLogger;

            var CountrysResponse = CountryService.AddCountry(request);

            return Ok(new
            {
                CountrysResponse
            });
        }

        [HttpPost]
        [Route("EditCountry")]
        public ActionResult EditCountry([FromBody] CountryRequest request)
        {
            request._context = _context;
            //request._seriLogger = _seriLogger;

            var CountrysResponse = CountryService.EditCountry(request);
            return Ok(new
            {
                CountrysResponse
            });
        }
    }
}
