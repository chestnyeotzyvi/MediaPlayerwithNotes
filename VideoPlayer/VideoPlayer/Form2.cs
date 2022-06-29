using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoPlayer
{
    public partial class Form2 : Form
    {
        public bool StartPlayList;
        public bool MediaChange;
        public Form2()
        {
            InitializeComponent();
            this.Text = "Плейлист";
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StartPlayList)
            {
                StartPlayList = false;
                return;
            }

            if (MediaChange)
            {
                MediaChange = false;
                return;
            }

            if (Program.f1.WMP.currentMedia.isIdentical[Program.f1.WMP.currentPlaylist.Item[listBox.SelectedIndex]])
            {
                return;
            }
            //Program.Form1.WMP.currentPlaylist.Item
            //MessageBox.Show(listBox.SelectedIndex.ToString());
            //Program.f1.WMP.Ctlcontrols.currentItem = Program.f1.WMP.currentPlaylist.get_Item(listBox.SelectedIndex);

            //Program.f1.WMP.currentPlaylist.Item[listBox.SelectedIndex].
            //Program.f1.Text = Program.f1.WMP.currentPlaylist.Item[listBox.SelectedIndex].name.ToString();
            //Program.f1.WMP.Ctlcontrols.currentItem = Program.f1.WMP.currentPlaylist.Item[listBox.SelectedIndex];
            //Program.f1.WMP.Ctlcontrols.playItem(Program.f1.WMP.Ctlcontrols.currentItem);
            //Program.f1.WMP.Ctlcontrols.playItem(Program.f1.WMP.currentPlaylist.Item[listBox.SelectedIndex]);
            Program.f1.LoadNote(listBox.Items[listBox.SelectedIndex].ToString());
            Program.f1.WMP.Ctlcontrols.playItem(Program.f1.WMP.currentPlaylist.Item[listBox.Items.IndexOf(listBox.SelectedItem)]);
        }
    }
}
