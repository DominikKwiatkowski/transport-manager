using MongoDB.Driver;
using TransportManager.DataModels;

namespace TransportManager.Services
{
    public class TransportService
    {
        readonly TransportContext context;

        public TransportService(TransportContext context)
        {
            this.context = context;
        }

        public List<TransportDataModel> GetAllTransports()
        {
            return context.TransportCollection.Find(x => true).ToList();
        }

        public void AddTransport(TransportDataModel transport)
        {
            context.TransportCollection.InsertOne(transport);
        }

        public void RemoveTransport(int id)
        {
            context.TransportCollection.DeleteOne(x => x.Id == id);
        }

        public void UpdateTransport(TransportDataModel transport)
        {
            context.TransportCollection.ReplaceOne(x => x.Id == transport.Id, transport);
        }

        public List<TransportDataModel> GetAllWithFilter(FilterDefinition<TransportDataModel> filter)
        {
            return context.TransportCollection.Find(filter).ToList();
        }

        public int CalculatePrice(int id, int numberOfPersons)
        {
            var transport = context.TransportCollection.Find(x => x.Id == id).FirstOrDefault();
            if (transport == null)
                return -1;
            return (int)(transport.Price * (2.0 - (double)transport.AvailableSeats / transport.AllSeats) * numberOfPersons);
        }

        public int ReserveTransport(int id, int numberOfPersons)
        {
            var transport = context.TransportCollection.Find(x => x.Id == id).FirstOrDefault();
            if (transport == null)
                return -1;
            if (transport.AvailableSeats < numberOfPersons)
                return -2;
            transport.AvailableSeats -= numberOfPersons;
            context.TransportCollection.ReplaceOne(x => x.Id == id, transport);
            return 0;
        }

        public int CancelReservation(int id, int numberOfPersons)
        {
            var transport = context.TransportCollection.Find(x => x.Id == id).FirstOrDefault();
            if (transport == null)
                return -1;
            if (transport.AvailableSeats + numberOfPersons > transport.AllSeats)
                return -2;
            transport.AvailableSeats += numberOfPersons;
            context.TransportCollection.ReplaceOne(x => x.Id == id, transport);
            return 0;
        }
    }
}
