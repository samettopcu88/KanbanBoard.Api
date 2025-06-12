using AutoMapper;
using KanbanBoard.Api.Dtos;
using KanbanBoard.Api.Entities;

namespace KanbanBoard.Api.Mappings
{
    // AutoMapper konfigürasyonu için kullanılan profil sınıfı
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Board, BoardDto>().ReverseMap();
            CreateMap<CreateBoardDto, Board>().ReverseMap(); // *
            CreateMap<CreateCardDto, Card>().ReverseMap(); // *
            CreateMap<Card, CardDto>().ReverseMap();
            CreateMap<TaskList, TaskListDto>().ReverseMap();
            CreateMap<UpdateCardDto, Card>().ReverseMap(); // *
        }
    }
}
