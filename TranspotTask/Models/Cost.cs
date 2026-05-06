namespace TranspotTask.Models
{
    public record class Cost
    {
        public double Price { get; set; }
        public double? Count { get; set; } = null;
    }
}
