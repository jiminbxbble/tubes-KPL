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
        private TransactionType _type;
        public TransactionType Type
        {
            get { return _type; }
            set
            {
                // invariant untuk memastikan tipe transaksi valid
                if (!Enum.IsDefined(typeof(TransactionType), value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Tipe transaksi tidak valid.");
                _type = value;
            }
        }

        public Transaction(int id, double amount, string category, DateTime date, TransactionType type, string description = "-")
        {
            // PRE-CONDITIONS
            if (id <= 0) throw new ArgumentException(nameof(id), "ID harus lebih besar dari 0.");

            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Nominal transaksi harus positif.");

            if (string.IsNullOrWhiteSpace(category)) throw new ArgumentNullException(nameof(category), "Kategori tidak boleh kosong.");

            if (date > DateTime.Now) throw new ArgumentException(nameof(date), "Tanggal tidak boleh di masa depan.");

            if (description.Length > 100)
                throw new ArgumentException(nameof(description), "Deskripsi terlalu panjang, maksimal 100 karakter.");

            // Pre-condition khusus untuk enum Type
            if (!Enum.IsDefined(typeof(TransactionType), type))
                throw new ArgumentOutOfRangeException(nameof(type), "Tipe transaksi tidak dikenali.");

            Id = id;
            Amount = amount;
            Category = category;
            Date = date;
            Type = type; // Melalui setter yang sudah dilindungi
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
            Debug.Assert(Enum.IsDefined(typeof(TransactionType), Type), "Invariant gagal: Tipe transaksi tidak valid.");
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"[{Date.ToShortDateString()}] {Type} - {Category}: Rp{Amount}");
        }

        // Override ToString dari RiwayatManager memanggil sesuai formatnya
        public override string ToString()
        {
            return $"[{Date.ToShortDateString()}] {Type,-12} | {Category.PadRight(15)} | Rp{Amount,10:N0} | Ket: {Description}";
        }
    }

    public enum TransactionType
    {
        Pemasukan,
        Pengeluaran
    }
}
