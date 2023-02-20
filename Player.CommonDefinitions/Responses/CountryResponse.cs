using Player.CommonDefinitions.Records;
using Reservation.CommonDefinitions.Records;
using Reservation.CommonDefinitions.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Responses
{
    public class CountryResponse : BaseResponse
    {
        public List<CountryRecord> CountryRecords { get; set; }

    }
}
