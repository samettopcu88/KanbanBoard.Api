namespace KanbanBoard.Api.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public int SortOrder { get; set; }

        public int TaskListId { get; set; }
        public TaskList TaskList { get; set; }
    }

}
