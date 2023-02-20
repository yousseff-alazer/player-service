using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Records
{
    public class CountryRecord
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public sbyte? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public ulong? IsArabic { get; set; }
        public string PhoneCode { get; set; }
        public string Ibanprefix { get; set; }
        public int? Ibanlength { get; set; }
        public int? ViewOrder { get; set; }
        public decimal? Tax { get; set; }
        public string IconUrl { get; set; }

        public decimal? Vat { get; set; }
        public ulong? IsPercentageVat { get; set; }
        public ulong? IsPercentageTax { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public bool IsAll { get; set; }
    }
}
