using Microsoft.AspNetCore.Mvc;
using Player.BL.Services;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;
using System.Threading.Tasks;
using System;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Reservation.Helpers;
using Player.CommonDefinitions.Responses;
using System.Linq;

namespace Player.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : Controller
    {
        private readonly playerContext _context;
        private readonly IOptions<AppSettingsRecord> appSettings;
        private readonly ILogger<AreaController> _logger;
        private readonly IBus _bus;

        public AreaController(playerContext context, IOptions<AppSettingsRecord> app, ILogger<AreaController> logger, IBus bus)
        {
            _context = context;
            appSettings = app;
            _logger = logger;
            _bus = bus;
        }

        [HttpPost]
        [Route("ListAreas")]
        public ActionResult ListAreas([FromBody] AreaRequest request)
        {
            request._context = _context;
            var AreasResponse = AreaService.ListAreas(request);
            return Ok(new
            {
                AreasResponse
            });
        }

        [HttpPost]
        [Route("DeleteArea")]
        public ActionResult DeleteArea([FromBody] AreaRequest request)
        {
            request._context = _context;
            var AreasResponse = AreaService.DeleteArea(request);

            if (AreasResponse != null && AreasResponse.Success && AreasResponse.AreaRecords?.Count() > 0)
                AreaRabbitMQ(AreasResponse.AreaRecords.FirstOrDefault());

            return Ok(new
            {
                AreasResponse
            });
        }

        [HttpPost]
        [Route("AddArea")]
        public ActionResult AddArea([FromBody] AreaRequest request)
        {
            request._context = _context;
            var AreasResponse = AreaService.AddArea(request);

            if (AreasResponse != null && AreasResponse.Success && AreasResponse.AreaRecords?.Count() > 0)
                AreaRabbitMQ(AreasResponse.AreaRecords.FirstOrDefault());

            return Ok(new
            {
                AreasResponse
            });
        }

        [HttpPost]
        [Route("EditArea")]
        public ActionResult EditArea([FromBody] AreaRequest request)
        {
            request._context = _context;
            var AreasResponse = AreaService.EditArea(request);
            if (AreasResponse != null && AreasResponse.Success && AreasResponse.AreaRecords?.Count() > 0)
            {
                var zaone = AreasResponse.AreaRecords.FirstOrDefault();
                zaone.IsQueueEdit = true;

                if (zaone != null)
                    AreaRabbitMQ(zaone);
            }
            return Ok(new
            {
                AreasResponse
            });
        }

        private void AreaRabbitMQ(AreaRecord Area)
        {

            var jsonString = JsonConvert.SerializeObject(Area, Formatting.None);
            var task1 = new Task(async () =>
            {
                var AreaCoachUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqAreaCoachUri;
                Uri AreaCoachuri = new Uri(AreaCoachUri);
                var endPointAreaCoachuri = await _bus.GetSendEndpoint(AreaCoachuri);
                await endPointAreaCoachuri.Send(Area);
            });

            task1.Start();

            var task2 = new Task(async () =>
            {
                var AreaPhysiotherapistUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqAreaPhysiotherapistUri;
                Uri AreaPhysiotherapisturi = new Uri(AreaPhysiotherapistUri);
                var endPointAreaPhysiotherapisturi = await _bus.GetSendEndpoint(AreaPhysiotherapisturi);
                await endPointAreaPhysiotherapisturi.Send(Area);
            });
            task2.Start();
            var task3 = new Task(async () =>
            {
                var AreaNutritionistUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqAreaNutritionistUri;
                Uri AreaNutritionisturi = new Uri(AreaNutritionistUri);
                var endPointAreaNutritionisturi = await _bus.GetSendEndpoint(AreaNutritionisturi);
                await endPointAreaNutritionisturi.Send(Area);
            });
            task3.Start();
            try
            {
                Task.WaitAll(task1, task2, task3);
            }
            catch (Exception ex)
            {
                var jsonRequest = System.Text.Json.JsonSerializer.Serialize(Area);
                LogHelper.LogException(_context, "AreaRabbitMQ", "", jsonRequest, ex);

                //cloud Log
                //_seriLogger.Error(ex, ex.Message, "AreaRabbitMQ", "");
                //_logger.LogError("ex.Message : " + ex.Message+ " ex.StackTrace : " + ex.StackTrace);
            }
        }
    }
}
