using CsvHelper;
using System.Globalization;
using BankSystem.Domain.Models;
using CsvHelper.Configuration;
using System.Text;

namespace ExportTool
{
    public class ExportService
    {
        private string _pathToDirectory { get; set; }
        private string _csvFileName { get; set; }

        public ExportService(string pathToDirectory, string textFileName)
        {
            _pathToDirectory = pathToDirectory;
            _csvFileName = textFileName;
        }

        public void WriteClientsToCsv(List<Client> clients)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);
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

        public List<Client> ReadClientsFromCsv()
        {
            var clientList = new List<Client>();

            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);

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
    }
}