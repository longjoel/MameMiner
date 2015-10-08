using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MameMiner
{
    static class Program
    {

        public static void CheckMamePath()
        {
            if (Properties.Settings.Default.MamePath == string.Empty)
            {
                var ofd = new OpenFileDialog()
                {
                    DefaultExt = ".exe",
                    CheckFileExists = true,
                    AddExtension = true,
                    CheckPathExists = true,
                    Title = "Open Mame Location",
                    Filter = "Mame Exe File (*.exe)|*.exe"

                };

                var result = ofd.ShowDialog();

                if (result != DialogResult.OK || !ofd.FileName.ToLower().Contains("mame"))
                {
                    MessageBox.Show("You need to select a mame executable in order to continue.");
                    return;
                }

                Properties.Settings.Default.MamePath = ofd.FileName;
                Properties.Settings.Default.Save();
            }
        }

        public static void CheckImportPath()
        {
            if (Properties.Settings.Default.RomArchivePath == string.Empty)
            {
                var ofd = new FolderBrowserDialog();
                ofd.Description = "Rom Archive Path";

                var result = ofd.ShowDialog();

                if (result != DialogResult.OK)
                {
                    MessageBox.Show("You need to select a folder containing games you want to import.");
                    return;
                }

                Properties.Settings.Default.RomArchivePath = ofd.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        public static void CheckExportPath()
        {
            if (Properties.Settings.Default.RomExportPath == string.Empty)
            {
                var ofd = new FolderBrowserDialog();
                ofd.Description = "Game Destination Path";
                var result = ofd.ShowDialog();

                if (result != DialogResult.OK)
                {
                    MessageBox.Show("You need to select a folder for exporting games.");
                    return;
                }

                Properties.Settings.Default.RomExportPath = ofd.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);

            Application.Run(new MainForm());
        }
    }
}
