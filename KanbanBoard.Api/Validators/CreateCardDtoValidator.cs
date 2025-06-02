using FluentValidation;
using KanbanBoard.Api.Dtos;

namespace KanbanBoard.Api.Validators
{
    // CreateCardDto için doğrulama kurallarını tanımlayan FluentValidation sınıfı
    public class CreateCardDtoValidator : AbstractValidator<CreateCardDto>
    {
        public CreateCardDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Kart başlığı boş olamaz.")
                .MaximumLength(100).WithMessage("Kart başlığı 100 karakteri geçemez.");

            RuleFor(x => x.BoardPublicId)
                .NotEmpty().WithMessage("BoardPublicId boş olamaz.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");

            RuleFor(x => x.Color)
                .MaximumLength(20).WithMessage("Renk en fazla 20 karakter olabilir.");
        }
    }
}
