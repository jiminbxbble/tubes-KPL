using System;

namespace MonTrack.Models
{
    /// <summary>
    /// Class Transaction merepresentasikan satu transaksi keuangan dalam MonTrack.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Identitas unik transaksi
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tanggal transaksi terjadi
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Jumlah uang dalam transaksi (bisa positif untuk income, negatif untuk expense)
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Kategori transaksi (contoh: "Makanan", "Transportasi", "Gaji")
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Deskripsi detail transaksi
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Constructor default
        /// </summary>
        public Transaction()
        {
        }

        /// <summary>
        /// Constructor dengan parameter untuk kemudahan inisialisasi
        /// </summary>
        public Transaction(int id, DateTime date, double amount, string category, string description)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Category = category;
            Description = description;
        }

        /// <summary>
        /// Override ToString untuk keperluan logging dan debugging
        /// </summary>
        public override string ToString()
        {
            return $"{Id},{Date:yyyy-MM-dd},{Amount},{Category},\"{Description}\"";
        }
    }
}
