using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace Practice
{
    class Program
    {
        static void Main(string[] args)
        {
               /*В папку “Domain” добавить новый проект по шаблону “Библиотека
               классов”, с именем “BankSystem.Domain”, в котором добавляем папку
               “Models”. В папке “Models”, спроектировать и реализовать свои типы
               “Person” (человек), и дочерние типы “Employee” (сотрудник), “Client”
               (клиент), структуру “Currency” (валюта).
               В папку “Tools” добавить проект (шаблон консольное приложение)
               “Practice”, в рамках которого реализуем методы:
               а) метод, обновляющий контракт сотрудника. Принимает на вход
               сотрудника и создает контракт (свойство класса “Employee”, строка) на
               основе его данных. Результат присваивается обратно в тело сотрудника,
               свойству “Contract”;
               б) метод обновляющий сущность валюты. (Метод принимает на вход
               экземпляр структуры “Currency”, меняет значение ее свойств); */
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

            Currency currency = new Currency ("Доллар США" ,16.35m, "USD");
            UpdateCurrency(ref currency, "Евро", 19.2m, "EUR");
            Console.WriteLine($"Обновлённая валюта: {currency.Name}, Курс: {currency.ExchangeRate}, Код: {currency.Code}");

            static void UpdateContract(Employee employee)
            {
                employee.Contract = $"Контракт для {employee.Surname} {employee.Name}, Должность: {employee.Position}, Зарплата: {employee.Salary} руб., Дата начала работы: {employee.DateStartWork}";
            }

            static void UpdateCurrency(ref Currency currency, string newName, decimal newRate, string newCode)
            {
                currency = new Currency(newName, newRate, newCode);
            }

                /*В папку “Application” добавить проект “BankSystem.App”, в него
                добавить новую папку “Services”, в которой создаем класс “BankService”, в
                рамках которого реализуем методы:
                а) метод расчета зарплаты владельцев банка = (прибыль банка -
                расходы) / количество владельцев (при условии, что владелец тоже
                сущность Employee и ЗП это int)
                б) метод преобразования клиента банка в сотрудника, метод
                принимает на вход сущность “Client”, приводит его к типу “Employee” и
                возвращает его в качестве результата для дальнейшей работы.*/

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
