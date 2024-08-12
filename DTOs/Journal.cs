using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class Journal
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
