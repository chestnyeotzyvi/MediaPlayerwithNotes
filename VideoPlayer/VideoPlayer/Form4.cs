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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://vk.com/id628844571");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked.");
                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://t.me/chestnye_otzyvi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked.");
                MessageBox.Show(ex.Message);
            }
        }

    }
}
