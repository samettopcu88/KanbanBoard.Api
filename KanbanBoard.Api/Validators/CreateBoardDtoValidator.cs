﻿using FluentValidation;
using KanbanBoard.Api.Dtos;

namespace KanbanBoard.Api.Validators
{
    // CreateBoardDto için doğrulama kurallarını tanımlayan FluentValidation sınıfı
    public class CreateBoardDtoValidator : AbstractValidator<CreateBoardDto>
    {
        public CreateBoardDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Board adı boş olamaz.")
                .MaximumLength(100).WithMessage("Board adı 100 karakterden uzun olamaz.");

            RuleFor(x => x.PublicId)
                .NotEmpty().WithMessage("PublicId alanı zorunludur.")
                .Matches("^[a-zA-Z0-9_-]{4,30}$") // Yalnızca harf, rakam, tire ve alt çizgi içerebilir; 4-30 karakter uzunluğunda olmalı
                .WithMessage("PublicId yalnızca harf, rakam, tire (-) ve alt çizgi (_) içerebilir, 4-30 karakter arası olmalıdır.");
        }
    }
}
