using Reservation.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.CommonDefinitions.MysqlRequests
{
    public class Player_ProfileRequest : MysqlBaseRequest
    {
        public player_profileRecord Player_profileRecord { get; set; }
     
    }
}