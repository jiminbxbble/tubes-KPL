using Xunit;
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
            // Arrange
            var repo = new DataRepository<Transaction>();
            var manager = new TransactionManager(repo);

            // Act
            manager.RecordTransaction(50000, "Uang Saku", "Dikasih ortu");

            // Assert
            Assert.Equal(50000, manager.GetCurrentBalance());
        }
    }
}