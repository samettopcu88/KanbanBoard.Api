using AutoMapper;
using FluentValidation;
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
        private readonly IValidator<CreateCardDto> _validator;
        private readonly IValidator<UpdateCardDto> _updateValidator;

        // Card işlemleri için servis katmanı dependency injection ile alınır ve CreateCardDto için validator alınır
        public CardController(ICardService cardService, IValidator<CreateCardDto> validator, IValidator<UpdateCardDto> updateValidator)
        {
            _cardService = cardService;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] CreateCardDto dto)
        {
            // FluentValidation ile DTO doğrulama işlemi
            // Bu satır dto içindeki alanların FluentValidation kurallarına uygun olup olmadığını kontrol eder ve sonucu validationResult'a atar
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                // Hatalar field bazında gruplanıyor
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new
                {
                    message = "Doğrulama hatası oluştu.",
                    errors
                });
            }

            try
            {
                var createdCard = await _cardService.CreateCardAsync(dto);

                return Ok(new
                {
                    message = "Card başarıyla oluşturuldu.",
                    data = createdCard
                });
            }
            catch (Exception ex)
            {
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
            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new
                {
                    message = "Doğrulama hatası oluştu.",
                    errors
                });
            }

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