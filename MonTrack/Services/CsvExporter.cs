using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using MonTrack.Models;

namespace MonTrack.Services
{
    /// <summary>
    /// CsvExporter adalah implementasi konkrit dari IDataExporter untuk format CSV.
    /// 
    /// Teknik: Code Reuse / Library
    /// - Menggunakan library CsvHelper dari NuGet
    /// - CsvHelper menangani parsing, escaping, dan format CSV standard
    /// - Ini adalah contoh code reuse: memanfaatkan kode yang sudah teruji
    /// 
    /// Teknik: Defensive Programming
    /// - Menggunakan try-catch untuk menangani error I/O
    /// - Melakukan validasi input sebelum proses
    /// - Memberikan pesan error yang informatif
    /// </summary>
    public class CsvExporter : IDataExporter
    {
        /// <summary>
        /// Mengekspor transaksi ke file CSV menggunakan CsvHelper.
        /// 
        /// TEKNIK CODE REUSE:
        /// Baris-baris kritis yang menunjukkan penggunaan library CsvHelper:
        /// - using (var writer = new StreamWriter(filePath)) → I/O handling
        /// - using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) → Create CSV writer
        /// - csv.WriteRecords(transactions) → REUSE: CsvHelper menangani formatting otomatis
        /// 
        /// </summary>
        public bool Export(List<Transaction> transactions, string filePath)
        {
            try
            {
                // PRE-CONDITION: Design by Contract
                // Validasi input sesuai kontrak yang ada di interface
                if (transactions == null)
                {
                    throw new ArgumentNullException(nameof(transactions), "List transaksi tidak boleh null");
                }

                if (transactions.Count == 0)
                {
                    throw new ArgumentException("List transaksi tidak boleh kosong", nameof(transactions));
                }

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException("File path tidak boleh null atau whitespace", nameof(filePath));
                }

                // Pastikan direktori ada
                string? directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // TEKNIK: CODE REUSE - Menggunakan CsvHelper Library
                // CsvHelper menangani semua kompleksitas CSV formatting
                using (var writer = new StreamWriter(filePath))
                {
                    // Membuat CsvWriter dengan culture invariant
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        // REUSE: Method WriteRecords dari CsvHelper
                        // CsvHelper otomatis:
                        // - Membaca property dari class Transaction
                        // - Membuat header row (Id, Date, Amount, Category, Description)
                        // - Mem-format setiap field dengan benar (escaping quotes, dll)
                        csv.WriteRecords(transactions);
                    }
                }

                // POST-CONDITION: Design by Contract
                // Verifikasi bahwa file berhasil dibuat
                if (!File.Exists(filePath))
                {
                    throw new System.IO.IOException($"File gagal dibuat di {filePath}");
                }

                Console.WriteLine($"✓ [CsvExporter] File CSV berhasil dibuat: {filePath}");
                Console.WriteLine($"  └─ Total transaksi: {transactions.Count}");

                return true;
            }
            catch (ArgumentNullException ex)
            {
                // DEFENSIVE PROGRAMMING: Menangani null reference error
                Console.WriteLine($"✗ [CsvExporter] Error validasi input: {ex.Message}");
                return false;
            }
            catch (ArgumentException ex)
            {
                // DEFENSIVE PROGRAMMING: Menangani invalid argument error
                Console.WriteLine($"✗ [CsvExporter] Error argument: {ex.Message}");
                return false;
            }
            catch (System.IO.IOException ex)
            {
                // DEFENSIVE PROGRAMMING: Menangani file I/O error
                Console.WriteLine($"✗ [CsvExporter] Error I/O: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // DEFENSIVE PROGRAMMING: Catch-all untuk error yang tidak terduga
                Console.WriteLine($"✗ [CsvExporter] Error tidak terduga: {ex.GetType().Name} - {ex.Message}");
                return false;
            }
        }

        public string GetFormatDescription()
        {
            return "Comma-Separated Values (CSV) - Format teks dengan koma sebagai pemisah";
        }

        public string GetFileExtension()
        {
            return ".csv";
        }
    }
}
