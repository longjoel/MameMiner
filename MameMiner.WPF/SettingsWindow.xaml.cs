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
using System.Windows.Shapes;

using MameMiner.Service;

using Microsoft.Win32;

using FolderPickerLib;


namespace MameMiner.WPF
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        private IMameMinerSettingsService _settingsService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingsService"></param>
        public SettingsWindow(IMameMinerSettingsService settingsService)
        {
            InitializeComponent();

            _settingsService = settingsService;

            if(_settingsService != null)
            {
                this.GameExportPathTextBox.Text = _settingsService.GetMameExportPath();
                this.PathToMameExeTextBox.Text = _settingsService.GetMameExecutablePath();
                this.RomImportPathTextBox.Text = _settingsService.GetMameImportPath();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SettingsWindow():this(null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            if (_settingsService != null)
            {
                _settingsService.SetMameExportPath(this.GameExportPathTextBox.Text);
                _settingsService.SetMameExecutablePath(this.PathToMameExeTextBox.Text);
                _settingsService.SetMameImportPath(this.RomImportPathTextBox.Text);
            }
        }

        private void PathToMameExeButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                CheckFileExists = true,
                DefaultExt = ".exe",
                Filter = "(.exe)|*.exe",
                Multiselect = false,
                Title = "Mame File Location"
            };

            ofd.ShowDialog();
            
            if(ofd.FileName != string.Empty)
            {
                this.PathToMameExeTextBox.Text = ofd.FileName;
            } 

        }

        private void RomImportPathButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new FolderPickerDialog()
            {
                Title = "Rom Import Path"
            };

            ofd.ShowDialog();

            if(ofd.SelectedPath != string.Empty)
            {
                this.RomImportPathTextBox.Text = ofd.SelectedPath;
            }

        }

        private void GameExportPathButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new FolderPickerDialog()
            {
                Title = "Game Export Path"
            };

            ofd.ShowDialog();

            if (ofd.SelectedPath != string.Empty)
            {
                this.GameExportPathTextBox.Text = ofd.SelectedPath;
            }

        }
    }
}
