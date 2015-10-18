using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using System.IO;

using MameMiner.Model;
using MameMiner.Repository;
using MameMiner.Service;

namespace MameMiner.WPF
{
    /// <summary>
    /// Interaction logic for RomDetailsView.xaml
    /// </summary>
    public partial class RomDetailsView : UserControl
    {

        IZipFileService _zipFileService;
        IZipFileRepository _zipFileRepository;
        IMameMinerSettingsService _settingsService;

        MameGame _game;

        public RomDetailsView(MameGame game)
        {
            InitializeComponent();
            _game = game;

            if (_game == null)
                return;

            _zipFileRepository = RepositoryManager.GetInstanceOf<IZipFileRepository>();
            _zipFileService = ServiceManager.GetInstanceOf<IZipFileService>();
            _settingsService = ServiceManager.GetInstanceOf<IMameMinerSettingsService>();

            this.GameNameTextBlock.Text = _game.GameName;
            this.GameDescriptionTextBlock.Text = _game.GameDescription;

            Task.Factory.StartNew(() =>
            {

                var hasDuplicates = _game.GroupBy(x => x.RomName).Any(g => g.Count() > 1);

                var sb = new StringBuilder();
                sb.AppendLine(string.Format("Report for : {0}.", game.GameDescription));
                sb.AppendLine("==== Report on Game ====");
                sb.AppendLine(string.Format("Bad Dump? : {0}", game.Any(g => g.BadDump)));
                sb.AppendLine(string.Format("Bad CRC? : {0}", game.Any(g => g.BadCRC)));
                sb.AppendLine(string.Format("Bad SHA1?: {0}", game.Any(g => g.BadSHA1)));
                
                if (game.Any(g => g.BadDump))
                {
                    sb.AppendLine("At least one rom in the collection is recognized as beeing a bad dump. You may not be able to run this game.");
                }

                if(hasDuplicates)
                {
                    sb.AppendLine("At least one rom is a duplicate entry from listrom results. You may not be able to run this game.");
                }

                sb.AppendLine("==== Report on Roms ====");
                var missing = _zipFileRepository.FindMissingRoms(game);

                if (missing.Any())
                {
                    foreach (var m in missing)
                    {
                        sb.AppendLine(string.Format("Missing Rom with name of {0}", m.RomName, m.CRC));
                    }

                    sb.AppendLine("You may not be able to play a game generated with missing roms.");
                }
                else
                {
                    sb.AppendLine("No roms missing.");
                }

                Dispatcher.Invoke(() => GameDetailsTextBlock.Text = sb.ToString());

            });


        }

        public RomDetailsView() : this(null)
        {

        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            ExportButton.IsEnabled = false;

            Task.Factory.StartNew(() =>
            {
                var fName = Path.Combine(_settingsService.GetMameExportPath(), _game.GameName + ".zip");

                if (File.Exists(fName))
                {
                    File.Delete(fName);
                }

                _zipFileService.CreateZipFile(fName);

                foreach (var r in _game)
                {
                    var rx = _zipFileRepository.SearchForRom(r.RomName, r.FileSize, r.CRC).FirstOrDefault();

                    if (rx != null)
                        _zipFileService.AddFileToZipFile(fName, rx.FileName, _zipFileService.ReadFile(rx.ZipFileContainer, rx.FileName));

                }

                Dispatcher.Invoke(() =>
                {

                    ExportButton.IsEnabled = true;
                    MessageBox.Show("Game Export Complete.");

                });

            });

        }
    }
}
