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

using MameMiner.Model;
using MameMiner.Repository;
using MameMiner.Service;

namespace MameMiner.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IZipFileService _zipFileService;
        IZipFileRepository _zipFileRepository;
        IMameGameRepository _gameRepository;
        IMameMinerSettingsService _settingsService;

        public void SafeInvoke(Action a)
        {
            try { Dispatcher.Invoke(() => a()); }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(string status)
        {
            SafeInvoke(() => this.ApplicationStatusText.Text = status);
        }

        public void SetProgress(double percent)
        {
            SafeInvoke(() =>
            {
                this.ActivityProgress.Maximum = 100.0;
                this.ActivityProgress.Minimum = 0.0;
                this.ActivityProgress.Value = percent;

            });
        }

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            _zipFileRepository = RepositoryManager.GetInstanceOf<IZipFileRepository>();
            _gameRepository = RepositoryManager.GetInstanceOf<IMameGameRepository>();
            _settingsService = ServiceManager.GetInstanceOf<IMameMinerSettingsService>();
            _zipFileService = ServiceManager.GetInstanceOf<IZipFileService>();

            InitializeComponent();

            SetStatus("Main Window Created");


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow(_settingsService).ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateZipFileDBMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GenerateZipFileDBMenuItem.IsEnabled = false;
            BuildFileDatabase();
        }

        /// <summary>
        /// 
        /// </summary>
        void SearchForRoms()
        {
            var searchQuery = SearchTextBox.Text;

            this.SearchResultsListBox.DisplayMemberPath = "GameDescription";
            this.SearchResultsListBox.Items.Clear();

            SetStatus("Searching for: " + searchQuery);

            SearchTextBox.IsEnabled = false;

            bool expandedSearch = AdvanceSearchExpander.IsExpanded;

            var nList = new List<int>();
            if (expandedSearch)
            {
                if (P1CheckBox.IsChecked.Value) nList.Add(1);
                if (P2CheckBox.IsChecked.Value) nList.Add(2);
                if (P3CheckBox.IsChecked.Value) nList.Add(3);
                if (P4CheckBox.IsChecked.Value) nList.Add(4);
            }

            Task.Factory.StartNew(() =>
            {

                IEnumerable<MameGame> games = null;
                if (expandedSearch)
                {
                   

                    games = _gameRepository.SearchForGame(searchQuery, nList, 100);

                }
                else
                {
                    games = _gameRepository.SearchForGame(searchQuery, 100);

                }

                foreach (var g in games)
                {

                    this.Dispatcher.Invoke(() => this.SearchResultsListBox.Items.Add(g));
                    SetStatus(string.Format("Found: " + g.GameDescription));

                }

                SafeInvoke(() => SearchTextBox.IsEnabled = true);
                SetStatus(string.Format("Search Complete!"));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        void BuildFileDatabase()
        {
            int i = 0;
            var files = _zipFileService.ReadAllFileNames();

            int max = files.Count();

            Task.Factory.StartNew(() =>
            {
                SetStatus("Buidling Database...");



                files.ForEach(f =>
                {
                    ProcessZipFile(f);
                    double percent = ((double)(i + 1) / (double)(max)) * 100.0; i++;
                    SetProgress(percent);


                });

                SetProgress(0);

            });



        }

        private void ProcessZipFile(string f)
        {
            if (f.ToLower().Contains(".zip"))
            {
                foreach (var fx in _zipFileRepository.GetZipFileContents(f))
                {
                    // Avoid duplicates
                    if (!_zipFileRepository.SearchForRom(fx.FileName, fx.FileSize, fx.CRC).Any())
                    {
                        fx.ZipFileContainer = f;
                        _zipFileRepository.InsertRom(fx);
                        SetStatus(string.Format("{0} added to database.", fx.FileName));
                    }
                    else
                    {
                        SetStatus(string.Format("{0} already in db.", fx.FileName));
                    }
                }
            }
            else
            {
                SetStatus(string.Format("Skipping non-zip file {0}.", f));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                SearchForRoms();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchResultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.RomDetailsContainerGrid.Children.Clear();

            if (SearchResultsListBox.SelectedIndex != -1)
            {
                this.RomDetailsContainerGrid.Children.Add(new RomDetailsView((MameGame)SearchResultsListBox.SelectedItem));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_settingsService.GetMameExecutablePath() == string.Empty || _settingsService.GetMameExportPath() == string.Empty || _settingsService.GetMameImportPath() == string.Empty)
            {
                new SettingsWindow(_settingsService).ShowDialog();
            }
        }
    }
}
