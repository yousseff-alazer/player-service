using Reservation.CommonDefinitions.Records;
using System.Collections.Generic;

namespace Reservation.CommonDefinitions.Responses
{
    public class Player_profileResponse : BaseResponse
    {

        public List<player_profileRecord> player_profileRecords { get; set; }
        public int Connections { get; set; }
    }
}