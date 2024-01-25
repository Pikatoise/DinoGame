using Microsoft.EntityFrameworkCore;

namespace DinoGame.Models;

public partial class GamedbContext: DbContext
{
    public GamedbContext(DbContextOptions<GamedbContext> options) : base(options)
    {
    }

    public virtual DbSet<Player> Players { get; set; }

    public bool DbStatus => Database.CanConnect();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("player");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
