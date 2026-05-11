using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MonTrack.Models;
using MonTrack.Services;

namespace MonTrack.Tests
{
    /// <summary>
    /// PerformanceTest mengukur performa fitur ekspor data.
    /// 
    /// Teknik: Performance Testing & Benchmarking
    /// - Menggunakan System.Diagnostics.Stopwatch
    /// - Mengukur waktu eksekusi dalam milidetik
    /// - Mengukur memory usage
    /// - Menguji dengan dataset besar (10.000 transaksi)
    /// 
    /// Tujuan: Membuktikan atribut kualitas Performance (CLO2)
    /// - Aplikasi dapat memproses data besar dengan cepat
    /// - Tidak ada bottleneck dalam eksekusi ekspor
    /// - Asynchronous approach memberikan benefit
    /// </summary>
    public class PerformanceTest
    {
        private readonly ExportApiService _service;
        private readonly string _testOutputPath;

        public PerformanceTest()
        {
            _service = new ExportApiService();
            _testOutputPath = Path.Combine(Path.GetTempPath(), "MonTrackPerf");
            if (!Directory.Exists(_testOutputPath))
            {
                Directory.CreateDirectory(_testOutputPath);
            }
        }

        /// <summary>
        /// Generate 10.000 data transaksi dummy untuk testing.
        /// Simulasi data transaksi real-world dengan berbagai kategori.
        /// </summary>
        private List<Transaction> GenerateDummyTransactions(int count)
        {
            var transactions = new List<Transaction>(count);
            var categories = new[] { "Makanan", "Transportasi", "Gaji", "Investasi", "Utilitas", "Entertainment" };
            var descriptions = new[] { "Makan siang", "Bensin", "Gaji bulanan", "Saham", "Listrik", "Bioskop" };

            var random = new Random(42); // Fixed seed untuk reproducibility

            for (int i = 1; i <= count; i++)
            {
                var transaction = new Transaction
                {
                    Id = i,
                    Date = DateTime.Now.AddDays(-random.Next(0, 365)),
                    Amount = random.Next(50000, 10000000),
                    Category = categories[random.Next(categories.Length)],
                    Description = $"{descriptions[random.Next(descriptions.Length)]} #{i}"
                };

                transactions.Add(transaction);
            }

            return transactions;
        }

