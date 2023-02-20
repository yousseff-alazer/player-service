using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class CountryLocalizeServiceManager
    {
        public static CountryLocalize AddOrEditCountryLocalize(CountryLocalizeRecord CountryLocalizeRecord, CountryLocalize CountryLocalize = null)
        {
            if (CountryLocalize == null)
            {
                CountryLocalize = new CountryLocalize();
                CountryLocalize.CreatedBy = CountryLocalizeRecord.CreatedBy;
                CountryLocalize.CreationDate = DateTime.Now;
                CountryLocalize.CountryId = CountryLocalizeRecord.CountryId;
                CountryLocalize.LanguageId = CountryLocalizeRecord.LanguageId;
            }
            else
            {
                CountryLocalize.ModificationDate = DateTime.Now;
                CountryLocalize.ModifiedBy = CountryLocalizeRecord.ModifiedBy;
            }

            CountryLocalize.Name = CountryLocalizeRecord.Name ?? CountryLocalize.Name;
            CountryLocalize.Code = CountryLocalizeRecord.Code ?? CountryLocalize.Code;
            CountryLocalize.PhoneCode = CountryLocalizeRecord.PhoneCode ?? CountryLocalize.PhoneCode;
            CountryLocalize.Ibanprefix = CountryLocalizeRecord.Ibanprefix ?? CountryLocalize.Ibanprefix;
            CountryLocalize.CurrencyName = CountryLocalizeRecord.CurrencyName ?? CountryLocalize.CurrencyName;
            CountryLocalize.CurrencyCode = CountryLocalizeRecord.CurrencyCode ?? CountryLocalize.CurrencyCode;
            return CountryLocalize;
        }
        public static IQueryable<CountryLocalizeRecord> ApplyFilter(IQueryable<CountryLocalizeRecord> query, CountryLocalizeRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(record.Name.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(record.Code))
            {
                query = query.Where(p => p.Code.ToLower().Contains(record.Code.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(record.PhoneCode))
            {
                query = query.Where(p => p.PhoneCode.ToLower().Contains(record.PhoneCode.ToLower()));
            }

            if (record.Id > 0)
            {
                query = query.Where(s => s.Id == record.Id);
            }

            if (record.IsDeleted.HasValue)
            {
                query = query.Where(p => p.IsDeleted == record.IsDeleted);
            }

            return query;
        }

    }
}
