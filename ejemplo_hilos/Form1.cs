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

namespace ejemplo_hilos
{
    public delegate void ProgressBar(bool which);

    public partial class Form1 : Form
    {
        private ThreadCount nail1, nail2;

        public Form1()
        {
            InitializeComponent();
            dataGridView1.Columns.Add("", "");
            dataGridView1.Columns.Add("", "");
            dataGridView2.Columns.Add("", "");
            dataGridView2.Columns.Add("", "");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "txt|*.txt";
            openDialog.ShowDialog();
            this.textBox1.Text = openDialog.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
                this.button4_Click(null, null);
                nail1 = new ThreadCount(File.ReadAllText(textBox1.Text), true);
                nail1.AccessData += AddGrid1;
                nail1.AccessBar += AddBar;
                progressBar1.Maximum = nail1.GetSize(true);
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Value = 0;
            }
            catch
            {
                MessageBox.Show("Archivo no especificado");
            }         
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (nail1 != null)
            {
                if (button3.Text.Equals("Pausar"))
                {
                    button3.Text = "Reanudar";
                    nail1.Pause();
                }

                else
                {
                    button3.Text = "Pausar";
                    nail1.Resume();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                nail1.Stop();
                button3.Text = "Pausar";
                nail1 = null;
            }
            catch { }         
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Rows.Clear();
                this.button7_Click(null, null);
                nail2 = new ThreadCount(File.ReadAllText(textBox1.Text), false);
                nail2.AccessData += AddGrid2;
                nail2.AccessBar += AddBar;
                progressBar2.Maximum = nail2.GetSize(false);
                progressBar2.Style = ProgressBarStyle.Continuous;
                progressBar2.Value = 0;
            }
            catch
            {
                MessageBox.Show("Archivo no especificado");
            }         
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (nail2 != null)
            {
                if (button6.Text.Equals("Pausar"))
                {
                    button6.Text = "Reanudar";
                    nail2.Pause();
                }

                else
                {
                    button6.Text = "Pausar";
                    nail2.Resume();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                nail2.Stop();
                button6.Text = "Pausar";
                nail2 = null;
            }
            catch { }
        }

        private void AddGrid1(List<String> match, List<int> count)
        {
            if (this.dataGridView1.InvokeRequired)
            {
                ShowData d = new ShowData(AddGrid1);
                this.Invoke(d, new object[] { match, count });
            }
            else
            {
                if (dataGridView1.RowCount != match.Count)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = match[match.Count - 1];
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = count[match.Count - 1];
                }
                for (int j = 0; j < match.Count; j++)
                    dataGridView1.Rows[j].Cells[1].Value = count[j];
            }
        }

        private void AddGrid2(List<String> match, List<int> count)
        {
            if (this.dataGridView2.InvokeRequired)
            {
                ShowData d = new ShowData(AddGrid2);
                this.Invoke(d, new object[] { match, count });
            }
            else
            {
                if (dataGridView2.RowCount != match.Count)
                {
                    dataGridView2.Rows.Add();
                    dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = match[match.Count - 1];
                    dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = count[match.Count - 1];
                }
                for (int j = 0; j < match.Count; j++)
                    dataGridView2.Rows[j].Cells[1].Value = count[j];
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.button4_Click(null, null);
                this.button7_Click(null, null);
            }
            catch
            {

            }
        }

        private void AddBar(bool which)
        {
            switch(which)
            {
                case true:
                    if (this.progressBar1.InvokeRequired)
                    {
                        ProgressBar d = new ProgressBar(AddBar);
                        this.Invoke(d, new object[] { which });
                    }
                    else progressBar1.Value++;
                    break;
                case false:
                    if (this.progressBar2.InvokeRequired)
                    {
                        ProgressBar d = new ProgressBar(AddBar);
                        this.Invoke(d, new object[] { which });
                    }
                    else progressBar2.Value++;
                    break;
            }
        }
    }
}
