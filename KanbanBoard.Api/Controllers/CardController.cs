using AutoMapper;
using KanbanBoard.Api.Data;
using KanbanBoard.Api.Dtos;
using KanbanBoard.Api.Entities;
using KanbanBoard.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] CreateCardDto dto)
        {
            try
            {
                var createdCard = await _cardService.CreateCardAsync(dto);
                return Ok(createdCard);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("move")]
        public async Task<IActionResult> MoveCard([FromBody] UpdateCardPositionDto dto)
        {
            try
            {
                var updatedCard = await _cardService.MoveCardAsync(dto);
                return Ok(updatedCard);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list/{taskListId}")]
        public async Task<IActionResult> GetCardsByList(int taskListId)
        {
            var cards = await _cardService.GetCardsByListAsync(taskListId);
            return Ok(cards);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            try
            {
                await _cardService.DeleteCardAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCard([FromBody] UpdateCardDto dto)
        {
            try
            {
                var updatedCard = await _cardService.UpdateCardAsync(dto);
                return Ok(updatedCard);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}