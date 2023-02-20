using Connect4Sports.Coach.API.CommonDefinitions.Records;
using MassTransit;
using Microsoft.Extensions.Options;
using Player.BL.Services;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;
using Reservation.BL.Services;
using System.Threading.Tasks;

namespace Player.API.Consumers
{
	public class LanguageConsumer : IConsumer<languageRecord>
	{
		private readonly IOptions<AppSettingsRecord> appSettings;
		private readonly playerContext _context;
		public LanguageConsumer(IOptions<AppSettingsRecord> app)
		{
			appSettings = app;
			_context = new playerContext(BaseService.GetDBContextConnectionOptions(appSettings.Value.DatabaseSettings.ConnectionString));
		}
		public async Task Consume(ConsumeContext<languageRecord> context)
		{
			var data = context.Message;
			bool isSuccess = false;
			if (data != null)
			{
				var request = new LanguageRequest
				{
					_context = _context,
					languageRecord = new languageRecord
					{
						Id = data.Id,
						IconUrl = data.IconUrl,
						Name = data.Name,
						IsDeleted = data.IsDeleted,
						IsQueueEdit = data.IsQueueEdit,
						Code = data.Code,
						ModificationDate = data.ModificationDate,
						CreatedBy = data.CreatedBy,
						CreationDate = data.CreationDate,
						ModifiedBy = data.ModifiedBy,
					}
				};
				if (data.IsDeleted.Value)
				{
					var LanguageResponse = LanguageService.DeleteLanguage(request);
					isSuccess = LanguageResponse.Success;
				}
				else if (data.IsQueueEdit)
				{
					var LanguageResponse = LanguageService.Editlanguage(request);
					isSuccess = LanguageResponse.Success;
				}
				else
				{
					var LanguageResponse = LanguageService.Addlanguage(request);
					isSuccess = LanguageResponse.Success;
				}
			}
		}
	}
}
