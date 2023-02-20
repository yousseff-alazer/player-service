using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Player.CommonDefinitions.Records;
using Reservation.CommonDefinitions.MysqlRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Requests
{
    public class AreaRequest : MysqlBaseRequest
    {
        public AreaRecord AreaRecord { get; set; }

    }
}
