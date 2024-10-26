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
        private const string DirectoryPath = @"E:\Practic\.net-course-2024Okylibaba\ClientJsonFiles";

        [Fact]          
        public void ReadAndWriteClientsPositivrTest()
        {
            var testDataGenerator = new TestDataGenerator();
            var exportService = new ExportService();
            var clientQueue = new ConcurrentQueue<Client>();
            int activeGenerators = 0;
            bool generating = true;

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

                Interlocked.Increment(ref activeGenerators);
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    var clients = testDataGenerator.GenerateClients(20);
                    foreach (var client in clients)
                    {
                        clientQueue.Enqueue(client);
                    }
                    Interlocked.Decrement(ref activeGenerators);
                });
            }

            int fileCounter = 1;
            string currentFileName = $"clients_{fileCounter}.json";
            int currentFileSize = 0;
            var clientsList = new List<Client>();

            var writerThread = new Thread(() =>
            {
                while (generating || !clientQueue.IsEmpty)
                {
                    if (clientQueue.TryDequeue(out Client client))
                    {
                        clientsList.Add(client);
                        string tempFile = Path.GetTempFileName();
                        exportService.WritePersonsToFileJson<Client>(client, DirectoryPath, tempFile);

                        var tempFileSize = new FileInfo(tempFile).Length;
                        if (currentFileSize + tempFileSize > MaxFileSize)
                        {
                            fileCounter++;
                            currentFileName = $"clients_{fileCounter}.json";
                            currentFileSize = 0;
                        }

                        string filePath = Path.Combine(DirectoryPath, currentFileName);
                        File.AppendAllText(filePath, File.ReadAllText(tempFile), Encoding.UTF8);
                        currentFileSize += (int)tempFileSize;

                        File.Delete(tempFile);
                    }
                }
            });

            writerThread.Start();
            while (Volatile.Read(ref activeGenerators) > 0)
            {
                Thread.Sleep(100);
            }
            generating = false;
            writerThread.Join();

            var generatedFiles = Directory.GetFiles(DirectoryPath, "*.json");

            Assert.Equal(7, generatedFiles.Count());

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
                {я
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