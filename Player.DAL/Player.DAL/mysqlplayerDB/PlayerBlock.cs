using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class PlayerBlock
    {
        public long Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string PlayerId { get; set; }
        public sbyte IsDeleted { get; set; }
        public string CreatedById { get; set; }

        public virtual PlayerInfo CreatedBy { get; set; }
        public virtual PlayerInfo Player { get; set; }
    }
}
