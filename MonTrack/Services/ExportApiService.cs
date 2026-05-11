using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonTrack.Models;

namespace MonTrack.Services
{
    /// <summary>
    /// ExportApiService adalah API internal untuk mengelola operasi ekspor.
    /// 
    /// Teknik: API Internal
    /// - Menyediakan interface terpadu untuk operasi ekspor
    /// - Menggunakan Strategy Pattern untuk memilih eksporter
    /// - Mengurangi coupling antara client dan implementasi eksporter
    /// 
    /// Teknik: Strategy Pattern
    /// - Menyimpan berbagai strategi eksporter dalam dictionary
    /// - Memilih strategi berdasarkan parameter format
    /// - Mudah menambah strategi baru tanpa mengubah existing code
    /// 
    /// Teknik: Asynchronous Programming
    /// - Menggunakan async/await untuk non-blocking I/O
    /// - Mendukung CLO2: Atribut kualitas Performance
    /// - Tidak memblokir thread utama saat ekspor data besar
    /// </summary>
    public class ExportApiService
    {
        // Strategy Pattern: Dictionary untuk menyimpan berbagai eksporter
        private readonly Dictionary<string, IDataExporter> _exporterStrategies;

        /// <summary>
        /// Constructor menginisialisasi semua strategi eksporter yang tersedia.
        /// </summary>
        public ExportApiService()
        {
            _exporterStrategies = new Dictionary<string, IDataExporter>(StringComparer.OrdinalIgnoreCase)
            {
                { "csv", new CsvExporter() },
                // Bisa menambah eksporter lain di sini tanpa mengubah method lain
                // { "excel", new ExcelExporter() },
                // { "pdf", new PdfExporter() }
            };
        }

        /// <summary>
        /// Mengeksekusi operasi ekspor secara asinkron.
        /// 
        /// Teknik: Strategy Pattern
        /// - Memilih eksporter berdasarkan parameter format
        /// - Eksporter yang dipilih adalah strategi yang digunakan
        /// 
        /// Teknik: Asynchronous Programming
        /// - Method bersifat async Task (tidak return value)
        /// - Menggunakan await untuk operasi yang time-consuming
        /// - Memastikan aplikasi tetap responsif (CLO2: Performance)
        /// 
        /// </summary>
        /// <param name="format">Format eksporter (csv, excel, pdf, dll)</param>
        /// <param name="data">Data transaksi yang akan diekspor</param>
        /// <param name="path">Path file tujuan</param>
        public async Task ExecuteExport(string format, List<Transaction> data, string path)
        {
            try
            {
                // Validasi format
                if (!_exporterStrategies.ContainsKey(format))
                {
                    Console.WriteLine($"✗ Format '{format}' tidak didukung");
                    Console.WriteLine($"  Format yang tersedia: {string.Join(", ", _exporterStrategies.Keys)}");
                    return;
                }

                // Strategy Pattern: Ambil eksporter yang sesuai
                var exporter = _exporterStrategies[format];

                Console.WriteLine($"\n[ExportApiService] Memulai ekspor dengan format: {format.ToUpper()}");
                Console.WriteLine($"  Format description: {exporter.GetFormatDescription()}");

                // Asynchronous Programming: Jalankan ekspor di thread pool
                // Ini tidak memblokir thread utama
                await Task.Run(() =>
                {
                    bool success = exporter.Export(data, path);
                    if (!success)
                    {
                        throw new System.IO.IOException($"Ekspor gagal untuk format {format}");
                    }
                });

                Console.WriteLine($"[ExportApiService] ✓ Ekspor selesai dengan sukses\n");
            }
            catch (System.IO.IOException ex)
            {
                Console.WriteLine($"✗ [ExportApiService] Error: {ex.Message}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ [ExportApiService] Error tidak terduga: {ex.GetType().Name} - {ex.Message}\n");
            }
        }

        /// <summary>
        /// Mendapatkan list format yang didukung.
        /// </summary>
        public List<string> GetSupportedFormats()
        {
            return new List<string>(_exporterStrategies.Keys);
        }

        /// <summary>
        /// Menambah eksporter baru secara dinamis (untuk extensibility).
        /// </summary>
        public void RegisterExporter(string format, IDataExporter exporter)
        {
            if (_exporterStrategies.ContainsKey(format))
            {
                Console.WriteLine($"⚠ Format '{format}' sudah ada, akan diganti");
            }

            _exporterStrategies[format] = exporter;
            Console.WriteLine($"✓ Eksporter '{format}' berhasil didaftarkan");
        }
    }
}
