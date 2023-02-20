using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Security.Authentication;

#nullable disable

namespace Reservation.DAL.DB
{
    public class reservationContext
    {
        public reservationContext(IConfiguration configuration)
        {
            // Connect to the Mongo database and obtain reference to Book collection
            MongoClientSettings settings = MongoClientSettings.FromUrl(
                new MongoUrl(configuration.GetConnectionString("MongoPlayerDBStoreContext"))
            );
            settings.SslSettings =
                new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            DB = mongoClient.GetDatabase("PlayerDB");
            PlayerAgenda = DB.GetCollection<PlayerAgenda>("PlayerAgenda");
        }

        // Readonly IMongoCollection, our equivalent to DbSet
        public IMongoCollection<PlayerAgenda> PlayerAgenda { get; }
        public IMongoDatabase DB { get; }
    }

    
}
