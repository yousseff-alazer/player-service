using Reservation.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.CommonDefinitions.MysqlRequests
{
    public class Player_SportRequest : MysqlBaseRequest
    {
        public Player_SportRecord Player_SportRecord { get; set; }
     
    }
}