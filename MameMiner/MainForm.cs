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

        private void SearchForGame()
        {
            //this.ApplicationLogTextBox.Text = string.Join(Environment.NewLine, _gameList.SearchGame(this.GameSearchTextBox.Text));


        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadMasterGameList();
        }

        private void GameSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            //this.Enabled = false;

            //Task.Factory.StartNew(() => {
            var results = _gameList.SearchGame(this.GameSearchTextBox.Text);


            // Invoke( new Action( ()=> {
            SearchResultsListBox.ValueMember = "GameDescription";
            SearchResultsListBox.DataSource = results.Take(Math.Min(50, results.Count)).ToList();
            // this.Enabled = true;

            //    }));
            //});

        }


        private void SearchResultsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SearchResultsListBox.SelectedIndex != -1)
            {
                var g = (GameData)SearchResultsListBox.SelectedItem;

                this.ApplicationLogTextBox.Text = g.FullText;
            }
        }
    }
}
