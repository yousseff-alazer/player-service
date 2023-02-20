using Connect4Sports.Coach.API.CommonDefinitions.Records;
using MassTransit;
using Player.BL.Services;
using Player.DAL.mysqlplayerDB;
using System.Threading.Tasks;
using Player.CommonDefinitions.Requests;
using Microsoft.Extensions.Options;
using Player.CommonDefinitions.Records;
using Reservation.BL.Services;

namespace Challenge.API.Consumers
{
    public class SportConsumer : IConsumer<sportRecord>
    {
        private readonly playerContext _context;
        private readonly IOptions<AppSettingsRecord> _appSettings;

        public SportConsumer(IOptions<AppSettingsRecord> appSettings)
        {
            _appSettings = appSettings;
            _context = new playerContext(BaseService.GetDBContextConnectionOptions(_appSettings.Value.DatabaseSettings.ConnectionString));
        }

        public async Task Consume(ConsumeContext<sportRecord> context)
        {
            var data = context.Message;
            bool isSuccess=false;
            if (data != null)
            {
                var request = new SportRequest
                {
                    _context = _context,
                    sportRecord = new sportRecord
                    {
                        SportId = data.id,
                        IconUrl = data.IconUrl,
                        name = data.name,
                        IsDeleted = data.IsDeleted,
                        IsQueueEdit  = data.IsQueueEdit ,
                    }
                };
                if(data.IsDeleted.Value)
                {
                    var SportResponse = SportService.DeleteSport(request);
                    isSuccess = SportResponse.Success;
                }
                else if (data.IsQueueEdit )
                {
                    var SportResponse = SportService.EditSport(request);
                    isSuccess = SportResponse.Success;
                }
                else
                {
                    var SportResponse = SportService.AddSport(request);
                    isSuccess = SportResponse.Success;
                }
            }
        }
    }
}
