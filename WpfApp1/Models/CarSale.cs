namespace WpfApp1.Models
{
    public class CarSale
    {
        public string Model { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public double Price { get; set; }
        public double VatPercent { get; set; }

        public double PriceWithVat => Price * (1 + VatPercent / 100.0);
    }
}
