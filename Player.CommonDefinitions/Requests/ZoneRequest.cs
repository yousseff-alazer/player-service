using Player.CommonDefinitions.Records;
using Reservation.CommonDefinitions.MysqlRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Requests
{
    public class ZoneRequest : MysqlBaseRequest
    {
        public ZoneRecord ZoneRecord { get; set; }
    }
}
