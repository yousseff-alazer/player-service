
using MassTransit;
using Reservation.CommonDefinitions.Records;
using System.Threading.Tasks;

namespace Connect4Sports.Player.API.Consumers
{
    public class PlayerQuoteConsumer : IConsumer<player_agendaRecord>
    {
        public async Task Consume(ConsumeContext<player_agendaRecord> context)
        {
            var data = context.Message;
            //if(data != null&&data.id>0&&data.isPaid!=null) {
            //    var request = new orderRequest();
            //    request.orderRecord = data;
            //    request._context = new CrossCuttingLayer.DAL.DB.orderContext();

            //    var orderResponse = orderService.Editorder(request);
            //    //return orderResponse;
            //}
        }
    }
}
