namespace KanbanBoard.Api.Dtos
{
    public class UpdateCardPositionDto
    {
        public int CardId { get; set; }
        public int TargetListId { get; set; }
        public int NewSortOrder { get; set; } // Aynı listede sıralama değişimi için
    }

}
