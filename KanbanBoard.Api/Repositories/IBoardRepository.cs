using KanbanBoard.Api.Entities;

namespace KanbanBoard.Api.Repositories
{
    public interface IBoardRepository
    {
        Task<Board> GetByPublicIdAsync(string publicId);
        Task AddAsync(Board board);
        Task SaveChangesAsync();
    }
}
