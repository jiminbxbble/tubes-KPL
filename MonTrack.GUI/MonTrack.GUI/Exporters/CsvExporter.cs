using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using MonTrack.Models;

namespace MonTrack.Exporters
{
    public class CsvExporter : IDataExporter
    {
        public void Export(List<Transaction> transactions, string filePath)
        {
            // --- Design by Contract (DbC) Implementation ---
            // Pre-condition: transactions tidak boleh null atau kosong
            if (transactions == null || transactions.Count == 0)
            {
                throw new ArgumentException("Transaction list cannot be null or empty.", nameof(transactions));
            }

            // Pre-condition: filePath tidak boleh null atau whitespace
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));
            }

            try
            {
                // --- Teknik Code Reuse / Library ---
                // Menggunakan library pihak ketiga 'CsvHelper' untuk menangani penulisan file CSV.
                // Ini mengurangi kerumitan dalam memformat string CSV secara manual.
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Memanggil fungsi dari CsvHelper (Code Reuse)
                    csv.WriteRecords(transactions);
                }
            }
            catch (Exception ex)
            {
                // Defensive Programming: Menangani error saat proses I/O atau penulisan CSV.
                Console.WriteLine($"Error during CSV export: {ex.Message}");
                throw;
            }
        }
    }
}
