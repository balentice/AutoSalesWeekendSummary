using System.Globalization;
using System.IO;
using System.Xml.Linq;
using WpfApp1.Constants;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public static class CarDataLoader
    {
        public static List<CarSale> LoadFromXml(string path)
        {
            var xmlDocument = XDocument.Load(path);
            if (xmlDocument.Root == null)
                throw new InvalidDataException(ErrorMessages.XmlRootMissing);

            return xmlDocument.Root
                .Elements(XmlConstants.CarTag)
                .Select(ParseCarSale)
                .OfType<CarSale>()
                .ToList();
        }

        public static List<SaleSummary> GenerateWeekendSaleSummary(List<CarSale> sales)
        {
            return sales
                .Where(sale =>
                sale.SaleDate.DayOfWeek == DayOfWeek.Saturday ||
                sale.SaleDate.DayOfWeek == DayOfWeek.Sunday)
                .GroupBy(sale => sale.Model)
                .OrderBy(group => group.Key)
                .Select(group => new SaleSummary
                {
                    Model = group.Key,
                    TotalPriceWithoutVat = group.Sum(sale => sale.Price),
                    TotalPriceWithVat = group.Sum(sale => sale.PriceWithVat)
                }).ToList();
        }

        private static CarSale? ParseCarSale(XElement element)
        {
            if (element == null)
                return null;

            string model = (string?)element.Element(XmlConstants.ModelTag) ?? XmlConstants.UnknownModel;

            bool hasDate = DateTime.TryParse((string?)element.Element(XmlConstants.SaleDateTag), out DateTime saleDate);
            bool hasPrice = double.TryParse((string?)element.Element(XmlConstants.PriceTag),
                                             NumberStyles.Any, CultureInfo.InvariantCulture, out double price);
            bool hasVat = double.TryParse((string?)element.Element(XmlConstants.VatTag),
                                             NumberStyles.Any, CultureInfo.InvariantCulture, out double vat);

            if (!hasDate || !hasPrice || !hasVat)
                return null;


            return new CarSale
            {
                Model = model,
                SaleDate = saleDate,
                Price = price,
                VatPercent = vat
            };
        }

    }
}
