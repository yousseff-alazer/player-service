using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Records
{
    public class PlayerReportRecord
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string PlayerId { get; set; }
        public string Comment { get; set; }
        public ulong? IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PlayerName { get; set; }
    }
}
