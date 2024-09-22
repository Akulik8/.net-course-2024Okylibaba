using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee employee1 = new Employee
            {
                Name = "Никита",
                Surname = "Окулибаба",
                PhoneNumber = 77960929,
                Passport = "IПР 123456789",
                Address = "г. Тирасполь, ул. Краснодонская",
                Date = new DateOnly(2003, 9, 26),
                Position = "Программист",
                Salary = 10000,
                DateStartWork = new DateOnly(2024, 9, 20)
            };

            UpdateContract(employee1);
            Console.WriteLine(employee1.Contract);

            Currency currency = new Currency
            { 
                Name = "Доллар США",
                ExchangeRate = 16.35m,
                Code = "USD"
            };

            UpdateCurrency(ref currency, "Евро", 19.2m, "EUR");
            Console.WriteLine($"Обновлённая валюта: {currency.Name}, Курс: {currency.ExchangeRate}, Код: {currency.Code}");

            static void UpdateContract(Employee employee)
            {
                employee.Contract = $"Контракт для {employee.Surname} {employee.Name}, Должность: {employee.Position}, Зарплата: {employee.Salary} руб., Дата начала работы: {employee.DateStartWork}";
            }

            static void UpdateCurrency(ref Currency currency, string newName, decimal newRate, string newCode)
            {
                currency = new Currency
                { 
                    Name= newName,
                    ExchangeRate= newRate,
                    Code= newCode
                };
            }

            Employee employee2 = new Employee
            {
                Name = "Игорь",
                Surname = "Афанасьев",
                PhoneNumber = 77867829,
                Passport = "IПР 123159789",
                Address = "г. Тирасполь, ул. Каховская",
                Date = new DateOnly(1997, 4, 6),
                Position = "Владелец",
                DateStartWork = new DateOnly(2024, 9, 19)
            };

            Employee employee3 = new Employee
            {
                Name = "Дима",
                Surname = "Топалов",
                PhoneNumber = 77511829,
                Passport = "IПР 321159789",
                Address = "г. Тирасполь, ул. Чапаева",
                Date = new DateOnly(1981, 2, 15),
                Position = "Владелец",
                DateStartWork = new DateOnly(2024, 9, 19)
            };

            Employee[] employees = [employee1, employee2, employee3];

            List<Employee> ownersList = new List<Employee>();
            
            foreach (var employee in employees)
            {
                if (employee.Position == "Владелец")
                {
                    ownersList.Add(employee);
                }
            }

            BankService bankService = new BankService();

            int SalaryOwners = bankService.CalculateSalaryOwners(100000, 50000, ownersList);
            Console.WriteLine($"Зарплата каждого владельца: {SalaryOwners}");

            Client client = new Client
            {
                Name = "Пётр",
                Surname = "Иванов",
                PhoneNumber = 77760606,
                Passport = "IПР 123321123",
                Address = "г. Тирасполь, ул. Юности",
                Date = new DateOnly(2010, 10, 16),
                AccountNumber = 123456,
                Balance = 412541,
            };
            Employee newEmployee = bankService.ConvertClientToEmployee(client, "Бухгалтер", 5000);
            UpdateContract(newEmployee);
            Console.WriteLine($"Новый сотрудник: {newEmployee.Surname} {newEmployee.Name}, Должность: {newEmployee.Position}");
        }    
    }
}
