using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
//using Reservation.DAL.DB;
using System.Linq;
using Reservation.Helpers;

namespace Reservation.CommonDefinitions.Records
{
    public class player_agendaRecord
    {
       



        public string activity { get; set; }


        public long activityId { get; set; }


		
        public DateTime? createdAt { get; set; }
	
		
        public DateTime date { get; set; }
	
        public DateTime endTime { get; set; }


        public long Id { get; set; }


        public string playerId { get; set; }


        public DateTime startTime { get; set; }


        public string status { get; set; }
		
        public DateTime? updatedAt { get; set; }
        public long? slotID { get; set; }
        public string providerId { get; set; }
        public string providerName { get; set; }
        public string sportName { get; set; }
        public string providerType { get; set; }
    }

}
