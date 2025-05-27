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
            var list = await _context.TaskLists.Include(l => l.Cards)
                .FirstOrDefaultAsync(l => l.Id == dto.TaskListId);

            if (list == null)
                throw new Exception("TaskList not found");

            var card = _mapper.Map<Card>(dto);
            card.SortOrder = list.Cards.Count + 1;

            await _cardRepository.AddAsync(card);
            await _cardRepository.SaveChangesAsync();

            return _mapper.Map<CardDto>(card);
        }

        public async Task<CardDto> MoveCardAsync(UpdateCardPositionDto dto)
        {
            var card = await _cardRepository.GetByIdAsync(dto.CardId);
            if (card == null)
                throw new Exception("Card not found");

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
            var cards = await _cardRepository.GetByTaskListIdAsync(taskListId);
            return _mapper.Map<List<CardDto>>(cards);
        }

        public async Task DeleteCardAsync(int id)
        {
            var card = await _cardRepository.GetByIdAsync(id);
            if (card == null)
                throw new Exception("Card not found");

            await _cardRepository.DeleteAsync(card);
            await _cardRepository.SaveChangesAsync();
        }

        public async Task<CardDto> UpdateCardAsync(UpdateCardDto dto)
        {
            var card = await _cardRepository.GetByIdAsync(dto.Id);
            if (card == null)
                throw new Exception("Card not found");

            card.Title = dto.Title;
            card.Description = dto.Description;
            card.Color = dto.Color;

            await _cardRepository.SaveChangesAsync();

            return _mapper.Map<CardDto>(card);
        }
    }
}
