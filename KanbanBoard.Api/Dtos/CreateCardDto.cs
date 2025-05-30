namespace KanbanBoard.Api.Dtos
{
    public class CreateCardDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string BoardPublicId { get; set; }
    }
}
