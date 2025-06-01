using AutoMapper;
using KanbanBoard.Api.Data;
using KanbanBoard.Api.Dtos;
using KanbanBoard.Api.Entities;
using KanbanBoard.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CardService(ICardRepository cardRepository, AppDbContext context, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<CardDto> CreateCardAsync(CreateCardDto dto)
        {
            // Geçerli bir Board var mı kontrolü için
            var board = await _context.Boards
                .Include(b => b.TaskLists)
                .ThenInclude(tl => tl.Cards)
                .FirstOrDefaultAsync(b => b.PublicId == dto.BoardPublicId);

            if (board == null)
                throw new Exception("Belirtilen BoardPublicId ile eşleşen bir board bulunamadı");

            // Backlog listesinin varlığını kontrol için
            var backlogList = board.TaskLists.FirstOrDefault(tl => tl.Name == "Backlog");
            if (backlogList == null)
                throw new Exception("İlgili board'da Backlog listesi bulunamadı");

            var card = new Card
            {
                Title = dto.Title,
                Description = dto.Description,
                Color = dto.Color,
                TaskListId = backlogList.Id,
                SortOrder = backlogList.Cards.Count + 1
            };

            await _cardRepository.AddAsync(card);
            await _cardRepository.SaveChangesAsync();

            return _mapper.Map<CardDto>(card);
        }

        public async Task<CardDto> MoveCardAsync(UpdateCardPositionDto dto)
        {
            var card = await _cardRepository.GetByIdAsync(dto.CardId);
            if (card == null)
                throw new Exception("Card bulunamadı.");

            var targetListCards = await _context.Cards
                .Where(c => c.TaskListId == dto.TargetListId && c.Id != dto.CardId)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

            foreach (var c in targetListCards.Where(c => c.SortOrder >= dto.NewSortOrder))
            {
                c.SortOrder += 1;
            }

            card.TaskListId = dto.TargetListId;
            card.SortOrder = dto.NewSortOrder;

            await _cardRepository.SaveChangesAsync();
            return _mapper.Map<CardDto>(card);
        }

        public async Task<List<CardDto>> GetCardsByListAsync(int taskListId)
        {
            var taskList = await _context.TaskLists
                .Include(tl => tl.Cards)
                .FirstOrDefaultAsync(tl => tl.Id == taskListId);

            if (taskList == null)
                return new List<CardDto>();

            return _mapper.Map<List<CardDto>>(taskList.Cards);
        }

        public async Task DeleteCardAsync(int id)
        {
            var card = await _cardRepository.GetByIdAsync(id);
            if (card == null)
                throw new Exception("Card bulunamadı.");

            await _cardRepository.DeleteAsync(card);
            await _cardRepository.SaveChangesAsync();
        }

        public async Task<CardDto> UpdateCardAsync(UpdateCardDto dto)
        {
            var card = await _cardRepository.GetByIdAsync(dto.Id);
            if (card == null)
                throw new Exception("Card bulunamadı.");

            card.Title = dto.Title;
            card.Description = dto.Description;
            card.Color = dto.Color;

            await _cardRepository.SaveChangesAsync();

            return _mapper.Map<CardDto>(card);
        }
    }
}
