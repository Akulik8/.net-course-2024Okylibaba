using BankSystem.App.Services;
using BankSystem.Domain.Models;
using ExportTool;
using System.Collections.Concurrent;
using System.Text;

namespace BankSystem.Data.Tests
{
    public class ThreadAndTaskTests
    {
        private const int MaxFileSize = 1024 * 5;
        private const string DirectoryPath = @"E:\Practic\ClientJsonFiles";

        [Fact]          
        public void ReadAndWriteJsonClientsPositiveTest()
        {
            //Arrange    
            var testDataGenerator = new TestDataGenerator();
            var exportService = new ExportService();
            var clientQueue = new ConcurrentQueue<Client>();
            var clientsFromFile = new ConcurrentBag<Client>();
            int activeGeneratorsCount = 0;
            bool isGenerating = true;

            //Act
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            else 
            {
                foreach (var file in Directory.GetFiles(DirectoryPath))
                {
                    File.Delete(file);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                Interlocked.Increment(ref activeGeneratorsCount);
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    var clients = testDataGenerator.GenerateClients(20);
                    foreach (var client in clients)
                    {
                        clientQueue.Enqueue(client);
                    }
                    Interlocked.Decrement(ref activeGeneratorsCount);
                });
            }

            int fileCounter = 1;
            string currentFileName = $"clients_{fileCounter}.json";
            int currentFileSize = 0;
            string filePath = Path.Combine(DirectoryPath, currentFileName);
         

            var writerThread = new Thread(() =>
            {
                while (isGenerating || !clientQueue.IsEmpty)
                {
                    if (clientQueue.TryDequeue(out Client client))
                    {
                        string tempFile = Path.GetTempFileName();

                        exportService.WritePersonToFileJson(client, DirectoryPath, tempFile);

                        var tempFileSize = new FileInfo(tempFile).Length;
                        if (currentFileSize + tempFileSize >= MaxFileSize)
                        {
                            fileCounter++;
                            currentFileName = $"clients_{fileCounter}.json";
                            currentFileSize = 0;
                        }
                        exportService.WritePersonToFileJson(client, DirectoryPath, currentFileName);
                        currentFileSize += (int)tempFileSize;

                        File.Delete(tempFile);
                    }
                }
            });

            writerThread.Start();
            while (Volatile.Read(ref activeGeneratorsCount) > 0)
            {
                Thread.Sleep(100);
            }
            isGenerating = false;
            writerThread.Join();

            var readerThread = new Thread(() =>
            {
                var generatedFiles = Directory.GetFiles(DirectoryPath, "*.json");

                foreach ( var file in generatedFiles) 
                { 
                    var clients = exportService.ReadPersonsFromFileJson<List<Client>>(DirectoryPath, Path.GetFileName(file));
                    foreach (var client in clients)
                    {
                        clientsFromFile.Add(client);
                    }
                }
                Thread.Sleep(100);
            });

            readerThread.Start();
            readerThread.Join();
            int totalClientsDeserialized = clientsFromFile.Count;
            
            //Assert
            Assert.Equal(totalClientsDeserialized, 100);
        }

        [Fact]
        public void MultithreadedAmountTransferTest()
        {
            //Arrange                
            var lockObject = new Object();


            var accountTest = new Account()
            {
                Amount = 0
            };

            //Act
            var completed = 0;
            ThreadPool.QueueUserWorkItem(_ => 
            {
                for (int i = 0; i < 10; i++)
                {
                    lock (lockObject)
                    {
                        accountTest.Amount += 100;
                    }
                }
                Interlocked.Increment(ref completed);
            });
            ThreadPool.QueueUserWorkItem(_ => 
            {
                for (int i = 0; i < 10; i++)
                {
                    lock (lockObject) 
                    {
                        accountTest.Amount += 100;
                    }
                }
                Interlocked.Increment(ref completed);
            });
            while (completed < 2)
            {
                Thread.Sleep(25);
            }

            //Assert
            Assert.Equal(accountTest.Amount, 2000);
        }
    }
}