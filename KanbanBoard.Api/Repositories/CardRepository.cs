using KanbanBoard.Api.Data;
using KanbanBoard.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Repositories
{
    // Card veritabanı işlemlerini yöneten repository sınıfı
    public class CardRepository : ICardRepository
    {
        private readonly AppDbContext _context;

        public CardRepository(AppDbContext context)
        {
            _context = context;
        }

        // Id'ye göre tek bir kart getirir, bulunamazsa null döner fakat bu duruma karşı hata yönetimini controller sınıfında tanımladık
        public async Task<Card> GetByIdAsync(int id) =>
            await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);

        // Belirtilen TaskListId'ye ait kartları, SortOrder'a göre sıralı şekilde getirir
        public async Task<List<Card>> GetByTaskListIdAsync(int taskListId) =>
            await _context.Cards
                .Where(c => c.TaskListId == taskListId)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

        public async Task AddAsync(Card card) =>
            await _context.Cards.AddAsync(card);

        public async Task DeleteAsync(Card card) =>
            _context.Cards.Remove(card);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
