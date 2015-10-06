using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MameMiner
{
    public partial class GameDataView : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        private class RomFileReportEntry
        {
            public string FileName { get; private set; }
            public string FileOK { get; private set; }

            public RomFileReportEntry(string fileName, string fileOK)
            {
                FileName = fileName;
                FileOK = fileOK;
            }
        }

        private GameData _gameData;
        RomDataEngine _dataEngine;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameData"></param>
        /// <param name="dataEngine"></param>
        public GameDataView(GameData gameData, RomDataEngine dataEngine)
        {
            _dataEngine = dataEngine;
            _gameData = gameData;

            InitializeComponent();
        }

        public GameDataView() : this(null,null) { }

        private void GameDataView_Load(object sender, EventArgs e)
        {
            this.Enabled = false;

            this.RomNameLabel.Text = _gameData.GameName;
            this.GameDescriptionLabel.Text = _gameData.GameDescription;
            var romReport = new List<RomFileReportEntry>();
            var report = "OK";

            Task.Factory.StartNew(() => {
                for (int i = 0; i < _gameData.FileNames.Count; i++)
                {
                    string fileStatus = "OK";

                    var qr = _dataEngine.Query(_gameData.FileNames[i], _gameData.Crc32s[i]);

                    if (qr.Count == 0)
                    {
                        report = "One or more missing files.";
                        fileStatus = "NOT FOUND";
                    }
                    var rx = new RomFileReportEntry(_gameData.FileNames[i], fileStatus);
                    romReport.Add(rx);
                }
                Invoke(new Action( ()=> {
                    this.FileMappingDataGridView.DataSource = romReport;

                    this.ReportLabel.Text = report;

                    this.Enabled = true;
                    if (report == "OK")
                        this.ExportButton.Enabled = true;
                } ));
                
            });

            
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {

        }
    }
}
