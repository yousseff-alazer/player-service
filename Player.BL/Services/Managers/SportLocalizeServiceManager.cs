using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class SportLocalizeServiceManager
    {
        public static SportLocalize AddOrEditSportLocalize(SportLocalizeRecord SportLocalizeRecord, SportLocalize SportLocalize = null)
        {
            if (SportLocalize == null)
            {
                SportLocalize = new SportLocalize();
                SportLocalize.CreatedBy = SportLocalizeRecord.CreatedBy;
                SportLocalize.CreationDate = DateTime.Now;
                SportLocalize.SportId = SportLocalizeRecord.SportId;
                SportLocalize.ExternalSportId = SportLocalizeRecord.ExternalSportId;
                SportLocalize.LanguageId = SportLocalizeRecord.LanguageId;
            }
            else
            {
                SportLocalize.ModificationDate = DateTime.Now;
                SportLocalize.ModifiedBy = SportLocalizeRecord.ModifiedBy;
            }

            SportLocalize.Name = SportLocalizeRecord.Name ?? SportLocalize.Name;
            return SportLocalize;
        }
        public static IQueryable<SportLocalizeRecord> ApplyFilter(IQueryable<SportLocalizeRecord> query, SportLocalizeRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(record.Name.ToLower()));
            }

            if (record.Id > 0)
            {
                query = query.Where(s => s.Id == record.Id);
            }

            if (record.IsDeleted.Value)
            {
                query = query.Where(p => p.IsDeleted.Value);
            }

            return query;
        }
    }
}
