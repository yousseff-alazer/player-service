using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
	public class LanguageServiceManager
	{
		public static Language AddOrEditlanguage(/*int createdBy,*/languageRecord languageRecord, Language language = null)
		{

			if (language == null) //new offerType
			{
				language = new Language();
				language.CreationDate = languageRecord.CreationDate;
			}
			else
			{
				language.ModificationDate = languageRecord.ModificationDate;

			}
			if (!string.IsNullOrEmpty(languageRecord.Name))
				language.Name = languageRecord.Name;

			if (!string.IsNullOrEmpty(languageRecord.IconUrl))
				language.IconUrl = languageRecord.IconUrl;

			if (!string.IsNullOrEmpty(languageRecord.Code))
				language.Code = languageRecord.Code;

			return language;
		}

		public static IQueryable<languageRecord> ApplyFilter(IQueryable<languageRecord> query, languageRecord record)
		{
			if (record.Id > 0)
			{
				query = query.Where(p => p.Id == record.Id);
			}
			
			if (!string.IsNullOrWhiteSpace(record.Name))
			{
				query = query.Where(p => p.Name != null && p.Name.Contains(record.Name));
			}
			if (!string.IsNullOrWhiteSpace(record.Code))
			{
				query = query.Where(p => p.Code != null && p.Code.Contains(record.Code));
			}
			
			return query;
		}

	}
}
