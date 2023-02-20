using Reservation.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.CommonDefinitions.MysqlRequests
{
    public class player_blockRequest : MysqlBaseRequest
    {
        public player_blockRecord player_blockRecord { get; set; }
     
    }
}