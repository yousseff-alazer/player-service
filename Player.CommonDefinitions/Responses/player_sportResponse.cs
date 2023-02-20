using Reservation.CommonDefinitions.Records;
using System.Collections.Generic;

namespace Reservation.CommonDefinitions.Responses
{
    public class Player_SportResponse : BaseResponse
    {

        public List<Player_SportRecord> Player_SportRecords { get; set; }
    }
}