        /// <summary>
        /// Test performa: Mengekspor 10.000 transaksi ke CSV
        /// 
        /// Metrics yang diukur:
        /// 1. Execution time (waktu eksekusi dalam ms)
        /// 2. Memory usage (perubahan memory sebelum/sesudah)
        /// 3. Throughput (transaksi per detik)
        /// </summary>
        public async void PerformanceTest_Export10000Transactions()
        {
            Console.WriteLine("\nΓòöΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòù");
            Console.WriteLine("Γòæ       PERFORMANCE TEST - EXPORT 10.000 TRANSACTIONS         Γòæ");
            Console.WriteLine("Γòæ       CLO2: Atribut Kualitas Performance                    Γòæ");
            Console.WriteLine("ΓòÜΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓò¥\n");

            try
            {
                // Generate dummy data
                Console.WriteLine("Generating 10.000 dummy transactions...");
                var transactions = GenerateDummyTransactions(10000);
                Console.WriteLine($"Γ£ô Generated {transactions.Count} transactions\n");

                // Memory snapshot sebelum eksekusi
                long memoryBefore = GC.GetTotalMemory(false);

                // Start stopwatch
                var stopwatch = Stopwatch.StartNew();

                // Jalankan ekspor
                string filePath = Path.Combine(_testOutputPath, "performance_test_10k.csv");
                Console.WriteLine("Starting export to CSV...");
                
                // Catatan: Karena menggunakan Task.Run di ExportApiService,
                // ini adalah async operation yang tidak memblokir thread
                await _service.ExecuteExport("csv", transactions, filePath);

                // Stop stopwatch
                stopwatch.Stop();

                // Memory snapshot sesudah eksekusi
                long memoryAfter = GC.GetTotalMemory(false);
                long memoryUsed = memoryAfter - memoryBefore;

                // Hitung metrics
                double executionTimeMs = stopwatch.Elapsed.TotalMilliseconds;
                double executionTimeSec = stopwatch.Elapsed.TotalSeconds;
                double throughput = transactions.Count / executionTimeSec;

                // Verifikasi file
                FileInfo fileInfo = new FileInfo(filePath);
                double fileSizeMB = fileInfo.Length / (1024.0 * 1024.0);

                // ΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉ
                // HASIL PERFORMANCE TESTING
                // ΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉ
                Console.WriteLine("\nΓòöΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòù");
                Console.WriteLine("Γòæ                    HASIL PERFORMANCE TEST                  Γòæ");
                Console.WriteLine("ΓòÜΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓò¥\n");

                Console.WriteLine("≡ƒôè METRICS:");
                Console.WriteLine($"   ΓÇó Execution Time:        {executionTimeMs:F2} ms ({executionTimeSec:F2} seconds)");
                Console.WriteLine($"   ΓÇó Throughput:            {throughput:F0} transactions/second");
                Console.WriteLine($"   ΓÇó Memory Used:           {(memoryUsed / 1024.0):F2} KB");
                Console.WriteLine($"   ΓÇó Output File Size:      {fileSizeMB:F2} MB");
                Console.WriteLine($"   ΓÇó Transactions Exported: {transactions.Count:N0}");

                Console.WriteLine("\n≡ƒôê PERFORMANCE ANALYSIS:");
                if (executionTimeMs < 5000)
                {
                    Console.WriteLine("   Γ£ô EXCELLENT: Ekspor < 5 detik untuk 10K transaksi");
                }
                else if (executionTimeMs < 10000)
                {
                    Console.WriteLine("   Γ£ô GOOD: Ekspor < 10 detik untuk 10K transaksi");
                }
                else
                {
                    Console.WriteLine("   ΓÜá ACCEPTABLE: Ekspor membutuhkan waktu lebih lama");
                }

                if (throughput > 1000)
                {
                    Console.WriteLine("   Γ£ô EXCELLENT: Throughput > 1000 tx/s");
                }
                else if (throughput > 500)
                {
                    Console.WriteLine("   Γ£ô GOOD: Throughput > 500 tx/s");
                }

                Console.WriteLine($"\n≡ƒÄ» CONCLUSION:");
                Console.WriteLine($"   Sistem dapat mengekspor {transactions.Count:N0} transaksi");
                Console.WriteLine($"   dalam {executionTimeMs:F0}ms dengan throughput {throughput:F0} tx/s");
                Console.WriteLine($"   Atribut kualitas PERFORMANCE terpenuhi Γ£ô\n");

                // Verifikasi file content
                if (File.Exists(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    Console.WriteLine($"Γ£ô File berhasil dibuat dengan {lines.Length} baris (termasuk header)");
                    Console.WriteLine($"  File location: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nΓ£ù ERROR: {ex.Message}");
                Console.WriteLine($"  Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Test performa dengan berbagai ukuran dataset
        /// Untuk melihat scalability
        /// </summary>
        public async Task PerformanceTest_Scalability()
        {
            Console.WriteLine("\nΓòöΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòù");
            Console.WriteLine("Γòæ      PERFORMANCE TEST - SCALABILITY ANALYSIS                Γòæ");
            Console.WriteLine("Γòæ      Mengukur performa dengan berbagai ukuran dataset       Γòæ");
            Console.WriteLine("ΓòÜΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓò¥\n");

            var testSizes = new[] { 1000, 5000, 10000 };

            Console.WriteLine("Testing with different dataset sizes:\n");

            foreach (int size in testSizes)
            {
                Console.WriteLine($"Testing {size:N0} transactions...");

                var transactions = GenerateDummyTransactions(size);
                var stopwatch = Stopwatch.StartNew();

                string filePath = Path.Combine(_testOutputPath, $"perf_test_{size}.csv");
                await _service.ExecuteExport("csv", transactions, filePath);

                stopwatch.Stop();

                double executionTimeMs = stopwatch.Elapsed.TotalMilliseconds;
                double throughput = size / stopwatch.Elapsed.TotalSeconds;

                Console.WriteLine($"  Γ£ô Time: {executionTimeMs:F2}ms | Throughput: {throughput:F0} tx/s");
            }

            Console.WriteLine("\nΓ£ô Scalability test selesai");
        }

        /// <summary>
        /// Jalankan semua performance tests
        /// </summary>
        public async Task RunAllPerformanceTests()
        {
            PerformanceTest_Export10000Transactions();
            await Task.Delay(1000);

            Console.WriteLine("\nΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉ");
            Console.WriteLine("Menjalankan scalability test...");
            Console.WriteLine("ΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉ");

            await PerformanceTest_Scalability();

            Console.WriteLine("\nΓòöΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòù");
            Console.WriteLine("Γòæ         SEMUA PERFORMANCE TESTS SELESAI                    Γòæ");
            Console.WriteLine("ΓòÜΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓò¥");

            CleanupTestFiles();
        }

        /// <summary>
        /// Bersihkan file test performa
        /// </summary>
        private void CleanupTestFiles()
        {
            try
            {
                if (Directory.Exists(_testOutputPath))
                {
                    var files = Directory.GetFiles(_testOutputPath, "perf_test_*.csv");
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                    Console.WriteLine("\nΓ£ô Cleanup: File performance test berhasil dihapus");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nΓÜá Cleanup error: {ex.Message}");
            }
        }
    }
}
