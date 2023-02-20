using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using Reservation.CommonDefinitions.MysqlRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Requests
{
    public class AreaLocalizeRequest : MysqlBaseRequest
    {
        public AreaLocalizeRecord AreaLocalizeRecord { get; set; }
    }
}
