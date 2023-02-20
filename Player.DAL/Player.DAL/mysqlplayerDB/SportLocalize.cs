using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class SportLocalize
    {
        public long Id { get; set; }
        public long SportId { get; set; }
        public long LanguageId { get; set; }
        public long? ExternalSportId { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public ulong IsDeleted { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Language Language { get; set; }
        public virtual Sport Sport { get; set; }
    }
}
