using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using Review.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class CountryServiceManager
    {
        public static Country AddOrEditCountry(CountryRecord CountryRecord, Country Country = null)
        {
            if (Country == null)
            {
                Country = new Country();
                if (CountryRecord != null)
                {
                    Country.CreatedBy = CountryRecord.CreatedBy;
                }

                Country.CreatedOn = DateTime.Now;
                Country.IsActive = 1;

            }
            else
                Country.UpdatedOn = DateTime.Now;

            if (CountryRecord.IsActive!=null)
            {
                Country.IsActive = CountryRecord.IsActive.Value;
            }
            Country.Code = CountryRecord.Code ?? Country.Code;
            Country.NameAr = CountryRecord.NameAr ?? Country.NameAr;
            Country.NameEn = CountryRecord.NameEn ?? Country.NameEn;
            Country.IsArabic = CountryRecord.IsArabic ?? Country.IsArabic;
            Country.PhoneCode = CountryRecord.PhoneCode ?? Country.PhoneCode;
            Country.Ibanlength = CountryRecord.Ibanlength ?? Country.Ibanlength;
            Country.Ibanprefix = CountryRecord.Ibanprefix ?? Country.Ibanprefix;
            Country.IconUrl = CountryRecord.IconUrl ?? Country.IconUrl;
            Country.ViewOrder = CountryRecord.ViewOrder ?? Country.ViewOrder;
            Country.Tax = CountryRecord.Tax ?? Country.Tax;

            Country.Vat = CountryRecord.Vat ?? Country.Vat;
            Country.IsPercentageTax = CountryRecord.IsPercentageTax ?? Country.IsPercentageTax;
            Country.IsPercentageVat = CountryRecord.IsPercentageVat ?? Country.IsPercentageVat;
            Country.CurrencyCode = CountryRecord.CurrencyCode ?? Country.CurrencyCode;
            Country.CurrencyName = CountryRecord.CurrencyName ?? Country.CurrencyName;

            return Country;
        }

        public static IQueryable<CountryRecord> ApplyFilter(IQueryable<CountryRecord> query, CountryRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.Code))
            {
                query = query.Where(p => p.Code.Contains(record.Code));
            }
            if (!string.IsNullOrWhiteSpace(record.Id))
            {
                query = query.Where(p => p.Id.Contains(record.Id));
            }
            if (!string.IsNullOrWhiteSpace(record.PhoneCode))
            {
                query = query.Where(p => p.PhoneCode.Contains(record.PhoneCode));
            }

            if (!string.IsNullOrWhiteSpace(record.CreatedBy))
            {
                query = query.Where(p => p.CreatedBy.Contains(record.CreatedBy));
            }


            if (!string.IsNullOrWhiteSpace(record.NameEn))
            {
                query = query.Where(p => p.NameEn == record.NameEn);
            }

            if (!string.IsNullOrWhiteSpace(record.Ibanprefix))
            {
                query = query.Where(p => p.Ibanprefix == record.Ibanprefix);
            }

            if (record.Ibanlength.HasValue)
            {
                query = query.Where(p => p.Ibanlength == record.Ibanlength);
            }

            if (record.ViewOrder.HasValue)
            {
                query = query.Where(p => p.ViewOrder == record.ViewOrder);
            }

            if (record.Tax.HasValue)
            {
                query = query.Where(p => p.Tax == record.Tax);
            }

            if (!string.IsNullOrWhiteSpace(record.NameAr))
            {
                query = query.Where(p => p.NameAr == record.NameAr);
            }

            if (!string.IsNullOrWhiteSpace(record.Id))
            {
                query = query.Where(s => s.Id == record.Id);
            }

            if (record.IsArabic.HasValue)
            {
                query = query.Where(p => p.IsArabic == record.IsArabic);
            }

            if (record.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == record.IsActive);
            }

            return query;
        }

    }
}
