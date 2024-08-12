using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;



namespace WebApi.Data;

public class StoreContext : DbContext
{
    public StoreContext()
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public DbSet<Node> Nodes { get; set; }
    public DbSet<JournalEvent> Events { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=apidb;Username=postgres;Password=123");
    }
}

