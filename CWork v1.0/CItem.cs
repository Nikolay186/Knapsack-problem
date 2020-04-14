namespace CWork_v1._0
{
    class Item
    {
        public string name { get; set; }

        public double weight { get; set; }

        public double price { get; set; }

        public Item(string Iname, double Iweight, double Iprice)
        {
            name = Iname;
            weight = Iweight;
            price = Iprice;
        }
    }
}
