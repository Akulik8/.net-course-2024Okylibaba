using CsvHelper;
using System.Globalization;
using BankSystem.Domain.Models;
using CsvHelper.Configuration;
using System.Text;
using Newtonsoft.Json;

namespace ExportTool
{
    public class ExportService
    {
        public void WriteClientsToCsv(List<Client> clients, string pathToDirectory, string csvFileName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string fullPath = Path.Combine(pathToDirectory, csvFileName);
            using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";"
                    };

                    using (var writer = new CsvWriter(streamWriter, config))
                    {
                        writer.Context.TypeConverterOptionsCache.GetOptions<DateOnly>().Formats = new[] { "yyyy-MM-dd" };
                        writer.WriteRecords(clients);
                        writer.Flush();
                    }
                }
            }
        }

        public List<Client> ReadClientsFromCsv(string pathToDirectory, string csvFileName)
        {
            var clientList = new List<Client>();

            string fullPath = Path.Combine(pathToDirectory, csvFileName);

            using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";"
                    };

                    using (var reader = new CsvReader(streamReader, config))
                    {
                        reader.Context.TypeConverterOptionsCache.GetOptions<DateOnly>().Formats = new[] { "yyyy-MM-dd" };
                        clientList = reader.GetRecords<Client>().ToList();
                    }
                }
            }
            return clientList;
        }

        public void WritePersonsToFileJson<T>(T person, string pathToDirectory, string csvFileName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string fullPath = Path.Combine(pathToDirectory, csvFileName);
            string serializePerson = JsonConvert.SerializeObject(person, Formatting.Indented);
            File.WriteAllText(fullPath, serializePerson);
        }

        public T ReadPersonsFromFileJson<T>(string pathToDirectory, string csvFileName)
        {
            string fullPath = Path.Combine(pathToDirectory, csvFileName);
            string deserializePerson = File.ReadAllText(fullPath);
            T persons = JsonConvert.DeserializeObject<T>(deserializePerson);
            
            return persons;
        }
    }
}