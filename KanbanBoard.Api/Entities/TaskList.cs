﻿namespace KanbanBoard.Api.Entities
{
    public class TaskList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
