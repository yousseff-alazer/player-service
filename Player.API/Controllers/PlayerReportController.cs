using Microsoft.AspNetCore.Mvc;
using Player.BL.Services;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;

namespace Player.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerReportController : Controller
    {
        private readonly playerContext _context;

        public PlayerReportController(playerContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("ListPlayerReports")]
        public ActionResult ListPlayerReports([FromBody] PlayerReportRequest request)
        {
            request._context = _context;

            var PlayerReportsResponse = PlayerReportService.ListPlayerReports(request);
            return Ok(new
            {
                PlayerReportsResponse
            });
        }

        [HttpPost]
        [Route("DeletePlayerReport")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult DeletePlayerReport([FromBody] PlayerReportRequest request)
        {
            request._context = _context;

            var PlayerReportsResponse = PlayerReportService.DeletePlayerReport(request);
            return Ok(new
            {
                PlayerReportsResponse
            });
        }

        [HttpPost]
        [Route("AddPlayerReport")]
        public ActionResult AddPlayerReport([FromBody] PlayerReportRequest request)
        {
            request._context = _context;
            var PlayerReportsResponse = PlayerReportService.AddPlayerReport(request);
            return Ok(new
            {
                PlayerReportsResponse
            });
        }

        [HttpPost]
        [Route("EditPlayerReport")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult EditPlayerReport([FromBody] PlayerReportRequest request)
        {
            request._context = _context;

            var PlayerReportsResponse = PlayerReportService.EditPlayerReport(request);
            return Ok(new
            {
                PlayerReportsResponse
            });
        }
    }
}
