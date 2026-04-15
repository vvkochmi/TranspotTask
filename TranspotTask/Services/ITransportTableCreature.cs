using TranspotTask.Models;

namespace TranspotTask.Services
{
    public interface ITransportTableCreature
    {
        TransportData GetInitialData();
        void AddSource(Source? source);
        void DelSource(Source? source);
        void AddDestination(Destination? destination);
        void DelDestination(Destination? destination);
        void SetCost(Source source, Destination destination, double cost);
        double GetCost(Source source, Destination destination);
    }
}
