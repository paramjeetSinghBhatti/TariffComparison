namespace TariffComparison.Models
{
    public class TariffModel
    {
        public string Name {  get; set; }

        public int Type { get; set; }

        public decimal BaseCost { get; set; }

        public Dictionary<string,object> AdditionalProperties { get; set; }

        //public abstract decimal CalculateAnnualCost(int consumption);
    }
}
