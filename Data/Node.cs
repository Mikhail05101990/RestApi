using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data
{
    public class Node
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } = 0;
        public long ParentId { get; set; } = -1;
        public required string Name { get; set; } = "NodeName";
        public ICollection<Node> Children { get; set; } = null;
    }
}
