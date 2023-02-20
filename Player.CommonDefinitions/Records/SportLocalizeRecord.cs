using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Sports.Coach.API.CommonDefinitions.Records
{
    public class SportLocalizeRecord
    {
        public long Id { get; set; }
        public long SportId { get; set; }
        public long LanguageId { get; set; }
        public long? ExternalSportId { get; set; }
        public string Name { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? IsDeleted { get; set; }=false;
        public bool? IsQueueEdit  { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
