namespace TranspotTask.Models
{
    public record class TransportData
    {
        public List<Source> Sources { get; set; } = new List<Source>();
        public List<Destination> Destinations { get; set; } = new List<Destination>();
        public Dictionary<(Source source, Destination destination), double> Costs { get; set; } = new Dictionary<(Source, Destination), double>();
    }
}
