namespace WpfApp1.Models
{
    public class SaleSummary
    {

        public string Model { get; set; } = string.Empty;
        public double TotalPriceWithoutVat { get; set; }
        public double TotalPriceWithVat { get; set; }
    }

}
