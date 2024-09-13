/// Author: Feng-Hao, Yeh
/// Contact: 
/// - Recommended: zzz3x2c1@gmail.com
/// - Alternate: yeh.feng.hao.110@gmail.com
/// - Work: yeh.feng.hao@try-n-go.com

using System.Collections.Generic;

namespace Apriori_Algorithm
{
    public class Item_Group
    {
        public static Item_Group operator -(Item_Group a, Item_Group b)
        {
            Item_Group A = a.Clone();
            for (int i = 0; i < b.Item_Count; i++)
                if (A.Items.Contains(b.Items[i]))
                    A.Items.Remove(b.Items[i]);
            return A;
        }
        public int Support = 0;
        public List<string> Items = new List<string>();
        public int Item_Count { get => Items.Count; }
        public Item_Group()
        {

        }
        public Item_Group(string Item)
        {
            string[] spl = Item.Split(' ');
            foreach (string item in spl)
                Add(item);
        }
        public Item_Group Clone()
        {
            Item_Group g = new Item_Group();
            g.Items.AddRange(Items);
            g.Support = Support;
            return g;
        }
        public void Add(string Input)
        {
            Items.Add(Input);
            Items.Sort();
        }
        public bool Contains_Group(Item_Group Group)
        {
            if (Group.Item_Count > Item_Count)
                return false;
            foreach (string Item in Group.Items)
                if (!Items.Contains(Item))
                    return false;
            return true;
        }

        public int Get_Items_Count()
        {
            return Items.Count;
        }
        public new string ToString()
        {
            string r = "";
            bool sp = false;
            foreach (string s in Items)
            {
                if (sp)
                    r += "^";
                r += s;
                sp = true;
            }
            return r.Trim();
        }
    }
}
