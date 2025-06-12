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
        private readonly IMapper _mapper; // Nesne dönüşümleri için AutoMapper

        // Constructor yapısı bağımlılıkları constructor üzerinden alırız (Dependency Injection)
        public CardService(ICardRepository cardRepository, AppDbContext context, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<CardDto> CreateCardAsync(CreateCardDto dto)
        {
            // Geçerli bir Board var mı kontrolü için yapıyoruz ve eager loading ile task list'leri ve card'ları getiriyoruz
            var board = await _context.Boards
                .Include(b => b.TaskLists) // Board içindeki TaskList'ler dahil ediliyor
                .ThenInclude(tl => tl.Cards) // Her bir TaskList'in içindeki card'lar da dahil ediliyor
                .FirstOrDefaultAsync(b => b.PublicId == dto.BoardPublicId);

            if (board == null)
                throw new Exception("Belirtilen BoardPublicId ile eşleşen bir board bulunamadı");

            // Backlog listesinin varlığını kontrol için
            var backlogList = board.TaskLists.FirstOrDefault(tl => tl.Name == "Backlog");
            if (backlogList == null)
                throw new Exception("İlgili board'da Backlog listesi bulunamadı");

            // Backlog listesi içindeki en yüksek SortOrder değerini buluyoruz
            // Hiç kart yoksa 0 kabul ediyoruz ve yeni kart 1. sırada olacak şekilde ekleniyor
            var maxSortOrder = await _context.Cards                           // burası yeni
                .Where(c => c.TaskListId == backlogList.Id)                   // burası yeni
                .MaxAsync(c => (int?)c.SortOrder) ?? 0;                        // burası yeni

            // Yeni kart nesnesi oluşturuluyor ve Backlog'a atanıyor
            var card = new Card
            {
                Title = dto.Title,
                Description = dto.Description,
                Color = dto.Color,
                TaskListId = backlogList.Id,
                SortOrder = maxSortOrder + 1 // Yeni kart en sona ekleniyor   // burası yeni

                
                //SortOrder = backlogList.Cards.Count + 1 (kart silindiğin durumda hata verebilir diye kaldırdım)
            };

            await _cardRepository.AddAsync(card);
            await _cardRepository.SaveChangesAsync();

            return _mapper.Map<CardDto>(card); // Entity to DTO dönüşümü
        }

        public async Task<CardDto> MoveCardAsync(UpdateCardPositionDto dto)
        {
            // Taşınacak kart bulunuyor
            var card = await _cardRepository.GetByIdAsync(dto.CardId);
            if (card == null)
                throw new Exception("Card bulunamadı.");

            // Diğer kartlar çekiliyor (taşınan hariç)
            // Taşınacak kartın pozisyonunu oluştururken  diğer kartların sırasını kaydırmak için kullanılıcak
            var targetListCards = await _context.Cards
                .Where(c => c.TaskListId == dto.TargetListId && c.Id != dto.CardId)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

            // Yeni sıraya denk gelen ve sonrası tüm kartların sırası 1 kaydırılıyor
            // Yani Hedef Task List'te bulunan ve yeni sıradan (NewSortOrder) daha büyük veya eşit sıradaki tüm kartları al ve her birinin sırasını bir artır
            foreach (var c in targetListCards.Where(c => c.SortOrder >= dto.NewSortOrder))
            {
                c.SortOrder += 1;
            }

            // Kartın yeni listeye ve sırasına taşınması işlemleri sadece 2 işlem olduğu için mapping yapmadım manuel çevirdim
            card.TaskListId = dto.TargetListId;
            card.SortOrder = dto.NewSortOrder;

            await _cardRepository.SaveChangesAsync();
            return _mapper.Map<CardDto>(card);
        }

        public async Task<List<CardDto>> GetCardsByListAsync(int taskListId)
        {
            // İlgili task list ve içindeki kartlar eager loading ile alınıyor
            var taskList = await _context.TaskLists
                .Include(tl => tl.Cards)
                .FirstOrDefaultAsync(tl => tl.Id == taskListId);

            if (taskList == null)
                return new List<CardDto>();

            return _mapper.Map<List<CardDto>>(taskList.Cards);
        }

        public async Task DeleteCardAsync(int id)
        {
            // Silinecek card id'ye göre bulunuyor
            var card = await _cardRepository.GetByIdAsync(id);
            if (card == null)
                throw new Exception("Card bulunamadı.");

            // Bulunan card siliniyor
            await _cardRepository.DeleteAsync(card);
            await _cardRepository.SaveChangesAsync();
        }

        public async Task<CardDto> UpdateCardAsync(UpdateCardDto dto)
        {
            var card = await _cardRepository.GetByIdAsync(dto.Id);
            if (card == null)
                throw new Exception("Card bulunamadı.");

            // DTO'daki verileri var olan card entity'sine günceller
            _mapper.Map(dto, card);

            await _cardRepository.SaveChangesAsync();

            // Güncellenen kartı DTO olarak döndür
            return _mapper.Map<CardDto>(card);
        }
    }
}
