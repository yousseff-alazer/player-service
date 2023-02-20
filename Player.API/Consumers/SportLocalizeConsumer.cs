using Player.BL.Services;
using Connect4Sports.Coach.API.CommonDefinitions.Records;
using MassTransit;
using Player.BL.Services;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Player.CommonDefinitions.Records;
using Reservation.BL.Services;

namespace Challenge.API.Consumers
{
    public class SportLocalizeConsumer : IConsumer<SportLocalizeRecord>
    {
        private readonly playerContext _context;
        private readonly IOptions<AppSettingsRecord> _appSettings;

        public SportLocalizeConsumer(IOptions<AppSettingsRecord> appSettings)
        {
            _appSettings = appSettings;
            _context = new playerContext(BaseService.GetDBContextConnectionOptions(_appSettings.Value.DatabaseSettings.ConnectionString));
        }
        public async Task Consume(ConsumeContext<SportLocalizeRecord> context)
        {
            var data = context.Message;
            bool isSuccess = false;
            if (data != null)
            {
                var request = new SportLocalizeRequest
                {
                    _context = _context,
                    SportLocalizeRecord = new SportLocalizeRecord
                    {
                        ExternalSportId = data.SportId,
                        Name = data.Name,
                        IsDeleted = data.IsDeleted,
                        IsQueueEdit = data.IsQueueEdit ,
                        LanguageId= data.LanguageId    
                    }
                };
                if (data.IsDeleted.Value)
                {
                    var SportResponse = SportLocalizeService.DeleteSportLocalize(request);
                    isSuccess = SportResponse.Success;
                }
                else if (data.IsQueueEdit .Value)
                {
                    var SportResponse = SportLocalizeService.EditSportLocalize(request);
                    isSuccess = SportResponse.Success;
                }
                else
                {
                    var SportResponse = SportLocalizeService.AddSportLocalize(request);
                    isSuccess = SportResponse.Success;
                }
            }
        }
    }
}
