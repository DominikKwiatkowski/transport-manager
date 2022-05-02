namespace TransportManager.DataModels
{
    public class TransportDbSettings
    {
        public string ConnectionString { get; set; } = null;

        public string DatabaseName { get; set; } = null;

        public string TransportCollectionName { get; set; } = null;
    }
}
