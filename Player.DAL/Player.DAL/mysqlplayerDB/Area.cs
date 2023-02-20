using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class Area
    {
        public Area()
        {
            AreaLocalizes = new HashSet<AreaLocalize>();
            Zones = new HashSet<Zone>();
        }

        public int Id { get; set; }
        public string CountryId { get; set; }
        public string Name { get; set; }
        public ulong IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public ulong IsDeleted { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<AreaLocalize> AreaLocalizes { get; set; }
        public virtual ICollection<Zone> Zones { get; set; }
    }
}
