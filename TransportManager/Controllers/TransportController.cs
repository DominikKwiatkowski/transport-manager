using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TransportManager.DataModels;
using TransportManager.Services;

namespace TransportManager.Controllers
{
    [ApiController]
    [Route("[transport]")]
    public class TransportController : ControllerBase
    {

        private readonly ILogger<TransportController> _logger;
        private readonly TransportService _transportService;

        public TransportController(ILogger<TransportController> logger, TransportContext context)
        {
            _logger = logger;
            _transportService = new TransportService(context);
        }

        [HttpGet(Name = "GetAll")]
        public IEnumerable<TransportDataModel> GetAll()
        {
            return _transportService.GetAllTransports();
        }

        [HttpGet("{date}{availableSeats}{types}", Name = "GetAllFilter")]
        public IEnumerable<TransportDataModel> GetAllFilter(DateTime date, int availableSeats, TransportType[] types)
        {
            FilterDefinitionBuilder<TransportDataModel> builder = Builders<TransportDataModel>.Filter;
            var filter = builder.Eq(x => x.Date.Date, date.Date) & 
                         builder.Gte(x => x.AvailableSeats, availableSeats);
            if(types.Length > 0 && types.Length + 1 < Enum.GetNames(typeof(TransportType)).Length)
                filter &= builder.Where(x => types.Contains(x.Type));

            return _transportService.GetAllWithFilter(filter);
        }

        [HttpPost(Name = "AddTransport")]
        public void AddTransport([FromBody] TransportDataModel transport)
        {
            _transportService.AddTransport(transport);
        }

        [HttpPost(Name = "UpdateTransport")]
        public void UpdateTransport([FromBody] TransportDataModel transport)
        {
            _transportService.UpdateTransport(transport);
        }

        [HttpDelete("{id}", Name = "RemoveHotel")]
        public void RemoveTransport(int id)
        {
            _transportService.RemoveTransport(id);
        }

        [HttpPost(Name = "Reserve")]
        public ActionResult<int> Reserve([FromBody] TransportData data)
        {
            return _transportService.ReserveTransport(data.Id, data.NumberOfPersons);
        }

        [HttpPost(Name = "Cancel")]
        public ActionResult<int> Cancel([FromBody] TransportData data)
        {
            return _transportService.CancelReservation(data.Id, data.NumberOfPersons);
        }

        [HttpPost(Name = "CalculatePrice")]
        public ActionResult<int> CalculatePrice([FromBody] TransportData data)
        {
            return _transportService.CalculatePrice(data.Id, data.NumberOfPersons);
        }

        public class TransportData
        {
            public int Id { get; set; }
            public int NumberOfPersons { get; set; }
        }
    }
}