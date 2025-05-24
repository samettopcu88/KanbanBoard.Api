using System.Diagnostics;

namespace KanbanBoard.Api.Entities
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PublicId { get; set; } // herkesin erişebileceği ID
        public DateTime CreatedAt { get; set; }

        public ICollection<TaskList> TaskLists { get; set; }
    }
}
