using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Player.BL.Services;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using Reservation.Helpers;
using Player.CommonDefinitions.Responses;
using System.Linq;

namespace Player.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaLocalizeController : Controller
    {
        private readonly playerContext _context;
        private readonly IOptions<AppSettingsRecord> appSettings;
        private readonly ILogger<AreaLocalizeController> _logger;
        private readonly IBus _bus;

        public AreaLocalizeController(playerContext context, IOptions<AppSettingsRecord> app, ILogger<AreaLocalizeController> logger, IBus bus)
        {
            _context = context;
            appSettings = app;
            _logger = logger;
            _bus = bus;
        }

        [HttpPost]
        [Route("ListAreaLocalizes")]
        public ActionResult ListAreaLocalizes([FromBody] AreaLocalizeRequest request)
        {
            request._context = _context;
            var AreaLocalizesResponse = AreaLocalizeService.ListAreaLocalizes(request);
            return Ok(new
            {
                AreaLocalizesResponse
            });
        }

        [HttpPost]
        [Route("DeleteAreaLocalize")]
        public ActionResult DeleteAreaLocalize([FromBody] AreaLocalizeRequest request)
        {
            request._context = _context;
            var AreaLocalizesResponse = AreaLocalizeService.DeleteAreaLocalize(request);
           
            if (AreaLocalizesResponse != null && AreaLocalizesResponse.Success && AreaLocalizesResponse.AreaLocalizeRecords?.Count() > 0)
                AreaLocalizeRabbitMQ(AreaLocalizesResponse.AreaLocalizeRecords.FirstOrDefault());
           
            return Ok(new
            {
                AreaLocalizesResponse
            });
        }

        [HttpPost]
        [Route("AddAreaLocalize")]
        public ActionResult AddAreaLocalize([FromBody] AreaLocalizeRequest request)
        {
            request._context = _context;
            var AreaLocalizesResponse = AreaLocalizeService.AddAreaLocalize(request);
            
            if (AreaLocalizesResponse != null && AreaLocalizesResponse.Success && AreaLocalizesResponse.AreaLocalizeRecords?.Count() > 0)
                AreaLocalizeRabbitMQ(AreaLocalizesResponse.AreaLocalizeRecords.FirstOrDefault());

            return Ok(new
            {
                AreaLocalizesResponse
            });
        }

        [HttpPost]
        [Route("EditAreaLocalize")]
        public ActionResult EditAreaLocalize([FromBody] AreaLocalizeRequest request)
        {
            request._context = _context;
            var AreaLocalizesResponse = AreaLocalizeService.EditAreaLocalize(request);
            if (AreaLocalizesResponse != null && AreaLocalizesResponse.Success && AreaLocalizesResponse.AreaLocalizeRecords?.Count() > 0)
            {
                var zaone = AreaLocalizesResponse.AreaLocalizeRecords.FirstOrDefault();
                zaone.IsQueueEdit = true;

                if (zaone != null)
                    AreaLocalizeRabbitMQ(zaone);
            }
            return Ok(new
            {
                AreaLocalizesResponse
            });
        }

        private void AreaLocalizeRabbitMQ(AreaLocalizeRecord AreaLocalize)
        {

            var jsonString = JsonConvert.SerializeObject(AreaLocalize, Formatting.None);
            var task1 = new Task(async () =>
            {
                var AreaLocalizeCoachUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqAreaLocalizeCoachUri;
                Uri AreaLocalizeCoachuri = new Uri(AreaLocalizeCoachUri);
                var endPointAreaLocalizeCoachuri = await _bus.GetSendEndpoint(AreaLocalizeCoachuri);
                await endPointAreaLocalizeCoachuri.Send(AreaLocalize);
            });

            task1.Start();

            var task2 = new Task(async () =>
            {
                var AreaLocalizePhysiotherapistUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqAreaLocalizePhysiotherapistUri;
                Uri AreaLocalizePhysiotherapisturi = new Uri(AreaLocalizePhysiotherapistUri);
                var endPointAreaLocalizePhysiotherapisturi = await _bus.GetSendEndpoint(AreaLocalizePhysiotherapisturi);
                await endPointAreaLocalizePhysiotherapisturi.Send(AreaLocalize);
            });
            task2.Start();
            var task3 = new Task(async () =>
            {
                var AreaLocalizeNutritionistUri = appSettings.Value.RabbitMQ.RabbitMqRootUri + appSettings.Value.RabbitMQ.RabbitMqAreaLocalizeNutritionistUri;
                Uri AreaLocalizeNutritionisturi = new Uri(AreaLocalizeNutritionistUri);
                var endPointAreaLocalizeNutritionisturi = await _bus.GetSendEndpoint(AreaLocalizeNutritionisturi);
                await endPointAreaLocalizeNutritionisturi.Send(AreaLocalize);
            });
            task3.Start();
            try
            {
                Task.WaitAll(task1, task2, task3);
            }
            catch (Exception ex)
            {
                var jsonRequest = System.Text.Json.JsonSerializer.Serialize(AreaLocalize);
                LogHelper.LogException(_context, "AreaLocalizeRabbitMQ", "", jsonRequest, ex);

                //cloud Log
                //_seriLogger.Error(ex, ex.Message, "AreaLocalizeRabbitMQ", "");
                //_logger.LogError("ex.Message : " + ex.Message+ " ex.StackTrace : " + ex.StackTrace);
            }
        }
    }
}
