using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Reservation.BL.Services;
using Reservation.CommonDefinitions.Requests;
using Reservation.DAL.DB;
using System;
using MassTransit;
using Player.DAL.mysqlplayerDB;

namespace Reservation.Portal.Controllers
{
    [Route("api/[controller]")]
    public class player_agendaController : Controller
    {
        private readonly reservationContext _context;
        private readonly playerContext _playerContext;
        private readonly IConfiguration configuration;
        private readonly IBus _bus;

        public player_agendaController(reservationContext context,
                             playerContext playerContext,
                             IConfiguration _configuration, IBus bus)
        {
            _context = context;
            _playerContext = playerContext;
            configuration = _configuration;
            _bus = bus;

        }

        [HttpPost]
        [Route("playerAgendaList")]
        public ActionResult List([FromBody] player_agendaRequest request)
        {

            request._context = _context;
            request._playerContext = _playerContext;
            var player_agendaResponse = player_agendaService.Listplayer_agenda(request);

            return Ok(new
            {
                player_agendaResponse
            });
        }


        //[HttpPost]
        //[Route("playerAgendaDelete")]

        //public ActionResult Delete([FromBody] player_agendaRequest request)
        //{
        //    request._context = _context;
        //    var player_agendaResponse = player_agendaService.Deleteplayer_agenda(request);
        //    return Ok(new
        //    {
        //        player_agendaResponse
        //    });
        //}

        [Route("playerAgendaAdd")]
        [HttpPost]
        public ActionResult Add([FromBody] player_agendaRequest request)
        {
            request._context = _context;
            request._playerContext = _playerContext;

            request.ValidateSlotApiBaseUrl = configuration.GetValue<string>("ValidateSlotApiBaseUrl");
            request.ValidateSlotApiBaseNUTRITIONISTUrl = configuration.GetValue<string>("ValidateSlotApiBaseNUTRITIONISTUrl");
            request.ValidateSlotApiBaseUrlPHYSIOTHERAPIST = configuration.GetValue<string>("ValidateSlotApiBaseUrlPHYSIOTHERAPIST");
            request.UpdateSoltsApiBaseUrl = configuration.GetValue<string>("UpdateSoltsApiBaseUrl");
            request.UpdateQuoteApiBaseUrl = configuration.GetValue<string>("UpdateQuoteApiBaseUrl");
            request.ValidateQuoteApiBaseUrl = configuration.GetValue<string>("ValidateQuoteApiBaseUrl");
            request.RabbitMqUri = configuration.GetValue<string>("RabbitMqUri");

            request._bus = _bus;
            var player_agendaResponse = player_agendaService.Addplayer_agenda(request);
            return Ok(new{ player_agendaResponse});
        }

        [HttpPost]
        [Route("playerAgendaUpdate")]
        public ActionResult Update([FromBody] player_agendaRequest request)
        {
            request._context = _context;
            request._playerContext = _playerContext;
            var leaderBoradApiBaseUrl = configuration.GetValue<string>("ValidateSlotApiBaseUrl");
            request.ValidateSlotApiBaseNUTRITIONISTUrl = configuration.GetValue<string>("ValidateSlotApiBaseNUTRITIONISTUrl");
            request.ValidateSlotApiBaseUrlPHYSIOTHERAPIST = configuration.GetValue<string>("ValidateSlotApiBaseUrlPHYSIOTHERAPIST");
            request.UpdateSoltsApiBaseUrl = configuration.GetValue<string>("UpdateSoltsApiBaseUrl");
            request.UpdateQuoteApiBaseUrl = configuration.GetValue<string>("UpdateQuoteApiBaseUrl");
            request.ValidateQuoteApiBaseUrl = configuration.GetValue<string>("ValidateQuoteApiBaseUrl");
            request.RabbitMqUri = configuration.GetValue<string>("RabbitMqUri");
            request.ValidateSlotApiBaseUrl = leaderBoradApiBaseUrl;
            var player_agendaResponse = player_agendaService.Editplayer_agenda(request);
            return Ok(new { player_agendaResponse });
        }

    }
}