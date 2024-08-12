using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using WebApi.Data;
using WebApi.Extensions;

namespace WebApi.Controllers
{

    /// <summary>
    /// Represents entire tree API
    /// </summary>
    [Tags("user.tree")]
    public class UserTreeController : ControllerBase
    {
        private readonly ILogger<UserTreeController> _logger;
        private readonly StoreContext _db; 

        public UserTreeController(ILogger<UserTreeController> logger, StoreContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpPost("/api.user.tree.get")]
        public IActionResult GetOrCreateTree([BindRequired][FromQuery]string treeName)
        {
            var tree = _db.Nodes.Where(o => o.ParentId == -1 & o.Name.Equals(treeName)).FirstOrDefault();

            if (tree == null)
            {
                tree = new Node { Name = treeName, Id = 0, ParentId = -1, Children = null };
                _db.Nodes.Add(tree);
                _db.SaveChanges();
            }else
                ChildrenFiller.Fill(tree, _db);

            return StatusCode(200, tree);
        }
    }
}
