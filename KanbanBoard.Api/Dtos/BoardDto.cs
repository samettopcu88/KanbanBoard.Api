namespace KanbanBoard.Api.Dtos
{
    public class BoardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TaskListDto> TaskLists { get; set; }
    }

}
