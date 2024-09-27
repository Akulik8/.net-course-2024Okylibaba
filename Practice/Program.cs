using BankSystem.App.Services;
using BankSystem.Domain.Models;
using System.Diagnostics;

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
                PhoneNumber = "+37377960929",
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

            static void UpdateContracts(List<Employee> employees)
            {
                foreach (Employee employee in employees)
                {
                    employee.Contract = $"Контракт для {employee.Surname} {employee.Name}, Должность: {employee.Position}, Зарплата: {employee.Salary} руб., Дата начала работы: {employee.DateStartWork}";
                }
            }

            static void UpdateCurrency(ref Currency currency, string newName, decimal newRate, string newCode)
            {
                currency = new Currency
                {
                    Name = newName,
                    ExchangeRate = newRate,
                    Code = newCode
                };
            }

            Employee employee2 = new Employee
            {
                Name = "Игорь",
                Surname = "Афанасьев",
                PhoneNumber = "+37377867829",
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
                PhoneNumber = "+37377511829",
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
                PhoneNumber = "+37377760606",
                Passport = "IПР 123321123",
                Address = "г. Тирасполь, ул. Юности",
                Date = new DateOnly(2010, 10, 16),
                AccountNumber = 123456,
                Balance = 412541,
            };
            Employee newEmployee = bankService.ConvertClientToEmployee(client, "Бухгалтер", 5000);
            UpdateContract(newEmployee);
            Console.WriteLine($"Новый сотрудник: {newEmployee.Surname} {newEmployee.Name}, Должность: {newEmployee.Position}");

            var generator = new TestDataGenerator();
            List<Client> clientsList = generator.GenerateClients(1000);
            Dictionary<string, Client> clientsDictionary = generator.GenerateClientDictionary(clientsList);
            List<Employee> employeesList = generator.GenerateEmployees(1000);

            Random random = new Random();
            string phoneNumbertoFind = clientsList[random.Next(clientsList.Count)].PhoneNumber;

            Stopwatch sw = Stopwatch.StartNew();
            var resultClientInList = clientsList.Find(e => e.PhoneNumber == phoneNumbertoFind);
            sw.Stop();
            if (resultClientInList != null)
            {
                Console.WriteLine($"Клиент найден: {resultClientInList.Surname} {resultClientInList.Name}, телефон: {resultClientInList.PhoneNumber}");
                Console.WriteLine($"Поиск в списке занял: {sw.ElapsedTicks} тиков");
            }
            else
            {
                Console.WriteLine("Клиент не найден");
            }

            sw.Restart();
            var resultClientInDictionary = clientsDictionary[phoneNumbertoFind];
            sw.Stop();
            if (resultClientInDictionary != null)
            {
                Console.WriteLine($"Клиент найден: {resultClientInDictionary.Surname} {resultClientInDictionary.Name}, телефон: {resultClientInDictionary.PhoneNumber}");
                Console.WriteLine($"Поиск в словаре занял: {sw.ElapsedTicks} тиков");
            }
            else
            {
                Console.WriteLine("Клиент не найден");
            }

            int ageLimit = 25;
            List<Client> clientsUnderAge = clientsList.Where(c => GetAge(c.Date, DateTime.Today) < ageLimit).ToList();
            foreach (var youngClient in clientsUnderAge)
            {
                Console.WriteLine($"Клиент: {youngClient.Surname} {youngClient.Name}, день рождения: {youngClient.Date}, телефон: {youngClient.PhoneNumber}");
            }

            static int GetAge(DateOnly dateOfBirth, DateTime today)
            {
                int age = today.Year - dateOfBirth.Year;
                if (dateOfBirth > DateOnly.FromDateTime(today.AddYears(-age)))
                    age--;
                return age;
            }

            var minSalaryEmployee = employeesList.MinBy(e => e.Salary);
            Console.WriteLine($"Сотрудник с минимальной зарплатой: {minSalaryEmployee.Surname} {minSalaryEmployee.Name}, зарплата: {minSalaryEmployee.Salary}");

            for (int i = 0; i < 20; i++)
            {
                sw.Restart();
                var clientByLastOrDefault = clientsDictionary.LastOrDefault();
                sw.Stop();
                Console.WriteLine($"Поиск методом LastOrDefault занял: {sw.ElapsedTicks} тиков");

                sw.Restart();
                var clientByKey = clientsDictionary[clientByLastOrDefault.Value.PhoneNumber];
                sw.Stop();
                Console.WriteLine($"Поиск по ключу занял: {sw.ElapsedTicks} тиков");
            }
        }
    }
}
