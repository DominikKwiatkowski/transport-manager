using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransportManager.DataModels
{
    public class TransportDataModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Price { get; set; }
        public int AvailableSeats { get; set; }
        public int AllSeats { get; set; }

        public DateTime Date { get; set; }

        public TransportType Type { get; set; } 

    }
}
