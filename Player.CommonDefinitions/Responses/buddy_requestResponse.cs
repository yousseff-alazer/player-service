using Reservation.CommonDefinitions.Records;
using System.Collections.Generic;

namespace Reservation.CommonDefinitions.Responses
{
    public class Buddy_requestResponse : BaseResponse
    {

        public List<buddy_requestRecord> buddy_requestRecords { get; set; }
    }
}