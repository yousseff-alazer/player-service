using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using Reservation.CommonDefinitions.MysqlRequests;
using Reservation.CommonDefinitions.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Requests
{
    public class CountryRequest : MysqlBaseRequest
    {
        public CountryRecord CountryRecord { get; set; }
    }
}
