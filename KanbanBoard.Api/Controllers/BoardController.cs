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
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly IValidator<CreateBoardDto> _validator;

        // Dependency Injection ile Board servisi ve CreateBoardDto için validator alınır
        public BoardController(IBoardService boardService, IValidator<CreateBoardDto> validator)
        {
            _boardService = boardService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] CreateBoardDto dto)
        {
            // İstekle gelen DTO doğrulanır
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                // Eğer doğrulama başarısızsa hata mesajları alınır PropertyName'e göre gruplanır ve client'a döner. ProtertyName FluentValidation kütüphanesinden gelir
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
                // Board servisi kullanılarak yeni board oluşturulur
                var result = await _boardService.CreateBoardAsync(dto);
                return Ok(new { message = "Board başarıyla oluşturuldu.", data = result });
            }
            catch (Exception ex)
            {
                // Oluşan hatalar client'a BadRequest olarak döner
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{publicId}")]
        public async Task<IActionResult> GetBoardByPublicId(string publicId)
        {
            try
            {
                // Verilen publicId ile board verisi servis üzerinden alınır
                var result = await _boardService.GetBoardByPublicIdAsync(publicId);
                return Ok(new { message = "Board başarıyla alındı.", data = result });
            }
            catch (Exception ex)
            {
                // Eğer board bulunamazsa NotFound döner
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBoards()
        {
            try
            {
                // Tüm boardlar servis aracılığıyla getirilir
                var boards = await _boardService.GetAllBoardsAsync();
                return Ok(new { message = "Tüm boardlar getirildi.", data = boards });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{publicId}")]
        public async Task<IActionResult> DeleteBoard(string publicId)
        {
            try
            {
                // Verilen publicId'ye ait board servis ile silinir
                await _boardService.DeleteBoardAsync(publicId);
                return Ok(new { message = "Board başarıyla silindi." });
            }
            catch (Exception ex)
            {
                // Board bulunamazsa NotFound döner
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
