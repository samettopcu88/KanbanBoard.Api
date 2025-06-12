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
            // Aynı PublicId'ye sahip bir board var mı kontrol ediliyor
            var existing = await _boardRepository.GetByPublicIdAsync(dto.PublicId); // Eğer böyle bir kayıt varsa o board nesnesini getirir ve existing içine atar
            if (existing != null)
                throw new Exception("Bu PublicId zaten kullanımda.");

            // DTO'dan Board entity'si oluşturuluyor
            var board = _mapper.Map<Board>(dto);
            // PublicId ve oluşturulma tarihi atanıyor
            board.PublicId = dto.PublicId;
            board.CreatedAt = DateTime.UtcNow;

            // Her yeni board 4 adet sabit listeyle birlikte oluşuyor
            board.TaskLists = new List<TaskList>
            {
                new TaskList { Name = "Backlog", SortOrder = 1 },
                new TaskList { Name = "To Do", SortOrder = 2 },
                new TaskList { Name = "In Progress", SortOrder = 3 },
                new TaskList { Name = "Done", SortOrder = 4 }
            };

            // Board veritabanına ekleniyor
            await _boardRepository.AddAsync(board);
            await _boardRepository.SaveChangesAsync();

            // Oluşturulan board DTO'ya dönüştürülerek client'a verilir
            return _mapper.Map<BoardDto>(board);
        }

        public async Task<BoardDto> GetBoardByPublicIdAsync(string publicId)
        {
            // PublicId ile board aranıyor
            var board = await _boardRepository.GetByPublicIdAsync(publicId);
            if (board == null)
                throw new Exception("Board bulunamadı.");

            return _mapper.Map<BoardDto>(board);
        }

        public async Task<List<BoardDto>> GetAllBoardsAsync()
        {
            // Tüm board'lar getiriliyor
            var boards = await _boardRepository.GetAllAsync();
            // Liste DTO'ya dönüştürülüyor
            return _mapper.Map<List<BoardDto>>(boards);
        }

        public async Task DeleteBoardAsync(string publicId)
        {
            // Silinecek board veritabanında aranıyor
            var board = await _boardRepository.GetByPublicIdAsync(publicId);
            if (board == null)
                throw new Exception("Board bulunamadı.");

            // Bulunan board siliniyor
            await _boardRepository.DeleteAsync(board);
            await _boardRepository.SaveChangesAsync();
        }

    }
}
