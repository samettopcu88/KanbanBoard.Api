namespace KanbanBoard.Api.Dtos
{
    public class TaskListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }

        public List<CardDto> Cards { get; set; }
    }
}
