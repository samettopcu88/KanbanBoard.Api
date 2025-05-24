using AutoMapper;
using KanbanBoard.Api.Data;
using KanbanBoard.Api.Dtos;
using KanbanBoard.Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CardController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] CreateCardDto dto)
        {
            var list = await _context.TaskLists
                .Include(l => l.Cards)
                .FirstOrDefaultAsync(l => l.Id == dto.TaskListId);

            if (list == null)
                return NotFound("TaskList not found");

            var card = _mapper.Map<Card>(dto);
            card.SortOrder = list.Cards.Count + 1;

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return Ok(card);
        }

        [HttpPut("move")]
        public async Task<IActionResult> MoveCard([FromBody] UpdateCardPositionDto dto)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == dto.CardId);
            if (card == null)
                return NotFound("Card not found");

            card.TaskListId = dto.TargetListId;
            card.SortOrder = dto.NewSortOrder;

            await _context.SaveChangesAsync();
            return Ok(card);
        }

        [HttpGet("list/{taskListId}")]
        public async Task<IActionResult> GetCardsByList(int taskListId)
        {
            var cards = await _context.Cards
                .Where(c => c.TaskListId == taskListId)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

            var cardDtos = _mapper.Map<List<CardDto>>(cards);
            return Ok(cardDtos);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (card == null)
                return NotFound("Card not found");

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
