using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CWork_v1._0
{
    public partial class Form1 : Form
    {
        int n = 0;
        int capacity = 0;
        List<Item> items = new List<Item>();
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


        private void AddItems()
        {
            for (int i = 0; i < n; i++)
            {
                items.Add(new Item(Convert.ToString(dataGridView1[1, i].Value), Convert.ToDouble(dataGridView1[2, i].Value), Convert.ToDouble(dataGridView1[3, i].Value)));
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
            AddItems();
            ShowItems(items);

            Backpack backpack = new Backpack(capacity);
            backpack.GetAllCombinations(items);
            List<Item> resList = backpack.ShowBestCombination();

            foreach (Item item in resList)
            {
                textBox2.Text += "Name: " + item.name + " Weight: " + item.weight + " Price: " + item.price + "\r\n";
            }
        }
    }
}
