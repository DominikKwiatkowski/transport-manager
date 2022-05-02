using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TransportManager.DataModels;
using TransportManager.Services;

namespace TransportManager.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TransportController : ControllerBase
    {

        private readonly ILogger<TransportController> _logger;
        private readonly TransportService _transportService;

        public TransportController(ILogger<TransportController> logger, TransportContext context)
        {
            _logger = logger;
            _transportService = new TransportService(context);
        }

        [HttpGet(Name = "GetTransports")]
        public IEnumerable<TransportDataModel> GetTransports()
        {
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][GetTransports]GetTransports called");
            var transports = _transportService.GetAllTransports();
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][GetTransports]GetTransports returns {transports.Count}");
            return transports;
        }

        [HttpGet("{date},{availableSeats},{types}", Name = "GetAllFilter")]
        public IEnumerable<TransportDataModel> GetAllFilter(DateTime date, int availableSeats, TransportType[] types)
        {
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][GetAllFilter]GetAllFilter called");
            FilterDefinitionBuilder<TransportDataModel> builder = Builders<TransportDataModel>.Filter;
            var filter = builder.Eq(x => x.Date.Date, date.Date) & 
                         builder.Gte(x => x.AvailableSeats, availableSeats);
            if(types.Length > 0 && types.Length + 1 < Enum.GetNames(typeof(TransportType)).Length)
                filter &= builder.Where(x => types.Contains(x.Type));

            var transports = _transportService.GetAllWithFilter(filter);
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][GetAllFilter]GetAllFilter returns {transports.Count}");
            return transports;
        }

        [HttpPost(Name = "AddTransport")]
        public void AddTransport([FromBody] TransportDataModel transport)
        {
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][AddTransport]AddTransport called");
            _transportService.AddTransport(transport);
        }

        [HttpPost(Name = "UpdateTransport")]
        public void UpdateTransport([FromBody] TransportDataModel transport)
        {
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][UpdateTransport]UpdateTransport called");
            _transportService.UpdateTransport(transport);
        }

        [HttpDelete("{id}", Name = "RemoveHotel")]
        public void RemoveTransport(int id)
        {
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][RemoveTransport]RemoveTransport called");
            _transportService.RemoveTransport(id);
        }

        [HttpPost(Name = "Reserve")]
        public ActionResult<int> Reserve([FromBody] TransportData data)
        {
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][Reserve]Reserve called");
            var result = _transportService.ReserveTransport(data.Id, data.NumberOfPersons);
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][Reserve]Reserve returns {result}");
            return result;
        }

        [HttpPost(Name = "Cancel")]
        public ActionResult<int> Cancel([FromBody] TransportData data)
        {
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][Cancel]Cancel called");
            var result = _transportService.CancelReservation(data.Id, data.NumberOfPersons);
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][Cancel]Cancel returns {result}");
            return result;
        }

        [HttpPost(Name = "CalculatePrice")]
        public ActionResult<int> CalculatePrice([FromBody] TransportData data)
        {
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][CalculatePrice]CalculatePrice called");
            var price = _transportService.CalculatePrice(data.Id, data.NumberOfPersons);
            _logger.LogInformation(
                $"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}][CalculatePrice]CalculatePrice returns {price}");
            return price;
        }

        public class TransportData
        {
            public int Id { get; set; }
            public int NumberOfPersons { get; set; }
        }
    }
}