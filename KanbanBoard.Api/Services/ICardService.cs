using KanbanBoard.Api.Dtos;

namespace KanbanBoard.Api.Services
{
    public interface ICardService
    {
        Task<CardDto> CreateCardAsync(CreateCardDto dto);
        Task<CardDto> MoveCardAsync(UpdateCardPositionDto dto);
        Task<List<CardDto>> GetCardsByListAsync(int taskListId);
        Task DeleteCardAsync(int id);
    }
}
