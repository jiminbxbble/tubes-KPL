using System;

namespace PencatatanKeuangan.Models
{
    public enum TransactionType
    {
        Pemasukan,
        Pengeluaran
    }

    public class Transaction
    {
        public int Id { get; set; }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}