using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Records
{
    public class ZoneRecord
    {
        public int Id { get; set; }
        public int? AreaId { get; set; }
        public string Name { get; set; }
        public ulong? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public ulong? IsDeleted { get; set; }
        public bool IsQueueEdit { get; set; }
    }
}
