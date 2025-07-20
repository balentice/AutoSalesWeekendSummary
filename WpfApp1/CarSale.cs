namespace WpfApp1
{
    public class CarSale
    {
        public string Model { get; set; }
        public DateTime DatumProdeje { get; set; }
        public double Cena { get; set; }
        public double DPH { get; set; }
        public double CenaSDPH => Cena * (1 + DPH / 100.0);
    }
}
