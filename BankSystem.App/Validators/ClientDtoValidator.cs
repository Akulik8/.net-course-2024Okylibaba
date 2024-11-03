using BankSystem.App.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators
{
    public class ClientDtoValidator : AbstractValidator<ClientDto>
    {
        public ClientDtoValidator()
        {
            RuleFor(c => c.FullName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Имя пользователя обязательно.");

            RuleFor(c => c.PasNumber)
                .NotNull()
                .NotEmpty()
                .WithMessage("Паспорт обязателен");

            RuleFor(c => c.Date)
                .NotNull()
                .NotEmpty()
                .WithMessage("Введите дату рождения")
                .Must(x => {
                    var today = DateOnly.FromDateTime(DateTime.Today);
                    var age = today.Year - x.Year;

                    if (today < x.AddYears(age))
                    {
                        age--;
                    }

                    return age >= 18;
                })
                .WithMessage("Вам меньше 18 лет");

            RuleFor(c => c.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .MaximumLength(15)
                .WithMessage("Введите корректный комер телефона");
        }
    }
}
