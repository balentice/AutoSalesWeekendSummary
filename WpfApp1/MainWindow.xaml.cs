using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Services;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialogWindow = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*\"",
                Title = "Vyberte XML soubor s daty o prodeji aut"
            };
            if (dialogWindow.ShowDialog() != true)
                return;

            var filePath = dialogWindow.FileName;
            try
            {
                LoadXml(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání dat: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadXml(string path)
        {
            var carSales = CarDataLoader.LoadFromXml(path);
            if (carSales == null || carSales.Count == 0)
            {
                MessageBox.Show("Nebyla nalezena žádná data o prodeji aut.", "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            dataGridAll.ItemsSource = carSales;

            var saleSummary = CarDataLoader.GenerateWeekendSaleSummary(carSales);
            if (saleSummary == null || saleSummary.Count == 0)
            {
                MessageBox.Show("Zadny vikendovy prodej nebyl nalezen.", "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            dataGridSummary.ItemsSource = saleSummary;
        }
    }
}