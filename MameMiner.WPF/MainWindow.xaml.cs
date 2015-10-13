using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MameMiner.Repository;
using MameMiner.Service;

namespace MameMiner.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IZipFileRepository _zipFileRepository;
        IMameGameRepository _gameRepository;
        IMameMinerSettingsService _settingsService;

       
        public void SetStatus(string status)
        {
            this.ApplicationStatusText.Text = status;
        }

        public MainWindow()
        {
            _zipFileRepository = RepositoryManager.GetInstanceOf<IZipFileRepository>();
            _gameRepository = RepositoryManager.GetInstanceOf<IMameGameRepository>();
            _settingsService = ServiceManager.GetInstanceOf<IMameMinerSettingsService>();

            InitializeComponent();

            SetStatus("Main Window Created");

            if(_settingsService.GetMameExecutablePath() == string.Empty || _settingsService.GetMameExportPath() == string.Empty || _settingsService.GetMameImportPath() == string.Empty)
            {

            }
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow(_settingsService).ShowDialog();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GenerateZipFileDBMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
