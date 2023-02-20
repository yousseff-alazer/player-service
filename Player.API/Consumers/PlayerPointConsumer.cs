using MassTransit;
using Microsoft.Extensions.Options;
using Player.BL.Services;
using Player.CommonDefinitions.Enums;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;
using Reservation.BL.Services;
using System.Threading.Tasks;

namespace Player.API.Consumers
{
    public class PlayerPointConsumer : IConsumer<PlayerEnergyPointRecord>
    {
        private readonly playerContext _context;
        private readonly IOptions<AppSettingsRecord> _appSettings;

        public PlayerPointConsumer(IOptions<AppSettingsRecord> appSettings)
        {
            _appSettings = appSettings;
            _context = new playerContext(BaseService.GetDBContextConnectionOptions(_appSettings.Value.DatabaseSettings.ConnectionString));
        }
        public async Task Consume(ConsumeContext<PlayerEnergyPointRecord> context)
        {
            var data = context.Message;
            if (data != null && data.Points != null)
            {
                var isSuccess = false;
                var request = new PlayerEnergyPointRequest
                {
                    _context = _context,
                    PlayerEnergyPointRecord = new PlayerEnergyPointRecord
                    {
                        PlayerId = data.PlayerId,
                        ProviderId = data.ProviderId,
                        ProviderType = data.ProviderType,
                        ProviderName = data.ProviderName,
                        ProviderImageUrl = data.ProviderImageUrl,
                        Points = data.Points,
                    }
                };
                if(!data.IsQuiet)
                {
                    var PlayerEnergyPointResponse = PlayerEnergyPointService.AddPlayerEnergyPoint(request);
                    isSuccess= PlayerEnergyPointResponse.Success;
                }
                else
                {
                    var PlayerEnergyPointResponse = PlayerEnergyPointService.DeletePlayerEnergyPoint(request);
                    isSuccess = PlayerEnergyPointResponse.Success;
                }
                
                if(isSuccess)
                    PlayerInfoService.UpdatePlayerPoint(_context, data.PlayerId, (int)data.Points, data.IsQuiet);

            }
        }
    }
}
