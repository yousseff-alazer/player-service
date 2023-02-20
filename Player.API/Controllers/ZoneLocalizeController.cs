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
    public class ZoneLocalizeController : Controller
    {
        private readonly playerContext _context;
        private readonly IOptions<AppSettingsRecord> appSettings;
        private readonly ILogger<ZoneLocalizeController> _logger;
        private readonly IBus _bus;

        public ZoneLocalizeController(playerContext context, IOptions<AppSettingsRecord> app, ILogger<ZoneLocalizeController> logger, IBus bus)
        {
            _context = context;
            appSettings = app;
            _logger = logger;
            _bus = bus;
        }

        [HttpPost]
        [Route("ListZoneLocalizes")]
        public ActionResult ListZoneLocalizes([FromBody] ZoneLocalizeRequest request)
        {
            request._context = _context;
            var ZoneLocalizesResponse = ZoneLocalizeService.ListZoneLocalizes(request);
            return Ok(new
            {
                ZoneLocalizesResponse
            });
        }

        [HttpPost]
        [Route("DeleteZoneLocalize")]
        public ActionResult DeleteZoneLocalize([FromBody] ZoneLocalizeRequest request)
        {
            request._context = _context;
            var ZoneLocalizesResponse = ZoneLocalizeService.DeleteZoneLocalize(request);

            if (ZoneLocalizesResponse != null && ZoneLocalizesResponse.Success && ZoneLocalizesResponse.ZoneLocalizeRecords?.Count() > 0)
                ZoneLocalizeRabbitMQ(ZoneLocalizesResponse.ZoneLocalizeRecords.FirstOrDefault());
           
            return Ok(new
            {
                ZoneLocalizesResponse
            });
        }

        [HttpPost]
        [Route("AddZoneLocalize")]
        public ActionResult AddZoneLocalize([FromBody] ZoneLocalizeRequest request)
        {
            request._context = _context;
            var ZoneLocalizesResponse = ZoneLocalizeService.AddZoneLocalize(request);

            if (ZoneLocalizesResponse != null && ZoneLocalizesResponse.Success && ZoneLocalizesResponse.ZoneLocalizeRecords?.Count() > 0)
                ZoneLocalizeRabbitMQ(ZoneLocalizesResponse.ZoneLocalizeRecords.FirstOrDefault());

            return Ok(new
            {
                ZoneLocalizesResponse
            });
        }

        [HttpPost]
        [Route("EditZoneLocalize")]
        public ActionResult EditZoneLocalize([FromBody] ZoneLocalizeRequest request)
        {
            request._context = _context;
            var ZoneLocalizesResponse = ZoneLocalizeService.EditZoneLocalize(request);

            if (ZoneLocalizesResponse != null && ZoneLocalizesResponse.Success && ZoneLocalizesResponse.ZoneLocalizeRecords?.Count() > 0)
            {
                var zaone = ZoneLocalizesResponse.ZoneLocalizeRecords.FirstOrDefault();
                zaone.IsQueueEdit = true;

                if (zaone != null)
                    ZoneLocalizeRabbitMQ(zaone);
            }

            return Ok(new
            {
                ZoneLocalizesResponse
            });
        }

        private void ZoneLocalizeRabbitMQ(ZoneLocalizeRecord ZoneLocalize)
        {

            var jsonString = JsonConvert.SerializeObject(ZoneLocalize, Formatting.None);
            var task1 = new Task(async () =>
            {
                var ZoneLocalizeCoachUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqZoneLocalizeCoachUri;
                Uri ZoneLocalizeCoachuri = new Uri(ZoneLocalizeCoachUri);
                var endPointZoneLocalizeCoachuri = await _bus.GetSendEndpoint(ZoneLocalizeCoachuri);
                await endPointZoneLocalizeCoachuri.Send(ZoneLocalize);
            });

            task1.Start();

            var task2 = new Task(async () =>
            {
                var ZoneLocalizePhysiotherapistUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqZoneLocalizePhysiotherapistUri;
                Uri ZoneLocalizePhysiotherapisturi = new Uri(ZoneLocalizePhysiotherapistUri);
                var endPointZoneLocalizePhysiotherapisturi = await _bus.GetSendEndpoint(ZoneLocalizePhysiotherapisturi);
                await endPointZoneLocalizePhysiotherapisturi.Send(ZoneLocalize);
            });
            task2.Start();
            var task3 = new Task(async () =>
            {
                var ZoneLocalizeNutritionistUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqZoneLocalizeNutritionistUri;
                Uri ZoneLocalizeNutritionisturi = new Uri(ZoneLocalizeNutritionistUri);
                var endPointZoneLocalizeNutritionisturi = await _bus.GetSendEndpoint(ZoneLocalizeNutritionisturi);
                await endPointZoneLocalizeNutritionisturi.Send(ZoneLocalize);
            });
            task3.Start();
            try
            {
                Task.WaitAll(task1, task2, task3);
            }
            catch (Exception ex)
            {
                var jsonRequest = System.Text.Json.JsonSerializer.Serialize(ZoneLocalize);
                LogHelper.LogException(_context, "ZoneLocalizeRabbitMQ", "", jsonRequest, ex);

                //cloud Log
                //_seriLogger.Error(ex, ex.Message, "ZoneLocalizeRabbitMQ", "");
                //_logger.LogError("ex.Message : " + ex.Message+ " ex.StackTrace : " + ex.StackTrace);
            }
        }
    }
}
