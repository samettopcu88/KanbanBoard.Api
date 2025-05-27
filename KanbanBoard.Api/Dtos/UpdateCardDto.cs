namespace KanbanBoard.Api.Dtos
{
    public class UpdateCardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}
