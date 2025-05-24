using FluentValidation;
using KanbanBoard.Api.Dtos;

namespace KanbanBoard.Api.Validators
{
    public class CreateBoardDtoValidator : AbstractValidator<CreateBoardDto>
    {
        public CreateBoardDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Board adı boş olamaz.")
                .MaximumLength(100).WithMessage("Board adı 100 karakterden uzun olamaz.");
        }
    }
}
