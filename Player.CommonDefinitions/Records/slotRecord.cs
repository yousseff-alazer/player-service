using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
//using Reservation.DAL.DB;
using System.Linq;
using Reservation.Helpers;

namespace Reservation.CommonDefinitions.Records
{
    public class slotRecord
    {
        public long coachId { get; set; }
        public DateTime createdAt { get; set; }
        public string day { get; set; }
        public DateTime endTime { get; set; }
        public long id { get; set; }
        public bool isReserved { get; set; }
        public DateTime startTime { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime validTo { get; set; }
    }

}
