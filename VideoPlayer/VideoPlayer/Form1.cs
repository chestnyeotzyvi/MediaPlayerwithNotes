using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMPLib;

namespace VideoPlayer
{
    public partial class Form1 : Form
    {
        string file = "";
        string pathFolder = "";
        bool modeFolder;
        bool filterNotes;
        Form2 f2 = new Form2();
        Form3 f3 = new Form3();
        Form4 f4 = new Form4();

        public Form1()
        {
            Program.f1 = this; 
            InitializeComponent();
            this.KeyPreview = true;
            WMP.settings.rate = 1.00;
            openFileDialog1.FileName = "";
            modeFolder = false;
            filterNotes = false;            
        }

        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            return;

            filterNotes = false;
            modeFolder = false;

            file = openFileDialog1.FileName;
            this.Text = file;

            WMP.URL = file;
            //WMP.currentPlaylist.name = " ";

            WMP.settings.rate = 1.00;

            string text_file = file.Substring(0, file.LastIndexOf(".")) + ".txt";
            //this.Text = text_file;

            dataGridView1.Rows.Clear();

            if (System.IO.File.Exists(text_file))
            {
                StreamReader reader = new StreamReader(text_file, Encoding.UTF8);
                while (!reader.EndOfStream)
                {
                    string[] slices = reader.ReadLine().Split('\t');

                    //if (slices.Length != dataGridView1.ColumnCount)
                    //  throw new Exception("Неправильная структура файла.");

                    int i = dataGridView1.Rows.Add();

                    for (int j = 0; j < dataGridView1.ColumnCount; ++j)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = slices[j];
                    }
                }
                reader.Close();                
            }
        }



        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            string temp;
            double speed;

            if (e.KeyCode == Keys.PageUp)
            {
                WMP.Ctlenabled = false;
                e.Handled = true;
                if (panel1.Visible)
                {
                     panel1.Visible = false;
                     return;
                }

                if (WMP.playState == WMPPlayState.wmppsPlaying)
                {
                    panel1.Location = new Point(WMP.Location.X, WMP.Location.Y);
                    panel1.Width = WMP.Width;
                    panel1.Height = WMP.Height - 45;
                    panel1.Visible = true;
                }

            }


            if (e.KeyCode == Keys.PageDown)
            {
              WMP.Ctlenabled = false;
              e.Handled = true;
              if (WMP.playState == WMPPlayState.wmppsPaused)
              {
                  WMP.Ctlcontrols.play();
                  e.Handled = true;
                  return;
              }

              if (WMP.playState == WMPPlayState.wmppsPlaying)
              {
                  WMP.Ctlcontrols.pause();                  
                  return;
              }
            }

            if (e.KeyCode == Keys.Right)
            {
                WMP.Ctlenabled = false;
                e.Handled = true;
                if (WMP.playState == WMPPlayState.wmppsPlaying)
                {
                 //MessageBox.Show("Right");
                 temp = WMP.settings.rate.ToString();
                 if (WMP.settings.rate<1.9)
                 {
                    WMP.settings.rate = WMP.settings.rate + 0.10;
                    WMP.Ctlcontrols.pause();
                    WMP.Ctlcontrols.play();
                    temp = WMP.settings.rate.ToString();
                    speed = Math.Round(WMP.settings.rate, 3);
                    if (!(temp == "1"))
                    {
                        temp = temp.Substring(0, 4);
                        this.Text = "Скорость воспроизведения: " + speed.ToString();
                    }
                    else
                        this.Text = "";
                 }
                }
                return;
            }

            if (e.KeyCode == Keys.Left)
            {
                WMP.Ctlenabled = false;
                e.Handled = true;
                if (WMP.playState == WMPPlayState.wmppsPlaying)
                {
                    //MessageBox.Show("Left");
                    WMP.settings.rate = WMP.settings.rate - 0.10;
                    WMP.Ctlcontrols.pause();
                    WMP.Ctlcontrols.play();
                    temp = WMP.settings.rate.ToString();
                    //MessageBox.Show("Затем: " + WMP.settings.rate.ToString());
                    speed = Math.Round(WMP.settings.rate, 3);
                    if (!(temp == "1"))
                    {
                        temp = temp.Substring(0, 4);
                        this.Text = "Скорость воспроизведения: " + speed.ToString();
                    }
                    else
                        this.Text = "";
                }
                
                return;
            }

            if (e.KeyCode == Keys.Insert)
            {
               if ((WMP.playState == WMPPlayState.wmppsUndefined) || (WMP.playState == WMPPlayState.wmppsStopped))
                    return;
               WMP.Ctlenabled = false;
               e.Handled = true;
               AddStr();
            }

            if (e.KeyCode == Keys.Delete)
            {
                if ((WMP.playState == WMPPlayState.wmppsUndefined) || (WMP.playState == WMPPlayState.wmppsStopped))
                    return;
                WMP.Ctlenabled = false;
                e.Handled = true;
                DeleteStr();
            }


            if (e.KeyCode == Keys.Up)
            {
                WMP.settings.volume = WMP.settings.volume + 5; 
            }

            if (e.KeyCode == Keys.Down)
            {
                WMP.settings.volume = WMP.settings.volume - 5;
            }


        }//KeyDown


        private void AddStr()
        {
            if ((WMP.playState == WMPPlayState.wmppsUndefined) || (WMP.playState == WMPPlayState.wmppsStopped))
                return;

            if (WMP.playState == WMPPlayState.wmppsPlaying)
            {
                WMP.Ctlcontrols.pause();
                dataGridView1.Focus();
            }

            string temp_str = WMP.Ctlcontrols.currentPositionString;
            if (temp_str.Length == 5) temp_str = "00:" + temp_str;

            dataGridView1.Rows.Add();
            dataGridView1["Time", dataGridView1.Rows.Count - 1].Value = temp_str;
            dataGridView1.CurrentCell = dataGridView1["Note", dataGridView1.Rows.Count - 1];
        }

        private void DeleteStr()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                this.Text = "Заметка отсутствует";
                return;
            }

            int ind = dataGridView1.SelectedCells[0].RowIndex;
            dataGridView1.Rows.RemoveAt(ind);
        }

        private void Save()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                this.Text = "Заметки отсутствуют";
                return;
            }
            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                if (dataGridView1.Rows[j].Cells[1].Value.ToString().Length > 221)
                {
                    MessageBox.Show("Заметка в строчке №" + (j + 1).ToString() + " больше 221 символов" + "\n" + "Сохранение заметок невозможно!");
                    dataGridView1.CurrentCell = dataGridView1["Note", j];
                    return;
                }
            }

            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);

            string text_file = file.Substring(0, file.LastIndexOf(".")) + ".txt";
            this.Text = text_file;

            FileStream fs = new FileStream(text_file, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fs);

            try
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    if ((!(dataGridView1.Rows[j].Cells[0].Value == null)) && (!(dataGridView1.Rows[j].Cells[1].Value == null)))
                    {
                        string temp_str = dataGridView1.Rows[j].Cells[0].Value.ToString();
                        if (temp_str.Length == 5) temp_str = "00:" + temp_str;
                        streamWriter.Write(temp_str + "\t");
                        streamWriter.Write(dataGridView1.Rows[j].Cells[1].Value);
                        streamWriter.WriteLine();
                    }
                }

                streamWriter.Close();
                fs.Close();
                MessageBox.Show("Заметки успешно сохранены");
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении заметок!");
                streamWriter.Close();
                fs.Close();
            }

        }

        private void SaveFolder()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                this.Text = "Заметки отсутствуют";
                return;
            }
            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                if (dataGridView1.Rows[j].Cells[1].Value.ToString().Length > 221)
                {
                    MessageBox.Show("Заметка в строчке №" + (j + 1).ToString() + " больше 221 символов" + "\n" + "Сохранение заметок невозможно!");
                    dataGridView1.CurrentCell = dataGridView1["Note", j];
                    return;
                }
            }

            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);

            string text_file = pathFolder + @"\"+ WMP.currentMedia.name.ToString() + ".txt";
            this.Text = text_file;

            FileStream fs = new FileStream(text_file, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fs);

            try
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    if ((!(dataGridView1.Rows[j].Cells[0].Value == null)) && (!(dataGridView1.Rows[j].Cells[1].Value == null)))
                    {
                        string temp_str = dataGridView1.Rows[j].Cells[0].Value.ToString();
                        if (temp_str.Length == 5) temp_str = "00:" + temp_str;
                        streamWriter.Write(temp_str + "\t");
                        streamWriter.Write(dataGridView1.Rows[j].Cells[1].Value);
                        streamWriter.WriteLine();
                    }
                }

                streamWriter.Close();
                fs.Close();
                MessageBox.Show("Заметки успешно сохранены");
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении заметок!");
                streamWriter.Close();
                fs.Close();
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddStr();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteStr();
        }

        private void WMP_PositionChange(object sender, AxWMPLib._WMPOCXEvents_PositionChangeEvent e)
        {
            WMP.Ctlenabled = false;
            dataGridView1.Focus();
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (modeFolder)
                SaveFolder();
            else
                Save();                         
        }

        private void WMP_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            WMP.Ctlenabled = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (e.ColumnIndex == 0) 
            {
                string str_position = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                DateTime DateObj = Convert.ToDateTime(str_position);
                this.Text = DateObj.TimeOfDay.TotalSeconds.ToString();
                WMP.Ctlcontrols.currentPosition = DateObj.TimeOfDay.TotalSeconds;
                WMP.Ctlcontrols.play();
            }
        }


        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filterNotes = false;
            dataGridView1.Rows.Clear();

            IWMPPlaylistArray plCollection = WMP.playlistCollection.getByName("myplaylist");

            if (plCollection.count > 0)
            {
                IWMPPlaylist pl = plCollection.Item(0);
                WMP.playlistCollection.remove(pl);
            }

            FolderBrowserDialog theFBD = new FolderBrowserDialog();
            //theFBD.SelectedPath = @"J:\Папка";
            if (theFBD.ShowDialog() == DialogResult.OK)
            {
                WMP.currentPlaylist.clear();

                WMPLib.IWMPPlaylist playlist = WMP.playlistCollection.newPlaylist("myplaylist");
                WMPLib.IWMPMedia media;

                modeFolder = true;
                f2.listBox.Items.Clear();
                foreach (string currentFile in System.IO.Directory.GetFiles(theFBD.SelectedPath))
                {
                    if ((currentFile.IndexOf(".xlsx") == -1) && (currentFile.IndexOf(".xls") == -1) && (currentFile.IndexOf(".pdf") == -1) && (currentFile.IndexOf(".7z") == -1)  &&
                        (currentFile.IndexOf(".html") == -1) && (currentFile.IndexOf(".htm") == -1) && (currentFile.IndexOf(".lnk") == -1) && (currentFile.IndexOf(".exe") == -1) &&
                        (currentFile.IndexOf(".webp") == -1) && (currentFile.IndexOf(".rar") == -1) && (currentFile.IndexOf(".iso") == -1) && (currentFile.IndexOf(".png") == -1) &&
                        (currentFile.IndexOf(".jpeg") == -1) && (currentFile.IndexOf(".zip") == -1) && (currentFile.IndexOf(".rtf") == -1) && (currentFile.IndexOf(".gif") == -1) &&
                        (currentFile.IndexOf(".djvu") == -1) && (currentFile.IndexOf(".txt") == -1) && (currentFile.IndexOf(".bmp") == -1) && (currentFile.IndexOf(".tif") == -1) &&
                        (currentFile.IndexOf(".docx") == -1) && (currentFile.IndexOf(".doc") == -1) && (currentFile.IndexOf(".url") == -1) && (currentFile.IndexOf(".jpg") == -1))
                    {
                        string temp = currentFile;
                        temp = temp.Substring(0, temp.LastIndexOf("."));
                        temp = temp.Substring(temp.LastIndexOf(@"\")+1);
                        media = WMP.newMedia(currentFile);
                        playlist.appendItem(media);
                        f2.listBox.Items.Add(temp);
                    }
                }


                if (f2.listBox.Items.Count > 0)
                {
                    WMP.settings.rate = 1.00;
                    f2.Show();
                    f2.StartPlayList = true;
                    f2.listBox.SetSelected(0, true);
                    pathFolder = theFBD.SelectedPath;
                    LoadNote(f2.listBox.Items[0].ToString());
                    WMP.currentPlaylist = playlist;
                    WMP.Ctlcontrols.play();
                }
            }

        }


        private void WMP_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            this.Text = WMP.currentMedia.name.ToString();
            if ((modeFolder) && (WMP.playState == WMPPlayState.wmppsPlaying))            
                if (!(WMP.currentMedia.isIdentical[WMP.currentPlaylist.Item[f2.listBox.SelectedIndex]]))
                {                    
                    for (int i = 0; i < f2.listBox.Items.Count; i++)                                            
                        if (WMP.currentMedia.name.ToString() == f2.listBox.Items[i].ToString())
                        {
                            f2.MediaChange = true;
                            LoadNote(f2.listBox.Items[i].ToString());
                            f2.listBox.SetSelected(i, true);
                            break;
                        }                                    
            }
        }

        public void LoadNote(string text_file)
        {
            text_file = pathFolder + @"\" + text_file + ".txt";

            dataGridView1.Rows.Clear();

            if (System.IO.File.Exists(text_file))
            {
                StreamReader reader = new StreamReader(text_file, Encoding.UTF8);
                while (!reader.EndOfStream)
                {
                    string[] slices = reader.ReadLine().Split('\t');

                    int i = dataGridView1.Rows.Add();

                    for (int j = 0; j < dataGridView1.ColumnCount; ++j)                    
                        dataGridView1.Rows[i].Cells[j].Value = slices[j];
                    
                }
                reader.Close();
            }
        }

        private void WMP_CurrentItemChange(object sender, AxWMPLib._WMPOCXEvents_CurrentItemChangeEvent e)
        {
            if (!(WMP.currentMedia.isIdentical[WMP.currentPlaylist.Item[f2.listBox.SelectedIndex]]))
            {
                for (int i = 0; i < f2.listBox.Items.Count; i++)
                    if (WMP.currentMedia.name.ToString() == f2.listBox.Items[i].ToString())
                    {
                        f2.MediaChange = true;
                        LoadNote(f2.listBox.Items[i].ToString());
                        f2.listBox.SetSelected(i, true);
                        WMP.Ctlcontrols.play();
                        break;
                    }    
            }
        }

        private void ConcatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(modeFolder)) return;
            if (f2.listBox.Items.Count > 0)
            {
                string temp;
                int ind = pathFolder.LastIndexOf(@"\");
                //MessageBox.Show(ind.ToString());
                //MessageBox.Show((pathFolder.Length - ind -1).ToString());

                string text_file = pathFolder + @"\" + pathFolder.Substring(pathFolder.LastIndexOf(@"\") + 1, pathFolder.Length - ind -1) + ".txt";
                this.Text = text_file;
                FileStream fs = new FileStream(text_file, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fs);

                for (int i = 0; i < f2.listBox.Items.Count; i++)
                {
                    text_file = pathFolder + @"\" + f2.listBox.Items[i].ToString() + ".txt"; 
                    //MessageBox.Show(text_file);
                    if (System.IO.File.Exists(text_file))
                    {
                        streamWriter.WriteLine(f2.listBox.Items[i].ToString() + "[" + WMP.currentPlaylist.Item[i].durationString + "]");
                        StreamReader reader = new StreamReader(text_file, Encoding.UTF8);
                        while (!reader.EndOfStream)
                        {
                            temp = reader.ReadLine();
                            streamWriter.WriteLine(temp);
                        }
                        reader.Close();
                        streamWriter.WriteLine();
                    }
                }

                streamWriter.Close();
                fs.Close();
            }
        }

        private void HotKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f3.ShowDialog();
            
        }

        private void AboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f4.ShowDialog();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if ((WMP.playState == WMPPlayState.wmppsPlaying) && (panel1.Visible))
            {
                panel1.Location = new Point(WMP.Location.X, WMP.Location.Y);
                panel1.Width = WMP.Width;
                panel1.Height = WMP.Height - 45;
            }
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filterNotes)
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    if (dataGridView1.Rows[j].Cells[1].Value.ToString().LastIndexOf("!") != 0)
                        dataGridView1.Rows[j].Visible = false;
                filterNotes = false;
            }
            else
           for (int j = 0; j < dataGridView1.Rows.Count; j++)
               if (dataGridView1.Rows[j].Cells[1].Value.ToString().LastIndexOf("!") != 0)               
                   dataGridView1.Rows[j].Visible = false;
        }


                   

    }//Form1
}//namespase
