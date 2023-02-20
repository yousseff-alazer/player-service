using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class PlayerEnergyPoint
    {
        public long Id { get; set; }
        public string PlayerId { get; set; }
        public string ProviderId { get; set; }
        public string ProviderType { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ProviderName { get; set; }
        public string ProviderImageUrl { get; set; }

        public virtual PlayerInfo Player { get; set; }
    }
}
