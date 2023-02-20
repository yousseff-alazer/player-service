using Reservation.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Reservation.DAL.DB;
using System.Security.Claims;
using Player.DAL.mysqlplayerDB;

namespace Reservation.CommonDefinitions.MysqlRequests
{
    public class MysqlBaseRequest
    {
        public playerContext _context;

        public int DefaultPageSize = 500;

        public bool IsDesc { get; set; }

        public string OrderByColumn { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public long LanguageId { get; set; }

        public string BaseUrl { get; set; }

    }
}
