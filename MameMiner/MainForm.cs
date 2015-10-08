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
using System.Runtime;
using System.Diagnostics;

namespace MameMiner
{
    public partial class MainForm : Form
    {
        GameList _gameList;
        RomDataEngine _romDataEngine;

        /// <summary>
        /// This initializes the master game list in the background on start up and caches it.
        /// </summary>
        private void LoadMasterGameList()
        {
            this.Enabled = false;
            this.ActivityDetailsLabel.Text = "Loading Game List";

            Task.Factory.StartNew(() =>
            {
                _gameList = new GameList();

                Invoke(new Action(() =>
                {
                    this.Enabled = true;
                    this.ActivityDetailsLabel.Text = "Game List Loaded";
                }));
            });
        }

        public MainForm()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.CheckMamePath();
            Program.CheckImportPath();
            Program.CheckExportPath();

            LoadMasterGameList();

            if (!File.Exists("roms.db"))
            {
                _romDataEngine = new RomDataEngine("Roms.db");

                var nx = new ImportRomsForm(_romDataEngine);
                nx.ShowDialog();
            }
            else
            {
                _romDataEngine = new RomDataEngine("Roms.db");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetDatabase()
        {
            if (File.Exists("Roms.db")) {
                if(_romDataEngine != null)
                {
                    _romDataEngine.Dispose();
                    _romDataEngine = null;
                }
                File.Delete("Roms.db");
            }

            _romDataEngine = new RomDataEngine("Roms.db");

            var nx = new ImportRomsForm(_romDataEngine);
            nx.ShowDialog();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameSearchTextBox_TextChanged(object sender, EventArgs e)
        {

            var results = _gameList.SearchGame(this.GameSearchTextBox.Text);

            SearchResultsListBox.ValueMember = "GameDescription";
            SearchResultsListBox.DataSource = results.Take(Math.Min(50, results.Count)).ToList();


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchResultsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SearchResultsListBox.SelectedIndex != -1)
            {
                var g = (GameData)SearchResultsListBox.SelectedItem;
                this.RomDataContainerPanel.Controls.Clear();
                this.RomDataContainerPanel.Controls.Add(new GameDataView(g, _romDataEngine)
                {
                    Dock = DockStyle.Fill
                });

                this.ApplicationLogTextBox.Text = g.FullText;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void regenerateDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetDatabase();
        }
    }
}
