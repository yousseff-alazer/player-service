using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Records
{
    public class PlayerZoneRecord
    {
        public int Id { get; set; }
        public int? ZoneId { get; set; }
        public string PlayerId { get; set; }
        public decimal? Tax { get; set; }
        public ulong? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public ulong? IsDeleted { get; set; }
        public string ZoneName { get; set; }
        public string AreaName { get; set; }
    }
}
