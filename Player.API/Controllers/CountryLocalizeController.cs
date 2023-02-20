using Microsoft.AspNetCore.Mvc;
using Player.BL.Services;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Responses;
using System.Linq;
using System;

namespace Player.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryLocalizeController : Controller
    {
        private readonly playerContext _context;

        public CountryLocalizeController(playerContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("ListCountries")]
        public ActionResult ListCountryLocalizes([FromBody] CountryLocalizeRequest request)
        {
            request._context = _context;
            //request._seriLogger = _seriLogger;

            var CountryLocalizesResponse = CountryLocalizeService.ListCountryLocalizes(request);
            return Ok(new
            {
                CountryLocalizesResponse
            });
        }



        [HttpPost]
        [Route("DeleteCountryLocalize")]
        public ActionResult DeleteCountryLocalize([FromBody] CountryLocalizeRequest request)
        {
            request._context = _context;
            //request._seriLogger = _seriLogger;

            var CountryLocalizesResponse = CountryLocalizeService.DeleteCountryLocalize(request);
            return Ok(new
            {
                CountryLocalizesResponse
            });
        }

        [HttpPost]
        [Route("AddCountryLocalize")]
        public ActionResult AddCountryLocalize([FromBody] CountryLocalizeRequest request)
        {
            request._context = _context;
            //request._seriLogger = _seriLogger;

            var CountryLocalizesResponse = CountryLocalizeService.AddCountryLocalize(request);

            return Ok(new
            {
                CountryLocalizesResponse
            });
        }

        [HttpPost]
        [Route("EditCountryLocalize")]
        public ActionResult EditCountryLocalize([FromBody] CountryLocalizeRequest request)
        {
            request._context = _context;
            //request._seriLogger = _seriLogger;

            var CountryLocalizesResponse = CountryLocalizeService.EditCountryLocalize(request);
            return Ok(new
            {
                CountryLocalizesResponse
            });
        }


        /// <summary>
        /// Return Country With id .
        /// </summary>
        [HttpGet("GetCountryLocalize/{Countryid}", Name = "GetCountryLocalize")]
        [Produces("application/json")]
        public IActionResult GetCountryLocalize(string Countryid)
        {
            var CountryLocalizeResponse = new CountryLocalizeResponse();
            try
            {
                var CountryLocalizeRequest = new CountryLocalizeRequest
                {
                    _context = _context,
                    //_serilogger = _seriLogger,
                    CountryLocalizeRecord = new CountryLocalizeRecord
                    {
                        CountryId = Countryid
                    }
                };
                CountryLocalizeResponse = CountryLocalizeService.ListCountryLocalizes(CountryLocalizeRequest);
            }
            catch (System.Exception ex)
            {
                CountryLocalizeResponse.Message = ex.Message;
                CountryLocalizeResponse.Success = false;
                //LogHelper.LogException(ex.Message, ex.StackTrace);
            }
            return Ok(CountryLocalizeResponse);
        }

        /// <summary>
        /// Creates CountryLocalize.
        /// </summary>
        [HttpPost]
        [Route("AddCountryLocalizes")]
        [Produces("application/json")]
        public IActionResult AddCountryLocalizes([FromBody] CountryLocalizeRequest model)
        {
            var CountryLocalizeResponse = new CountryLocalizeResponse();
            try
            {
                if (model == null)
                {
                    CountryLocalizeResponse.Message = "Empty Body";
                    CountryLocalizeResponse.Success = false;
                    return Ok(CountryLocalizeResponse);
                }
                CountryLocalizeResponse.Success = true;
                var editedTranslateCountry = model.CountryLocalizeRecords.Where(c => c.Id > 0).ToList();
                if (editedTranslateCountry != null && editedTranslateCountry.Count() > 0)
                {
                    var editReq = new CountryLocalizeRequest
                    {
                        _context = _context,
                        //_serilogger = _seriLogger,
                        // BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase,
                        CountryLocalizeRecords = editedTranslateCountry
                    };
                    CountryLocalizeResponse = CountryLocalizeService.EditCountryLocalizes(editReq);
                }

                var addedTranslateCountry = model.CountryLocalizeRecords.Where(c => c.Id == 0).ToList();
                if (addedTranslateCountry != null && addedTranslateCountry.Count() > 0)
                {
                    var addReq = new CountryLocalizeRequest
                    {
                        _context = _context,
                        //_serilogger = _seriLogger,

                        // BaseUrl = Request.Scheme + "://" + Request.Host.Value + Request.PathBase,
                        CountryLocalizeRecords = addedTranslateCountry
                    };
                    CountryLocalizeResponse = CountryLocalizeService.AddCountryLocalizes(addReq);
                }

            }
            catch (Exception ex)
            {
                CountryLocalizeResponse.Message = ex.Message + ex.StackTrace;
                CountryLocalizeResponse.Success = false;
                //LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return Ok(CountryLocalizeResponse);
        }
    }
}
