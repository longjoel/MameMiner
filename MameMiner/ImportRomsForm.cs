using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;

namespace MameMiner
{
    public partial class ImportRomsForm : Form
    {
        RomDataEngine _dataEngine;
        public ImportRomsForm(RomDataEngine dataEngine)
        {
            _dataEngine = dataEngine;
            InitializeComponent();
        }

        public ImportRomsForm() : this(null) { }

        private void ImportRomsForm_Load(object sender, EventArgs e)
        {
            var files = Directory.EnumerateFiles(Properties.Settings.Default.RomArchivePath, "*.zip", SearchOption.AllDirectories).ToList();
            if(files.Count() == 0)
            {
                this.Close();
                return;
            }

            this.LoadingRomsProgressBar.Maximum = files.Count() - 1;

            Task.Factory.StartNew(() =>
            {
                for (int iFile = 0; iFile < files.Count(); iFile++)
                {
                    
                    var f = new ICSharpCode.SharpZipLib.Zip.ZipFile(files[iFile]);
                    foreach(ZipEntry fx in f)
                    {

                        _dataEngine.Insert(files[iFile], fx.Name, fx.Size.ToString(), fx.Crc.ToString());
                    }



                    Invoke(new Action(() =>
                    {
                        this.LoadingRomsProgressBar.Value = iFile;
                        this.FileNameXOfYLabel.Text = string.Format("{0}: {1} of {2}", files[iFile],
                            iFile + 1,
                            files.Count());
                    }));

                    System.Threading.Thread.Sleep(0);

                }

                Invoke(new Action(() =>
                {
                    this.Close();
                }));


            });
        }
    }
}
