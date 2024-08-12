using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data
{
    public class JournalEvent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EventId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Text { get; set; }
    }
}
