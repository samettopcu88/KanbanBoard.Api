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

        // Card işlemleri için servis katmanı dependency injection ile alınır
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

                // Başarılı olursa kart bilgisiyle birlikte 200 OK döner
                return Ok(new
                {
                    message = "Card başarıyla oluşturuldu.",
                    data = createdCard
                });
            }
            catch (Exception ex)
            {
                // Servis katmanından gelen hata client'a BadRequest olarak döner
                return BadRequest(new { message = ex.Message });
            }
        }

        // Belirli bir kartı başka bir task listesine taşır
        [HttpPut("move")]
        public async Task<IActionResult> MoveCard([FromBody] UpdateCardPositionDto dto)
        {
            try
            {
                var updatedCard = await _cardService.MoveCardAsync(dto);

                // Yeni konumuyla birlikte güncellenmiş kart DTO’su döner
                return Ok(updatedCard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Belirli bir task listesine ait kartları getirir.
        [HttpGet("list/{taskListId}")]
        public async Task<IActionResult> GetCardsByList(int taskListId)
        {
            try
            {
                var cards = await _cardService.GetCardsByListAsync(taskListId);

                // Liste boş ya da bulunamadı ise mesaj gönderir
                if (cards == null || cards.Count == 0)
                {
                    return Ok(new { message = "Task listesi boş ya da bulunamadı.", data = cards });
                }

                // Eğer listede card varsa
                return Ok(new { message = "Kartlar başarıyla getirildi.", data = cards });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            try
            {
                await _cardService.DeleteCardAsync(id);
                return Ok(new { message = "Card başarıyla silindi." });
            }
            catch (Exception ex)
            {
                // Kart bulunamazsa 404 NotFound döner
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCard([FromBody] UpdateCardDto dto)
        {
            try
            {
                var updatedCard = await _cardService.UpdateCardAsync(dto);
                return Ok(new
                {
                    message = "Card başarıyla güncellendi.",
                    data = updatedCard
                });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}