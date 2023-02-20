using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Records
{
    public class PlayerEnergyPointRecord
    {
        public long Id { get; set; }
        public string PlayerId { get; set; }
        public string? ProviderId { get; set; }
        public string? ProviderType { get; set; }
        public int? Points { get; set; }
        public bool IsQuiet { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ProviderName { get; set; }
        public string ProviderImageUrl { get; set; }
    }
}
