using KanbanBoard.Api.Dtos;

namespace KanbanBoard.Api.Services
{
    public interface IBoardService
    {
        Task<BoardDto> CreateBoardAsync(CreateBoardDto dto);
        Task<BoardDto> GetBoardByPublicIdAsync(string publicId);
        Task<List<BoardDto>> GetAllBoardsAsync();
        Task DeleteBoardAsync(string publicId);

    }
}
