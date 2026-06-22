using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MonTrack.Models;
using MonTrack.Services;
using NUnit.Framework;

namespace MonTrack.Tests
{
    [TestFixture]
    public class ExportTests
    {
        private ExportApiService _service = null!;
        private string _testFilePath = null!;

        [SetUp]
        public void Setup()
        {
            _service = new ExportApiService();
            _testFilePath = Path.Combine(Path.GetTempPath(), "test_transactions.csv");
        }

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [Test]
        public async Task TestExportSuccess()
        {
            // Arrange
            var data = new List<Transaction>
            {
                new Transaction { Id = 1, Date = DateTime.Now, Amount = 1000, Category = "Food", Description = "Lunch" }
            };

            // Act
            await _service.ExecuteExport("CSV", data, _testFilePath, true);

            // Assert
            Assert.That(File.Exists(_testFilePath), Is.True, "File should be created.");
            var content = File.ReadAllText(_testFilePath);
            Assert.That(content, Is.Not.Empty, "File content should not be empty.");
        }

        [Test]
        public void TestExportFailWithEmptyData()
        {
            // Arrange
            var data = new List<Transaction>(); // Empty list

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => 
                await _service.ExecuteExport("CSV", data, _testFilePath, true));
            
            Assert.That(ex!.Message, Does.Contain("Transaction list cannot be null or empty."));
        }

        [Test]
        public void TestInvalidFilePath()
        {
            // Arrange
            var data = new List<Transaction>
            {
                new Transaction { Id = 1, Date = DateTime.Now, Amount = 1000 }
            };
            string invalidPath = ""; // Whitespace/Empty

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => 
                await _service.ExecuteExport("CSV", data, invalidPath, true));

            Assert.That(ex!.Message, Does.Contain("File path cannot be null or whitespace."));
        }
    }
}
