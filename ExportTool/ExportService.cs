using CsvHelper;
using System.Globalization;
using BankSystem.Domain.Models;
using CsvHelper.Configuration;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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

        public void WritePersonsToFileJson<T>(List<T> person, string pathToDirectory, string jsonFileName) where T : class
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string fullPath = Path.Combine(pathToDirectory, jsonFileName);
            string newPersonsJson = JsonConvert.SerializeObject(person, Formatting.Indented);

            if (File.Exists(fullPath) && new FileInfo(fullPath).Length > 2)
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    fileStream.Seek(-2, SeekOrigin.End);

                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.Write(",\n");
                        writer.Write("\n]");
                    }
                }
            }
            else
            {
                File.WriteAllText(fullPath, newPersonsJson.Replace("\n", "\n"));
            }

        }

        public void WritePersonToFileJson<T>(T person, string pathToDirectory, string jsonFileName) where T : class
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string fullPath = Path.Combine(pathToDirectory, jsonFileName);
            string newPersonJson = JsonConvert.SerializeObject(person, Formatting.Indented);

            if (File.Exists(fullPath) && new FileInfo(fullPath).Length > 2)
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    fileStream.Seek(-2, SeekOrigin.End);

                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.Write(",\n  ");
                        writer.Write(newPersonJson.Replace("\n", "\n  "));
                        writer.Write("\n]");
                    }
                }
            }
            else
            {
                File.WriteAllText(fullPath, "[\n  " + newPersonJson.Replace("\n", "\n  ") + "\n]");
            }
        }

        public T ReadPersonsFromFileJson<T>(string pathToDirectory, string jsonFileName)
        {
            string fullPath = Path.Combine(pathToDirectory, jsonFileName);
            string deserializePerson = File.ReadAllText(fullPath);
            T persons = JsonConvert.DeserializeObject<T>(deserializePerson);

            return persons;
        }
    }
}