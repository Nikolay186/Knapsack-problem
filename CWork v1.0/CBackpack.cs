﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWork_v1._0
{
    class Backpack
    {
        List<Item> IMax = new List<Item>();

        private double mWeight;

        private double mPrice;

        public Backpack(int bCapacity)
        {
            mWeight = bCapacity;
        }

        public void GetAllCombinations(List<Item> items)
        {
            if (items.Count > 0)
                IsBest(items);

            for (int i = 0; i < items.Count; i++)
            {
                List<Item> set = new List<Item>(items);
                set.RemoveAt(i);
                GetAllCombinations(set);
            }
        }

        private void IsBest(List<Item> items)
        {
            if (GetWeight(items) <= mWeight && GetPrice(items) > mPrice)
            {
                IMax = items;
                mPrice = GetPrice(items);
            }

        }

        private double GetWeight(List<Item> items)
        {
            double currentWeight = 0;

            foreach (Item item in items)
            {
                currentWeight += item.weight;
            }
            return currentWeight;
        }

        private double GetPrice(List<Item> items)
        {
            double currentPrice = 0;

            foreach (Item item in items)
            {
                currentPrice += item.price;
            }
            return currentPrice;
        }

        public List<Item> ShowBestCombination()
        {
            return IMax;
        }
    }
}
