using Microsoft.AspNetCore.Mvc;
using TranspotTask.Models;
using TranspotTask.Services;

namespace TranspotTask.Controllers
{
    public class TransportController : Controller
    {
        static ITransportTableCreature? table;
        static TransportData? data;

        public TransportController()
        {
            if (data == null)
            {
                table = new TransportTableCreature();
                data = table.GetInitialData();
            }
        }

        public IActionResult Index()
        {
            return View(data);
        }

        [HttpPost]
        public IActionResult AddSource(Source source)
        {
            if (source.Name != null && source.Supply != 0)
                table?.AddSource(source);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddDestination(Destination destination)
        {
            if (destination.Name != null && destination.Demand != 0)
                table?.AddDestination(destination);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SetCost(List<List<double>> costs)
        {
            for (int i = 0; i < data?.Sources.Count; i++)
            {
                for (int j = 0; j < data.Destinations.Count; j++)
                {
                    if (i < costs.Count && j < costs[i].Count)
                    {
                        table?.SetCost(data.Sources[i], data.Destinations[j], costs[i][j]);
                    }
                }
            }

            new TransportDecision().TaskSolution(table);

            return RedirectToAction("Index");
        }
    }
}
