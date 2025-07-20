using System.IO;
using System.Xml.Linq;

namespace WpfApp1
{
    public static class CarDataLoader
    {
        public static List<CarSale> LoadFromXml(string path)
        {
            var xmlDocument = XDocument.Load(path);
            if (xmlDocument == null || xmlDocument.Root == null)
                throw new InvalidDataException("Nesprávný formát XML: kořenový element chybí");

            var carSales = xmlDocument.Root
                .Elements("Auto")
                .Select(ParseCarSale)
                .OfType<CarSale>()
                .ToList();

            return carSales;
        }

        public static List<SaleSummary> GenerateWeekendSaleSummary(List<CarSale> sales)
        {
            var weekendSales = FilterWeekendSales(sales);

            var saleSummary = weekendSales
                .GroupBy(sale => sale.Model)
                .OrderBy(g => g.Key)
                .Select(grCarSale => new SaleSummary
                {
                    Model = grCarSale.Key,
                    CenaBezDPH = grCarSale.Sum(sale => sale.Cena),
                    CenaSDPH = grCarSale.Sum(x => x.CenaSDPH)
                }).ToList();

            return saleSummary;
        }

        private static List<CarSale> FilterWeekendSales(List<CarSale> sales)
        {
            return sales
                .Where(sale =>
                    sale.DatumProdeje.DayOfWeek == DayOfWeek.Saturday ||
                    sale.DatumProdeje.DayOfWeek == DayOfWeek.Sunday)
                .ToList();
        }
        private static CarSale? ParseCarSale(XElement element)
        {
            if (element == null)
                return null;

            string model = (string?)element.Element("Model") ?? "Neznámý model";

            if (!DateTime.TryParse((string?)element.Element("DatumProdeje"), out DateTime datum) 
                || !double.TryParse((string?)element.Element("Cena"), out double cena)
                || !double.TryParse((string?)element.Element("DPH"), out double dph))
                return null;


            return new CarSale
            {
                Model = model,
                DatumProdeje = datum,
                Cena = cena,
                DPH = dph
            };
        }

    }
}
