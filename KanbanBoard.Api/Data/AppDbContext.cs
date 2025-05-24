using KanbanBoard.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Board> Boards { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
                .HasMany(b => b.TaskLists)
                .WithOne(l => l.Board)
                .HasForeignKey(l => l.BoardId);

            modelBuilder.Entity<TaskList>()
                .HasMany(l => l.Cards)
                .WithOne(c => c.TaskList)
                .HasForeignKey(c => c.TaskListId);
        }
    }
}
