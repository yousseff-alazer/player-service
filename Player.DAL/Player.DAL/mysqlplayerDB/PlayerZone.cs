using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class PlayerZone
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string PlayerId { get; set; }
        public decimal Tax { get; set; }
        public ulong IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public ulong IsDeleted { get; set; }

        public virtual PlayerInfo Player { get; set; }
        public virtual Zone Zone { get; set; }
    }
}
