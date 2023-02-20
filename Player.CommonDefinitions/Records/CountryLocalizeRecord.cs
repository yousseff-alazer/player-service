using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Records
{
    public class CountryLocalizeRecord
    {
        public long Id { get; set; }
        public string CountryId { get; set; }
        public long LanguageId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public ulong? IsDeleted { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string PhoneCode { get; set; }
        public string Ibanprefix { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
    }
}
