using Reservation.CommonDefinitions.Records;
using System.Collections.Generic;

namespace Reservation.CommonDefinitions.Responses
{
    public class Player_blockResponse : BaseResponse
    {

        public List<player_blockRecord> player_blockRecords { get; set; }
    }
}