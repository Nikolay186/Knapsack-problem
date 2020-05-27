using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace CWork_v1._0
{
    public partial class Form1 : Form
    {
        int n = 0;
        int capacity = 0;
        List<Item> items = new List<Item>();
        List<List<Item>> currList = new List<List<Item>>();
        public Form1()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            n = (int)numericUpDown1.Value;
            dataGridView1.RowCount = n;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            capacity = (int)numericUpDown2.Value;
        }

        private void loadDataBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ParseFile(openFileDialog1.FileName);
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
            StreamReader sr = new StreamReader(file);
            string str;
            int linesCount = File.ReadAllLines(file).Length;
            dataGridView1.RowCount = linesCount;
            numericUpDown1.Value = linesCount;
            n = linesCount;

            Regex Iname = new Regex(@"^[A-z]+\b", RegexOptions.Multiline);
            Regex Iweight = new Regex(@"\b\d*\.?\d\b*", RegexOptions.Multiline);
            Regex Iprice = new Regex(@"\b\d*\.?\d*$", RegexOptions.Multiline);

            int i = 0;
            while (!sr.EndOfStream)
            {
                str = sr.ReadLine();

                if (Iname.IsMatch(str))
                {
                    dataGridView1[0, i].Value = Iname.Match(str).ToString();
                }
                if (Iweight.IsMatch(str))
                {
                    dataGridView1[1, i].Value = Iweight.Match(str).ToString();
                }
                if (Iprice.IsMatch(str))
                {
                    dataGridView1[2, i].Value = Iprice.Match(str).ToString();
                }
                i++;
            }          
        }

        private void AddItems()
        {
            for (int i = 0; i < n; i++)
            {
                items.Add(new Item(Convert.ToString(dataGridView1[0, i].Value), Convert.ToDouble(dataGridView1[1, i].Value), Convert.ToDouble(dataGridView1[2, i].Value)));
            }
        }

        private void ShowItems(List<Item> items)
        {
            int f = 1;
            foreach (var Item in items)
            {
                textBox1.Text += f + " - " + Item.name + " - " + Item.weight + " - " + Item.price + "\r\n";
                f++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            progressBar1.Value = 0;
           
            AddItems();
            ShowItems(items);

            IRSort sort = new IRSort();
            items.Sort(sort);

            Backpack backpack = new Backpack(capacity);
            backpack.Start(items);
            
            if (checkBox1.Checked)
            {
                currList = backpack.ReturnCurrentBestSet();
                progressBar1.Maximum = currList.Count;
                foreach (List<Item> items in currList)
                {
                    foreach (Item item2 in items)
                    {
                        textBox3.Text += $"{item2.name} \t {item2.weight} \t {item2.price} \r\n";
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
            currList.Clear();
        }
    }
}