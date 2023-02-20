using Player.CommonDefinitions.Records;
using Reservation.CommonDefinitions.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Responses
{
    public class AreaLocalizeResponse : BaseResponse
    {
        public List<AreaLocalizeRecord> AreaLocalizeRecords { get; set; }

    }
}
