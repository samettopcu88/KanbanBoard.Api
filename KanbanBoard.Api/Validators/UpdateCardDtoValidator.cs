using FluentValidation;
using KanbanBoard.Api.Dtos;

namespace KanbanBoard.Api.Validators
{
    public class UpdateCardDtoValidator : AbstractValidator<UpdateCardDto>
    {
        public UpdateCardDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Geçerli bir kart ID'si girilmelidir.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Başlık boş bırakılamaz.")
                .MaximumLength(100)
                .WithMessage("Başlık en fazla 100 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Açıklama en fazla 500 karakter olabilir.");

            RuleFor(x => x.Color)
                .MaximumLength(20)
                .WithMessage("Renk en fazla 20 karakter olabilir.");
        }
    }
}
