using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class Language
    {
        public Language()
        {
            AreaLocalizes = new HashSet<AreaLocalize>();
            CountryLocalizes = new HashSet<CountryLocalize>();
            SportLocalizes = new HashSet<SportLocalize>();
            ZoneLocalizes = new HashSet<ZoneLocalize>();
        }

        public long Id { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public ulong IsDeleted { get; set; }
        public DateTime? ModificationDate { get; set; }
        public long? CreatedBy { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string IconUrl { get; set; }

        public virtual ICollection<AreaLocalize> AreaLocalizes { get; set; }
        public virtual ICollection<CountryLocalize> CountryLocalizes { get; set; }
        public virtual ICollection<SportLocalize> SportLocalizes { get; set; }
        public virtual ICollection<ZoneLocalize> ZoneLocalizes { get; set; }
    }
}
