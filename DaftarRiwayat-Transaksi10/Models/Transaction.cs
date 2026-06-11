using DaftarRiwayat_Transaksi10.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace DaftarRiwayat_Transaksi10.Models
{
    public class Transaction
    {
        public int Id { get; private set; }
        public double Amount { get; private set; }
        public string Category { get; private set; }
        public DateTime Date { get; private set; }
        public string Description { get; private set; }

        public Transaction(int id, double amount, string category, DateTime date, string description = "-")
        {
            // PRE-CONDITIONS
            if (id <= 0) throw new ArgumentException(nameof(id), "ID harus lebih besar dari 0.");

            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Nominal transaksi harus positif.");

            if (string.IsNullOrWhiteSpace(category)) throw new ArgumentNullException(nameof(category), "Kategori tidak boleh kosong.");

            if (date > DateTime.Now) throw new ArgumentException(nameof(date), "Tanggal tidak boleh di masa depan.");

            if (description.Length > 100)
                throw new ArgumentException(nameof(description), "Deskripsi terlalu panjang, maksimal 100 karakter.");

            Id = id;
            Amount = amount;
            Category = category;
            Date = date;
            Description = string.IsNullOrWhiteSpace(description) ? "-" : description;

            // INVARIANTS
            ObjectInvariant();
        }

        private void ObjectInvariant()
        {
            Debug.Assert(Id > 0);
            Debug.Assert(Amount > 0);
            Debug.Assert(!string.IsNullOrEmpty(Category));
            Debug.Assert(Description != null);
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"[{Date.ToShortDateString()}] {Category}: Rp{Amount}");
        }

        // Override ToString dari RiwayatManager memanggil sesuai formatnya
        public override string ToString()
        {
            return $"[{Date.ToShortDateString()}] {Category.PadRight(15)} | Rp{Amount,10:N0} | Ket: {Description}";
        }
    }
}
