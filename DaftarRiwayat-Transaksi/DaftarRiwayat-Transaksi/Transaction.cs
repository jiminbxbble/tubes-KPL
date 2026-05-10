using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DaftarRiwayat_Transaksi
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
            if (id <= 0) throw new ArgumentException("ID harus lebih besar dari 0.");

            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Nominal transaksi harus positif.");

            if (string.IsNullOrWhiteSpace(category)) throw new ArgumentNullException(nameof(category), "Kategori tidak boleh kosong.");

            if (date > DateTime.Now) throw new ArgumentException("Tanggal tidak boleh di masa depan.");

            if (description.Length > 100)
                throw new ArgumentException("Deskripsi terlalu panjang, maksimal 100 karakter.");

            this.Id = id;
            this.Amount = amount;
            this.Category = category;
            this.Date = date;
            this.Description = string.IsNullOrWhiteSpace(description) ? "-" : description;

            // INVARIANTS
            ObjectInvariant();
        }

        private void ObjectInvariant()
        {
            Debug.Assert(this.Id > 0);
            Debug.Assert(this.Amount > 0);
            Debug.Assert(!string.IsNullOrEmpty(this.Category));
            Debug.Assert(this.Description != null);
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