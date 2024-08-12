using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Xml.Linq;
using WebApi.Data;
using WebApi.Exceptions;
using WebApi.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApi.Controllers
{

    /// <summary>
    /// Represents tree node API
    /// </summary>
    [Tags("user.tree.node")]
    public class UserTreeNodeController : ControllerBase
    {
        private readonly ILogger<UserTreeNodeController> _logger;
        private readonly StoreContext _db;

        public UserTreeNodeController(ILogger<UserTreeNodeController> logger, StoreContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <remarks>
        /// Create a new node in your tree. You must to specify a parent node ID that belongs to your tree. A new node name must be unique across all siblings.
        /// </remarks>
        [HttpPost("/api.user.tree.node.create")]
        public IActionResult Create([BindRequired][FromQuery]string treeName, [BindRequired]long parentNodeId, [BindRequired]string nodeName)
        {
            var n = _db.Nodes.Where(x => x.Id == parentNodeId).FirstOrDefault();

            if (n == null)
                throw new SecureException($"Node with ID = {parentNodeId} was not found");

            ChildrenFiller.Fill(n, _db);

            var tree = _db.Nodes.Where(x => x.ParentId == -1 & x.Name == treeName).FirstOrDefault();

            if (tree == null)
                throw new SecureException("Requested node was found, but it doesn't belong your tree");

            if (n.Children.Count > 0)
            {
                var clone = n.Children.Where(x => x.Name.Equals(nodeName)).FirstOrDefault();

                if (clone != null)
                    throw new SecureException("Duplicate name");
            }
            
            var nNode = new Node
            {
                Children = null,
                Name = nodeName,
                ParentId = parentNodeId
            };

            _db.Nodes.Add(nNode);
            _db.SaveChanges();

            return Ok();
        }

        /// <remarks>
        /// Delete an existing node in your tree. You must specify a node ID that belongs your tree.
        /// </remarks>
        [HttpPost("/api.user.tree.node.delete")]
        public IActionResult Delete([BindRequired][FromQuery]string treeName, [BindRequired][FromQuery]long nodeId)
        {
            var n = _db.Nodes.Where(x => x.Id == nodeId)
                .Select(p => new Node
                {
                    Id = p.Id,
                    Name = p.Name,
                    ParentId = p.ParentId,
                    Children = _db.Nodes.Where(x => x.ParentId == p.Id).ToList()
                }).FirstOrDefault();

            if (n == null)
                throw new SecureException($"Node with ID = {nodeId} was not found");

            if (n.Children.Count > 0)
                throw new SecureException("You have to delete all children nodes first");

            _db.Nodes.Remove(n);
            _db.SaveChanges();

            return Ok();
        }

        /// <remarks>
        /// Rename an existing node in your tree. You must specify a node ID that belongs your tree. A new name of the node must be unique across all siblings.
        /// </remarks>
        [HttpPost("/api.user.tree.node.rename")]
        public IActionResult Rename([BindRequired]long nodeId, [BindRequired]string nodeName)
        {
            var n = _db.Nodes.Where(x => x.Id == nodeId).FirstOrDefault();

            if (n == null)
                throw new SecureException($"Node with ID = {nodeId} was not found");

            n.Name = nodeName;
            _db.SaveChanges();

            return Ok();
        }
    }
}
