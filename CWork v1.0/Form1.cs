using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace CWork_v1._0
{
    public partial class Form1 : Form
    {
        int capacity = 0;
        List<Item> items = new List<Item>();
        List<List<Item>> allLists = new List<List<Item>>();
        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowCount = 1;
            Column2.ValueType = typeof(string);
            Column3.ValueType = typeof(double);
            Column4.ValueType = typeof(double);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.RowCount = (int)numericUpDown1.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error occurred!");
                dataGridView1.Rows.Clear();
                numericUpDown1.Value = 1;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            capacity = (int)numericUpDown2.Value;
        }


        private void loadDataBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    ParseFile(openFileDialog1.FileName);                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void saveSlnBtn_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(saveFileDialog1.FileName);
                using (StreamWriter sw = fileInfo.CreateText())
                {
                    sw.WriteLine("-------------------------------------------");
                    sw.WriteLine("Source data: ");
                    sw.WriteLine(textBox1.Text);
                    sw.WriteLine("-------------------------------------------");
                    sw.WriteLine("The best set: ");
                    sw.WriteLine(textBox2.Text);
                    sw.WriteLine("-------------------------------------------");
                }    
            }
        }

        private void ParseFile(string file)
        {
            try
            {
                StreamReader sr = new StreamReader(file);
                string str;
                int linesCount = File.ReadAllLines(file).Length;
                dataGridView1.RowCount = linesCount;
                numericUpDown1.Value = linesCount;

                Regex rstring = new Regex(@"(^[A-z]*.*[A-z]+)(\s\d\.?\d*)(\s\d*\.?\d*)");
                Regex Iname = new Regex(@"^([A-z]*[A-z]+)", RegexOptions.Multiline);
                Regex Iweight = new Regex(@"\b\d*\.?\d\b*", RegexOptions.Multiline);
                Regex Iprice = new Regex(@"\b\d*\.?\d*$", RegexOptions.Multiline);

                int i = 0;
                while (!sr.EndOfStream)
                {
                    str = sr.ReadLine();
                    if (rstring.IsMatch(str))
                    {
                        if (Iname.IsMatch(str))
                        {
                            string nm = Iname.Match(str).Value;
                            dataGridView1[0, i].Value = nm;
                        }
                        if (Iweight.IsMatch(str))
                        {
                            double nw = Convert.ToDouble(Iweight.Match(str).Value);
                            dataGridView1[1, i].Value = Math.Abs(nw);
                        }
                        if (Iprice.IsMatch(str))
                        {
                            double np = Convert.ToDouble(Iprice.Match(str).Value);
                            dataGridView1[2, i].Value = Math.Abs(np);
                        }
                        i++;
                    }
                    else
                    {
                        sr.Close();
                        throw new FormatException();
                    }
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error occurred!");
            }
        }

        private void AddItems()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                items.Add(new Item(Convert.ToString(dataGridView1[0, i].Value), Convert.ToDouble(dataGridView1[1, i].Value), Convert.ToDouble(dataGridView1[2, i].Value)));
            }
        }

        private void ShowItems(List<Item> items)
        {
            int f = 1;
            foreach (var Item in items)
            {
                textBox1.Text += f + ". " + Item.name + " - " + Item.weight + " - " + Item.price + "\r\n";
                f++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            progressBar1.Value = 0;

            for (int i = dataGridView1.Rows.Count - 1; i > -1; i--)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                if (!row.IsNewRow && row.Cells[0].Value == null)
                {
                    dataGridView1.Rows.RemoveAt(i);
                }
            }

            AddItems();
            ShowItems(items);

            IRSort sort = new IRSort();
            items.Sort(sort);

            Backpack backpack = new Backpack(capacity);
            backpack.Start(items);
            
            if (checkBox1.Checked)
            {
                allLists = backpack.ReturnAllSets();
                progressBar1.Maximum = allLists.Count;
                foreach (List<Item> items in allLists)
                {
                    foreach (Item item2 in items)
                    {
                        textBox3.Text += $"{item2.name}    {item2.weight}   {item2.price} \r\n";
                        progressBar1.PerformStep();
                    }
                    textBox3.Text += $"Total price: {backpack.GetPrice(items)} \t Total Weight: {backpack.GetWeight(items)} \r\n";
                    textBox3.Text += "------------------------------------------------------------------------------------------------- \r\n";
                }
            }
            
            List<Item> resList = backpack.ShowBestCombination();
            progressBar1.Maximum = resList.Count;
            foreach (Item item in resList)
            {
                textBox2.Text += "Name: " + item.name + "\t" + "Weight: " + item.weight + "\t" + "Price: " + item.price + "\t" + "\r\n";
                progressBar1.PerformStep();
            }
            textBox2.Text += $"Total price: {backpack.GetPrice(resList)} \t Total Weight: {backpack.GetWeight(resList)} \r\n";
            tabControl1.SelectedTab = tabPage2;
            items.Clear();
            resList.Clear();
            allLists.Clear();
            backpack.Clear();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {          
            MessageBox.Show($"Error: {e.Exception.Message} at [{e.ColumnIndex + 1}, {e.RowIndex + 1}] cell.", "Error occurred!");         
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\tПрограмму разработал студент 1 курса ОмГТУ, ФИТиКС \r\n\tВоробьев Николай Романович\r\n\tГруппа ПИН-192\r\n\t2020", "About");
        }
    }
}