/// Author: Feng-Hao, Yeh
/// Contact: 
/// - Recommended: zzz3x2c1@gmail.com
/// - Alternate: yeh.feng.hao.110@gmail.com
/// - Work: yeh.feng.hao@try-n-go.com

using System.Collections.Generic;

namespace Apriori_Algorithm
{
    public class Group_Container
    {
        public List<Item_Group> Groups = new List<Item_Group>();
        public void Add_Group(Item_Group g) => Groups.Add(g);
        public int Get_Groups_Count() => Groups.Count;
        public bool Contains_Group(Item_Group Group)
        {
            foreach (Item_Group group in Groups)
                if (!group.Contains_Group(Group))
                    return false;
            return true;
        }
    }
}
