using AutoMapper;
using KanbanBoard.Api.Dtos;
using KanbanBoard.Api.Entities;
using KanbanBoard.Api.Repositories;

namespace KanbanBoard.Api.Services
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IMapper _mapper;

        public BoardService(IBoardRepository boardRepository, IMapper mapper)
        {
            _boardRepository = boardRepository;
            _mapper = mapper;
        }

        public async Task<BoardDto> CreateBoardAsync(CreateBoardDto dto)
        {
            var board = _mapper.Map<Board>(dto);
            board.PublicId = Guid.NewGuid().ToString();
            board.CreatedAt = DateTime.UtcNow;

            board.TaskLists = new List<TaskList>
            {
                new TaskList { Name = "Backlog", SortOrder = 1 },
                new TaskList { Name = "To Do", SortOrder = 2 },
                new TaskList { Name = "In Progress", SortOrder = 3 },
                new TaskList { Name = "Done", SortOrder = 4 }
            };

            await _boardRepository.AddAsync(board);
            await _boardRepository.SaveChangesAsync();

            return _mapper.Map<BoardDto>(board);
        }

        public async Task<BoardDto> GetBoardByPublicIdAsync(string publicId)
        {
            var board = await _boardRepository.GetByPublicIdAsync(publicId);
            if (board == null)
                throw new Exception("Board not found");

            return _mapper.Map<BoardDto>(board);
        }
    }
}
