/// Author: Feng-Hao, Yeh
/// Contact: 
/// - Recommended: zzz3x2c1@gmail.com
/// - Alternate: yeh.feng.hao.110@gmail.com
/// - Work: yeh.feng.hao@try-n-go.com

using System;
using System.Collections.Generic;
using static Apriori_Algorithm.Apriori;

namespace Apriori_Algorithm
{
    public class Apriori
    {
        public struct Apriori_Result
        {
            public Item_Group A;
            public Item_Group B;
            public double Confidence;
        }
        private static List<Item_Group> General_Data_By_Elements(List<Item_Group> target_list, List<string> Items, Item_Group current, int sample_times, int offset = 0)
        {
            if (sample_times == 0)
                target_list.Add(current);
            else
            {
                int size = Items.Count - sample_times + 1;
                for (int i = offset; i < size; i++)
                {
                    Item_Group clone = current.Clone();
                    clone.Add(Items[i]);
                    General_Data_By_Elements(target_list, Items, clone, sample_times - 1, i + 1);
                }
            }
            return target_list;
        }

        public static Dictionary<int, List<Item_Group>> Analysis_Tree(List<Group_Container> Datas, double Minimum_Support, out int Maximum_Elements_Count)
        {
            if (Minimum_Support > 1)
                Minimum_Support = 1;
            if (Minimum_Support == 0)
                Minimum_Support = 0;
            int min_support = (int)(Datas.Count * Minimum_Support);
            int sample_times = 0;

            List<string> items = new List<string>();
            foreach (Group_Container data in Datas)
                foreach (Item_Group group in data.Groups)
                    foreach (string item in group.Items)
                        if (!items.Contains(item))
                            items.Add(item);

            Dictionary<int, List<Item_Group>> All_Groups = new Dictionary<int, List<Item_Group>>();
            while (true)
            {
                List<Item_Group> new_group = General_Data_By_Elements(new List<Item_Group>(), items, new Item_Group(), ++sample_times);
                for (int i = 0; i < new_group.Count; i++)
                {
                    Item_Group group = new_group[i];
                    foreach (Group_Container data in Datas)
                        if (data.Contains_Group(group))
                            group.Support++;
                    if (group.Support < min_support)
                        new_group.RemoveAt(i--);
                }
                if (new_group.Count == 0)
                    break;
                All_Groups.Add(sample_times, new_group);
            }
            Maximum_Elements_Count = sample_times - 1;
            return All_Groups;
        }
        public static List<Apriori_Result> Analysis_Result(Dictionary<int, List<Item_Group>> Tree, int Maximum_Elements_Count)
        {
            List<Apriori_Result> result = new List<Apriori_Result>();
            int count = Tree[Maximum_Elements_Count].Count;
            for (int i = 0; i < count; i++)
            {
                Item_Group a = Tree[Maximum_Elements_Count][i];
                for (int j = 1; j <= Maximum_Elements_Count; j++)
                    for (int k = 0; k < Tree[j].Count; k++)
                    {
                        if (j == Maximum_Elements_Count)
                            if (k == i)
                                continue;
                        Item_Group b = Tree[j][k];
                        if (a.Contains_Group(b))
                        {
                            Apriori_Result r = new Apriori_Result();
                            r.B = a - b;
                            r.A = b;
                            r.Confidence = a.Support / (double)b.Support;
                            result.Add(r);
                        }
                    }
            }
            if (Maximum_Elements_Count > 1)
                result.AddRange(Analysis_Result(Tree, Maximum_Elements_Count - 1));
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Enter the number of association item groups: ");
            int Count = int.Parse(Console.ReadLine());
            List<Group_Container> Data_List = new List<Group_Container>();
            for (int i = 0; i < Count; i++)
            {
                Console.Write("Input items (separated by space): ");
                Group_Container d = new Group_Container();
                d.Add_Group(new Item_Group(Console.ReadLine()));
                Data_List.Add(d);
            }

            Console.Write("Minimum Support (Percentage): ");
            double min_sup = double.Parse(Console.ReadLine());
            Console.Write("Minimum confidence (Percentage): ");
            double min_con = double.Parse(Console.ReadLine());
            int Maximum_Elements_Count = 0;
            Dictionary<int, List<Item_Group>> tree = Apriori.Analysis_Tree(Data_List, min_sup, out Maximum_Elements_Count);
            List<Apriori_Result> result = Apriori.Analysis_Result(tree, Maximum_Elements_Count);
            foreach (Apriori_Result r in result)
            {
                if (r.Confidence < min_con)
                    continue;
                Console.WriteLine("--------------------");
                Console.WriteLine("[" + r.A.ToString() + "] -> [" + r.B.ToString() + "]");
                Console.WriteLine("Confidence: " + Math.Round(r.Confidence, 3));
            }
            Console.Read();
        }
    }
}
