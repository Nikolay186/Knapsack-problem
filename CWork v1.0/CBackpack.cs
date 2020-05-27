using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Collections;

namespace CWork_v1._0
{
    class Backpack
    {
        List<Item> IMax = new List<Item>();
        List<List<Item>> allSets;
        List<Item> set;
        List<Item> tSet = new List<Item>();

        private double mWeight { get; set; }
        private double mPrice = 0;
        private double cPrice = 0;

        public Backpack(int bCapacity)
        {
            mWeight = bCapacity;
        }

        public void Start(List<Item> items)
        {
            allSets = new List<List<Item>>();
            set = new List<Item>();
            GetAllCombinations(items, set);
        }

        public void GetAllCombinations(List<Item> src, List<Item> st)
        {
            bool full = true;
            foreach (Item item in src)
                if (NotOverflow(st, item))
                {
                    full = false;
                    List<Item> newSet = new List<Item>(st);
                    newSet.Add(item);
                    List<Item> newSrc = new List<Item>(src);
                    newSrc.Remove(item);
                    GetAllCombinations(newSrc, newSet);
                }

            if (full)
            {
                cPrice = GetPrice(tSet);
                if (GetPrice(st) > cPrice)
                {
                    tSet = st;
                    allSets.Add(st);
                    IsBest(st);
                }
            }
        }

        private bool NotOverflow(List<Item> items, Item item)
        {
            double currW = 0;
            foreach (Item it in items)
            {
                currW += it.weight;
            }
            if (currW + item.weight <= mWeight)
                return true;
            else
                return false;
        }

        private void IsBest(List<Item> its)
        {
            if (mPrice < GetPrice(its))
            {
                IMax = its;
                mPrice = GetPrice(its);
            }

        }

        public List<List<Item>> ReturnCurrentBestSet()
        {
            return allSets;
        }

        public double GetWeight(List<Item> items)
        {
            double currentWeight = 0;

            foreach (Item item in items)
            {
                currentWeight += item.weight;
            }
            return currentWeight;
        }

        public double GetPrice(List<Item> items)
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
