using KanbanBoard.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Data
{
    // EF Core DbContext sınıfı veritabanı işlemlerini yönetmek için
    public class AppDbContext:DbContext
    {
        // DbContext yapılandırması için 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Board, TaskList ve Card tabloları için DbSet'ler
        public DbSet<Board> Boards { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Board ile TaskList arasında bire-çok ilişki
            modelBuilder.Entity<Board>()
                .HasMany(b => b.TaskLists) // Bir board’un birçok task listesi olabilir
                .WithOne(l => l.Board) // Her task listenin bir board'u vardır
                .HasForeignKey(l => l.BoardId); // TaskList tablosunda BoardId yabancı anahtar

            // TaskList ile Card arasında bire-çok ilişki
            modelBuilder.Entity<TaskList>()
                .HasMany(l => l.Cards) // Bir task listede birçok kart olabilir
                .WithOne(c => c.TaskList) // Her kartın bir task listesi vardır
                .HasForeignKey(c => c.TaskListId); // Card tablosunda TaskListId yabancı anahtar
        }
    }
}
