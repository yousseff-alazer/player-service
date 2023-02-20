using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Microsoft.AspNetCore.Mvc;
using Player.BL.Services;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Player.CommonDefinitions.Records;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Reservation.Helpers;
using Player.CommonDefinitions.Responses;
using System.Linq;

namespace Player.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneController : Controller
    {
        private readonly playerContext _context;
        private readonly IOptions<AppSettingsRecord> appSettings;
        private readonly ILogger<ZoneController> _logger;
        private readonly IBus _bus;

        public ZoneController(playerContext context, IOptions<AppSettingsRecord> app, ILogger<ZoneController> logger, IBus bus)
        {
            _context = context;
            appSettings = app;
            _logger = logger;
            _bus = bus;
        }

        [HttpPost]
        [Route("ListZones")]
        public ActionResult ListZones([FromBody] ZoneRequest request)
        {
            request._context = _context;
            var ZonesResponse = ZoneService.ListZones(request);
            return Ok(new
            {
                ZonesResponse
            });
        }

        [HttpPost]
        [Route("DeleteZone")]
        public ActionResult DeleteZoneAsync([FromBody] ZoneRequest request)
        {
            request._context = _context;
            var ZonesResponse = ZoneService.DeleteZone(request);

            if (ZonesResponse != null && ZonesResponse.Success && ZonesResponse.ZoneRecords?.Count() > 0)
                ZoneRabbitMQ(ZonesResponse.ZoneRecords.FirstOrDefault());

            return Ok(new
            {
                ZonesResponse
            });
        }

        [HttpPost]
        [Route("AddZone")]
        public ActionResult AddZoneAsync([FromBody] ZoneRequest request)
        {
            request._context = _context;
            var ZonesResponse = ZoneService.AddZone(request);

            if (ZonesResponse != null && ZonesResponse.Success && ZonesResponse.ZoneRecords?.Count() > 0)
                ZoneRabbitMQ(ZonesResponse.ZoneRecords.FirstOrDefault());

            return Ok(new
            {
                ZonesResponse
            });
        }

        [HttpPost]
        [Route("EditZone")]
        public ActionResult EditZone([FromBody] ZoneRequest request)
        {
            request._context = _context;
            var ZonesResponse = ZoneService.EditZone(request);
            if (ZonesResponse != null && ZonesResponse.Success && ZonesResponse.ZoneRecords?.Count() > 0)
            {
                var zaone = ZonesResponse.ZoneRecords.FirstOrDefault();
                zaone.IsQueueEdit = true;

                if (zaone != null)
                    ZoneRabbitMQ(zaone);
            }
            return Ok(new
            {
                ZonesResponse
            });
        }

        private void ZoneRabbitMQ(ZoneRecord Zone)
        {

            var jsonString = JsonConvert.SerializeObject(Zone, Formatting.None);
            var task1 = new Task(async () =>
            {
                var ZoneCoachUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqZoneCoachUri;
                Uri ZoneCoachuri = new Uri(ZoneCoachUri);
                var endPointZoneCoachuri = await _bus.GetSendEndpoint(ZoneCoachuri);
                await endPointZoneCoachuri.Send(Zone);
            });

            task1.Start();

            var task2 = new Task(async () =>
            {
                var ZonePhysiotherapistUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqZonePhysiotherapistUri;
                Uri ZonePhysiotherapisturi = new Uri(ZonePhysiotherapistUri);
                var endPointZonePhysiotherapisturi = await _bus.GetSendEndpoint(ZonePhysiotherapisturi);
                await endPointZonePhysiotherapisturi.Send(Zone);
            });
            task2.Start();
            var task3 = new Task(async () =>
            {
                var ZoneNutritionistUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqZoneNutritionistUri;
                Uri ZoneNutritionisturi = new Uri(ZoneNutritionistUri);
                var endPointZoneNutritionisturi = await _bus.GetSendEndpoint(ZoneNutritionisturi);
                await endPointZoneNutritionisturi.Send(Zone);
            });
            task3.Start();
            try
            {
                Task.WaitAll(task1, task2, task3);
            }
            catch (Exception ex)
            {
                var jsonRequest = System.Text.Json.JsonSerializer.Serialize(Zone);
                LogHelper.LogException(_context, "ZoneRabbitMQ", "", jsonRequest, ex);

                //cloud Log
                //_seriLogger.Error(ex, ex.Message, "ZoneRabbitMQ", "");
                //_logger.LogError("ex.Message : " + ex.Message+ " ex.StackTrace : " + ex.StackTrace);
            }
        }
    }
}
