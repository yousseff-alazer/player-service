
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Connect4Sports.Coach.API.CommonDefinitions.Records
{
    public class sportRecord
    {
        public DateTime createdAt { get; set; }
        public long id { get; set; }
        public string name { get; set; }
        public DateTime updatedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string IconUrl { get; set; }
        public bool IsQueueEdit { get; set; }
        public long SportId { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }

}

