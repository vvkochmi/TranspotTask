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
                    new Source() {Name = "A1", Supply = 90},
                    new Source() {Name = "A2", Supply = 30},
                    new Source() {Name = "A3", Supply = 40},
                },
                Destinations = new List<Destination>()
                {
                    new Destination(){ Name ="B1", Demand = 70},
                    new Destination(){ Name ="B2", Demand = 30},
                    new Destination(){ Name ="B3", Demand = 20},
                    new Destination(){ Name ="B3", Demand = 40}
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
            data.Costs[(source, destination)] = new Cost() { Price = cost };
        }

        public double GetCost(Source source, Destination destination)
        {
            return data.Costs.TryGetValue((source, destination), out var cost) ? cost.Price : 0;
        }
    }
}
