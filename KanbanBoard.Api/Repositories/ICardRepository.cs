using KanbanBoard.Api.Entities;

namespace KanbanBoard.Api.Repositories
{
    // Card entity'si ile ilgili veri erişim işlemlerini tanımlayan repository arayüzü
    public interface ICardRepository
    {
        Task<Card> GetByIdAsync(int id);
        Task<List<Card>> GetByTaskListIdAsync(int taskListId);
        Task AddAsync(Card card);
        Task DeleteAsync(Card card);
        Task SaveChangesAsync();
    }
}
