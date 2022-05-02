using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TransportManager.DataModels;

namespace TransportManager
{
    public class TransportContext
    {
        private readonly IMongoDatabase _database;
        public readonly IMongoCollection<TransportDataModel> TransportCollection;

        public TransportContext(IOptions<TransportDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.DatabaseName);
                TransportCollection = _database.GetCollection<TransportDataModel>(settings.Value.TransportCollectionName);
            }
        }
    }
}