using System;
using System.Collections.Generic;
using System.Linq;
using PencatatanKeuangan.Models;
using PencatatanKeuangan.Repositories;

namespace PencatatanKeuangan.Services
{
    public class TransactionManager
    {
        private readonly DataRepository<Transaction> _repository;
        private int _nextId = 1;

        // Teknik: Table-driven construction untuk nentuin Pemasukan/Pengeluaran otomatis
        private readonly Dictionary<string, TransactionType> _categoryTable = new Dictionary<string, TransactionType>(StringComparer.OrdinalIgnoreCase)
        {
            { "Pemasukan", TransactionType.Pemasukan },
            { "Pengeluaran", TransactionType.Pengeluaran },
            { "Gaji", TransactionType.Pemasukan },
            { "Uang Saku", TransactionType.Pemasukan },
            { "Makanan dan Minuman", TransactionType.Pengeluaran },
            { "Tagihan", TransactionType.Pengeluaran },
            { "Cicilan", TransactionType.Pengeluaran },
            { "Belanja", TransactionType.Pengeluaran },
            { "Transportasi", TransactionType.Pengeluaran },
            { "Hiburan", TransactionType.Pengeluaran },
            { "Pendidikan dan Kesehatan", TransactionType.Pengeluaran }
        };

        public TransactionManager(DataRepository<Transaction> repository)
        {
            _repository = repository;

            // Re-index transaction IDs to ensure they are unique and strictly sequential
            var existing = _repository.GetAll();
            for (int i = 0; i < existing.Count; i++)
            {
                existing[i].Id = i + 1;
            }
            _nextId = existing.Count + 1;

            _repository.Save();
        }

        public void RecordTransaction(double amount, string category, string description)
        {
            // Teknik: Defensive Programming / Design by Contract (DbC)
            // Pre-conditions (Syarat sebelum fungsi jalan)
            if (amount <= 0)
            {
                throw new ArgumentException("Nominal transaksi harus lebih besar dari 0!");
            }
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Kategori tidak boleh kosong.");
            }

            // Validasi kategori via Table-Driven
            TransactionType type = _categoryTable.ContainsKey(category)
                ? _categoryTable[category]
                : TransactionType.Pengeluaran;

            // Pastikan pengeluaran tidak melebihi saldo berjalan
            if (type == TransactionType.Pengeluaran && amount > GetCurrentBalance())
            {
                throw new InvalidOperationException("Saldo tidak mencukupi untuk melakukan transaksi pengeluaran ini!");
            }

            var transaction = new Transaction
            {
                Id = _nextId++,
                Type = type,
                Amount = amount,
                Category = category,
                Description = description,
                Date = DateTime.Now
            };

            _repository.Add(transaction);

            // Post-condition (Syarat memastikan fungsi berhasil)
            if (!_repository.GetAll().Contains(transaction))
            {
                throw new InvalidOperationException("Gagal menyimpan transaksi!");
            }
        }

        public double GetCurrentBalance()
        {
            double balance = 0;
            foreach (var t in _repository.GetAll())
            {
                balance += t.Type == TransactionType.Pemasukan ? t.Amount : -t.Amount;
            }
            return balance;
        }
    }
}