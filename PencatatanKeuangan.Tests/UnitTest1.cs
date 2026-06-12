using Xunit;
using System.IO;
using PencatatanKeuangan.Models;
using PencatatanKeuangan.Repositories;
using PencatatanKeuangan.Services;

namespace PencatatanKeuangan.Tests
{
    public class TransactionTests
    {
        [Fact]
        public void TestInputValid_HarusBerhasil()
        {
            string projectRoot = @"d:\4. Thoriq_KULIAH\4. Matkul\Semester 4\LKPL\TUBES-Thoriq\tubes-KPL";
            string testFile = Path.Combine(projectRoot, "_Output", "Database", "test_transactions.json");
            
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            try
            {
                // Arrange
                var repo = new DataRepository<Transaction>("test_transactions.json");
                var manager = new TransactionManager(repo);

                // Act
                manager.RecordTransaction(50000, "Uang Saku", "Dikasih ortu");

                // Assert
                Assert.Equal(50000, manager.GetCurrentBalance());
            }
            finally
            {
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
            }
        }
    }
}