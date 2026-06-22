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
                manager.RecordTransaction(50000, "Pemasukan", "Dikasih ortu");

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

        [Fact]
        public void TestNegativeBalance_ShouldThrowException()
        {
            string projectRoot = @"d:\4. Thoriq_KULIAH\4. Matkul\Semester 4\LKPL\TUBES-Thoriq\tubes-KPL";
            string testFile = Path.Combine(projectRoot, "_Output", "Database", "test_neg_transactions.json");
            
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            try
            {
                var repo = new DataRepository<Transaction>("test_neg_transactions.json");
                var manager = new TransactionManager(repo);

                // Initial balance is 0. Adding expense of 5000 should fail because balance would become negative.
                var ex = Assert.Throws<System.InvalidOperationException>(() => 
                    manager.RecordTransaction(5000, "Makanan dan Minuman", "Beli kopi")
                );
                Assert.Contains("Saldo tidak mencukupi", ex.Message);
            }
            finally
            {
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
            }
        }

        [Fact]
        public void TestNewCategoriesMapping_ShouldInferCorrectType()
        {
            string projectRoot = @"d:\4. Thoriq_KULIAH\4. Matkul\Semester 4\LKPL\TUBES-Thoriq\tubes-KPL";
            string testFile = Path.Combine(projectRoot, "_Output", "Database", "test_cat_transactions.json");
            
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            try
            {
                var repo = new DataRepository<Transaction>("test_cat_transactions.json");
                var manager = new TransactionManager(repo);

                // Record income "Gaji" -> should increase balance
                manager.RecordTransaction(100000, "Gaji", "Gaji bulanan");
                Assert.Equal(100000, manager.GetCurrentBalance());

                // Record expense "Makanan dan Minuman" -> should decrease balance
                manager.RecordTransaction(30000, "Makanan dan Minuman", "Makan siang");
                Assert.Equal(70000, manager.GetCurrentBalance());

                // Record expense "Tagihan" -> should decrease balance
                manager.RecordTransaction(20000, "Tagihan", "Listrik");
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