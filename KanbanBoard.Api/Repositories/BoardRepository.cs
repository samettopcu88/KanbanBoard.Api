using KanbanBoard.Api.Data;
using KanbanBoard.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Repositories
{
    // Board veritabanı işlemlerini yöneten repository sınıfı
    public class BoardRepository : IBoardRepository
    {
        private readonly AppDbContext _context;

        public BoardRepository(AppDbContext context)
        {
            _context = context;
        }

        // Verilen publicId ile eşleşen board'u, ilişkili TaskList ve Card'larıyla birlikte getirir
        public async Task<Board> GetByPublicIdAsync(string publicId)
        {
            return await _context.Boards
                .Include(b => b.TaskLists)
                .ThenInclude(tl => tl.Cards)
                .FirstAsync(b => b.PublicId == publicId);
        }

        public async Task AddAsync(Board board)
        {
            await _context.Boards.AddAsync(board);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Tüm boardları, ilişkili TaskList ve Card'larıyla birlikte liste halinde getirir
        public async Task<List<Board>> GetAllAsync()
        {
            return await _context.Boards
                .Include(b => b.TaskLists)
                .ThenInclude(t => t.Cards)
                .ToListAsync();
        }

        public async Task DeleteAsync(Board board) =>
            _context.Boards.Remove(board);
    }
}
