using Reservation.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.CommonDefinitions.Requests
{
    public class player_agendaRequest : BaseRequest
    {
        public player_agendaRecord player_agendaRecord { get; set; }
        public string ValidateSlotApiBaseUrl { get;  set; }
        public string UpdateSoltsApiBaseUrl { get; set; }
        public string UpdateQuoteApiBaseUrl { get; set; }
        public string ValidateQuoteApiBaseUrl { get; set; }
        public string ValidateSlotApiBaseNUTRITIONISTUrl { get; set; }
        public string ValidateSlotApiBaseUrlPHYSIOTHERAPIST { get; set; }
        public string RabbitMqUri { get; set; }
    }
}