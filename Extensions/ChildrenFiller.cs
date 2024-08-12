using WebApi.Data;

namespace WebApi.Extensions
{
    public static class ChildrenFiller
    {
        public static void Fill(Node node, StoreContext db)
        {
            var children = db.Nodes.Where(f => f.ParentId == node.Id).ToList();
              
            if(children.Count > 0)
            {
                foreach (var c in children) 
                {
                    Fill(c, db);
                }
            }

            node.Children = children;
        }
    }
}
