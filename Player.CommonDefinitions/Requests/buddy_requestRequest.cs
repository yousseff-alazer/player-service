using Reservation.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.CommonDefinitions.MysqlRequests
{
    public class buddy_requestRequest : MysqlBaseRequest
    {
        public buddy_requestRecord Buddy_requestRecord { get; set; }
     
    }
}