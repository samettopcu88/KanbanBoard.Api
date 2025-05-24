using KanbanBoard.Api.Data;
using KanbanBoard.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly AppDbContext _context;

        public CardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Card> GetByIdAsync(int id) =>
            await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);

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
