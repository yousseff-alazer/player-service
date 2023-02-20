using Player.CommonDefinitions.Records;
using Reservation.CommonDefinitions.MysqlRequests;
using Reservation.CommonDefinitions.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Requests
{
    public class PlayerReportRequest : MysqlBaseRequest
    {
        public PlayerReportRecord PlayerReportRecord { get; set; }
    }
}
