using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Constants;
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
                Filter = DialogTexts.FileDialogFilter,
                Title = DialogTexts.FileDialogTitle
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
                MessageBox.Show($"{ErrorMessages.XmlLoadErrorTitle} {ex.Message}", ErrorMessages.GeneralErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadXml(string path)
        {
            var carSales = CarDataLoader.LoadFromXml(path);

            dataGridAll.ItemsSource = null;
            dataGridSummary.ItemsSource = null;

            if (carSales == null || carSales.Count == 0)
            {
                MessageBox.Show(ErrorMessages.NoCarDataFound, ErrorMessages.InfoTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            dataGridAll.ItemsSource = carSales;

            var saleSummary = CarDataLoader.GenerateWeekendSaleSummary(carSales);
            if (saleSummary == null || saleSummary.Count == 0)
            {
                MessageBox.Show(ErrorMessages.NoWeekendSalesFound, ErrorMessages.InfoTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            dataGridSummary.ItemsSource = saleSummary;
        }
    }
}