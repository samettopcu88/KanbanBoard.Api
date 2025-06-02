using KanbanBoard.Api.Entities;

namespace KanbanBoard.Api.Repositories
{
    // Board entity'siyle ilgili veritabanı işlemlerini tanımlayan repository arayüzü
    public interface IBoardRepository
    {
        Task<Board> GetByPublicIdAsync(string publicId);
        Task AddAsync(Board board);
        Task SaveChangesAsync();
        Task<List<Board>> GetAllAsync();
        Task DeleteAsync(Board board);
    }
}
