using FluentValidation;
using KanbanBoard.Api.Dtos;

namespace KanbanBoard.Api.Validators
{
    public class CreateCardDtoValidator : AbstractValidator<CreateCardDto>
    {
        public CreateCardDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Kart başlığı boş olamaz.")
                .MaximumLength(100).WithMessage("Kart başlığı 100 karakteri geçemez.");

            RuleFor(x => x.TaskListId)
                .GreaterThan(0).WithMessage("Geçerli bir TaskListId girilmelidir.");
        }
    }
}
