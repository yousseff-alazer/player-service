using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

#nullable disable

namespace Reservation.DAL.DB
{
    public partial class PlayerAgenda
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.Int32)]

        [BsonId()]
        public ObjectId _Id { get; set; }

        public long Id { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [BsonElement("PlayerId")]
        public string PlayerId { get; set; }

        [BsonElement("ProviderId")]
        public string ProviderId { get; set; }

        [BsonElement("ProviderName")]
        public string ProviderName { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("StartTime")]
        public DateTime StartTime { get; set; }
        public long ActivityId { get; set; }
        public string Activity { get; set; }
        public string Status { get; set; }
        public DateTime EndTime { get; set; }
        public string SportName { get; set; }
        public long SlotId { get; set; }
        public string ProviderType { get; set; }
    }
}
