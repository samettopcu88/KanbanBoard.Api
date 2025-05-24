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
    public class BoardController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BoardController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] CreateBoardDto dto)
        {
            var board = _mapper.Map<Board>(dto);
            board.PublicId = Guid.NewGuid().ToString();
            board.CreatedAt = DateTime.UtcNow;

            // Sabit 4 listeyi oluştur
            var defaultLists = new List<TaskList>
        {
            new TaskList { Name = "Backlog", SortOrder = 1 },
            new TaskList { Name = "To Do", SortOrder = 2 },
            new TaskList { Name = "In Progress", SortOrder = 3 },
            new TaskList { Name = "Done", SortOrder = 4 },
        };

            board.TaskLists = defaultLists;

            _context.Boards.Add(board);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<BoardDto>(board);
            return Ok(new { message = "Board başarıyla oluşturuldu.", data = result });
        }

        [HttpGet("{publicId}")]
        public async Task<IActionResult> GetBoardByPublicId(string publicId)
        {
            var board = await _context.Boards
                .Include(b => b.TaskLists)
                .ThenInclude(l => l.Cards)
                .FirstOrDefaultAsync(b => b.PublicId == publicId);

            if (board == null)
                return NotFound();


            var result = _mapper.Map<BoardDto>(board);
            return Ok(new { message = "Board başarıyla alındı.", data = result });


        }
    }
}
