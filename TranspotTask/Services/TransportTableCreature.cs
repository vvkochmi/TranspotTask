using TranspotTask.Models;

namespace TranspotTask.Services
{
    public class TransportTableCreature : ITransportTableCreature
    {
        readonly TransportData data;

        public TransportTableCreature()
        {
            data = new TransportData()
            {
                Sources = new List<Source>()
                {
                    new Source() {Name = "A1", Supply = 200},
                    new Source() {Name = "A2", Supply = 300},
                    new Source() {Name = "A3", Supply = 100},
                },
                Destinations = new List<Destination>()
                {
                    new Destination(){ Name ="B1", Demand = 200},
                    new Destination(){ Name ="B2", Demand = 200},
                    new Destination(){ Name ="B3", Demand = 500},
                }
            };
        }

        public TransportData GetInitialData()
        {
            return data;
        }

        public void AddSource(Source? source)
        {
            if (source != null)
                data.Sources.Add(source);
        }

        public void DelSource(Source? source)
        {
            if (source != null)
                data.Sources.Remove(source);
        }

        public void AddDestination(Destination? destination)
        {
            if (destination != null)
                data.Destinations.Add(destination);
        }

        public void DelDestination(Destination? destination)
        {
            if (destination != null)
                data.Destinations.Remove(destination);
        }

        public void SetCost(Source source, Destination destination, double cost)
        {
            data.Costs[(source, destination)] = cost;
        }

        public double GetCost(Source source, Destination destination)
        {
            return data.Costs.TryGetValue((source, destination), out double cost) ? cost : 0;
        }
    }
}
