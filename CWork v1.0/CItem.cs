using System.Collections.Generic;
using System.Windows.Forms;

namespace CWork_v1._0
{
    class Item
    {
        public string name { get; set; }

        public double weight { get; set; }

        public double price { get; set; }

        public double ratio { get; set; }

        public Item(string Iname, double Iweight, double Iprice)
        {
            name = Iname;
            weight = Iweight;
            price = Iprice;
            ratio = price / weight;
        }
    }

    class IRSort : IComparer<Item>
    {
        public IRSort()
        {

        }
        public int Compare(Item a, Item b)
        {
            if (a.ratio < b.ratio)
                return 1;
            if (a.ratio > b.ratio)
                return -1;
            return 0;
        }
    }

    class IWSort : IComparer<Item>
    {
        public IWSort()
        {

        }

        public int Compare(Item a, Item b)
        {
            if (a.weight > b.weight)
                return 1;
            if (a.weight < b.weight)
                return -1;
            return 0;
        }
    }

    class IVSort : IComparer<Item>
    {
        public IVSort() 
        {
            
        }

        public int Compare(Item a, Item b)
        {
            if (a.price > b.price)
                return 1;
            if (a.price < b.price)
                return -1;
            return 0;
        }
    }
}
