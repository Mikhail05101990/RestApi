using WebApi.Data;

namespace WebApi.DTOs
{
    public class JournalData
    {
        public int skip { get; set; } = 0;
        public int count { get; set; } = 100;
        public List<Journal> items { get; set; } = null;

    }
}
