﻿using KanbanBoard.Api.Dtos;

namespace KanbanBoard.Api.Services
{
    // Card işlemlerini tanımlayan servis arayüzü
    public interface ICardService
    {
        Task<CardDto> CreateCardAsync(CreateCardDto dto);
        Task<CardDto> MoveCardAsync(UpdateCardPositionDto dto);
        Task<List<CardDto>> GetCardsByListAsync(int taskListId);
        Task DeleteCardAsync(int id);

        Task<CardDto> UpdateCardAsync(UpdateCardDto dto);

    }
}
