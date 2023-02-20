using Reservation.CommonDefinitions.Records;
using System.Collections.Generic;

namespace Reservation.CommonDefinitions.Responses
{
    public class player_agendaResponse : BaseResponse
    {

        public List<player_agendaRecord> player_agendaRecords { get; set; }
    }
}