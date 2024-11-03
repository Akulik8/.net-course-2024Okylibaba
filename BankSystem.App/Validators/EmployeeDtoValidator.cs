using BankSystem.App.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators
{
    public class EmployeeDtoValidator: AbstractValidator<EmployeeDto>
    {
        public EmployeeDtoValidator()
        {
            RuleFor(e => e.FullName)
                .NotEmpty()
                .WithMessage("Полное имя обязательно для заполнения.");

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

            RuleFor(e => e.Salary)
                .GreaterThan(0)
                .WithMessage("Зарплата должна быть положительным числом.");

            RuleFor(e => e.Position)
                .NotEmpty()
                .WithMessage("Должность обязательно для заполнения.");

            RuleFor(e => e.PhoneNumber)
                .NotEmpty()
                .MaximumLength(15)
                .WithMessage("Неверный формат номера телефона.");

            RuleFor(e => e.PasNumber)
                .NotEmpty()
                .WithMessage("Паспортные данные обязательны для заполнения.");

            RuleFor(e => e.Address)
                .NotEmpty()
                .WithMessage("Адрес обязателен для заполнения.");
        }
    }
}
