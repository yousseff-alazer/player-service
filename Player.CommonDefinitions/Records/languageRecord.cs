using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Connect4Sports.Coach.API.CommonDefinitions.Records
{
    public class languageRecord
    {

        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string IconUrl { get; set; }
        public bool IsQueueEdit { get; set; }
    }

}
