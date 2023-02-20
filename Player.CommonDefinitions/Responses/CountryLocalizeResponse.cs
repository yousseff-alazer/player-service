using Player.CommonDefinitions.Records;
using Reservation.CommonDefinitions.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Responses
{
    public class CountryLocalizeResponse : BaseResponse
    {
        public List<CountryLocalizeRecord> CountryLocalizeRecords { get; set; }
    }
}
