using TranspotTask.Models;

namespace TranspotTask.Services
{
    public class TransportDecision
    {
        TransportData? data;

        public void TaskSolution(ITransportTableCreature table)
        {
            data = table.GetInitialData();

            double sumSupply = data.Sources.Sum(s => s.Supply);
            double sumDemand = data.Destinations.Sum(d => d.Demand);

            if (sumSupply != sumDemand)
                AddVirtual(table, sumSupply, sumDemand);

            NorthWestCorner();
            //LowestCost();

            if ((data.Sources.Count + data.Destinations.Count - 1) != data.Costs.Count(c => c.Value.Count.HasValue))
                AddPlan();

            
        }

        void AddPlan()
        {
            var filledCosts = data.Costs.Where(c => c.Value.Count.HasValue).ToList();
            List<(Source, Destination)> values = filledCosts.Select(item => item.Key).ToList();

            CheckPlan(values, data.Costs.First().Key);

            var updateElem = data.Costs
                .Where(el => el.Value.Count == null)
                .Where(el => values.Any(v => el.Key.Item1 == v.Item1 || el.Key.Item2 == v.Item2))
                .OrderBy(el => el.Value.Price)
                .FirstOrDefault();

            updateElem.Value?.Count = 0;
        }

        void CheckPlan(List<(Source, Destination)> values, (Source, Destination) cell)
        {
            values.Remove(cell);

            List<(Source, Destination)> neighbors = values.Where(v => v.Item1 == cell.Item1 || v.Item2 == cell.Item2).ToList();

            for (int i = 0; i < neighbors.Count; i++)
            {
                CheckPlan(values, neighbors[i]);
            }
        }

        void AddVirtual(ITransportTableCreature table, double sumSupply, double sumDemand)
        {
            if (sumSupply > sumDemand)
            {
                Destination destination = new Destination() { Name = "Фиктивный", Demand = sumSupply - sumDemand };
                table.AddDestination(destination);
                foreach (Source item in data.Sources)
                    table.SetCost(item, destination, 0);
            }
            else if (sumSupply < sumDemand)
            {
                Source source = new Source() { Name = "Фиктивный", Supply = sumDemand - sumSupply };
                table.AddSource(source);
                foreach (Destination item in data.Destinations)
                    table.SetCost(source, item, 0);
            }
        }

        void NorthWestCorner()
        {
            double[] supply = data.Sources.Select(s => s.Supply).ToArray();
            double[] demand = data.Destinations.Select(d => d.Demand).ToArray();

            int i = 0, j = 0;

            while (i < supply.Length && j < demand.Length)
            {
                double sum = Math.Min(supply[i], demand[j]);
                data.Costs[(data.Sources[i], data.Destinations[j])].Count = sum;

                supply[i] -= sum;
                demand[j] -= sum;

                if (supply[i] == 0)
                    i++;
                if (demand[j] == 0)
                    j++;
            }
        }

        void LowestCost()
        {
            double[] supply = data.Sources.Select(s => s.Supply).ToArray();
            double[] demand = data.Destinations.Select(d => d.Demand).ToArray();

            while (true)
            {
                // Находим индекс минимальной стоимости
                int minCostRow = -1;
                int minCostCol = -1;
                double? minCostValue = double.MaxValue;

                for (int i = 0; i < supply.Length; i++)
                {
                    for (int j = 0; j < demand.Length; j++)
                    {
                        if (supply[i] > 0 && demand[j] > 0 && data.Costs[(data.Sources[i], data.Destinations[j])].Price < minCostValue)
                        {
                            minCostValue = data.Costs[(data.Sources[i], data.Destinations[j])].Price;
                            minCostRow = i;
                            minCostCol = j;
                        }
                    }
                }

                // Если не найдено подходящее место, то выходим из цикла
                if (minCostRow == -1 || minCostCol == -1)
                    break;

                // Вычисляем количество, которое можем выделить
                double allocatedAmount = Math.Min(supply[minCostRow], demand[minCostCol]);
                data.Costs[(data.Sources[minCostRow], data.Destinations[minCostCol])].Count = allocatedAmount; // Заполняем план

                // Уменьшаем запасы и потребности
                supply[minCostRow] -= allocatedAmount;
                demand[minCostCol] -= allocatedAmount;
            }
        }

        void PlanOptimization()
        {

        }
    }
}
