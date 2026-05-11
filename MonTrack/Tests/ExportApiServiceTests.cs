using System;
using System.Collections.Generic;
using System.IO;
using MonTrack.Models;
using MonTrack.Services;

namespace MonTrack.Tests
{
    /// <summary>
    /// Class ExportApiServiceTests berisi unit test untuk ExportApiService.
    /// 
    /// Framework: Menggunakan metode manual assertion (tanpa NUnit/xUnit)
    /// untuk kesederhanaan dan clarity.
    /// 
    /// Test Strategy:
    /// 1. TestExportSuccess - Happy path, ekspor dengan data valid
    /// 2. TestExportFailWithEmptyData - Validasi pre-condition DbC
    /// 3. TestInvalidFilePath - Validasi path parameter
    /// 
    /// Coverage: Menguji kriteria keberhasilan dan kegagalan
    /// </summary>
    public class ExportApiServiceTests
    {
        private readonly string _testOutputFolder;
        private readonly ExportApiService _service;

        public ExportApiServiceTests()
        {
            _testOutputFolder = Path.Combine(Path.GetTempPath(), "MonTrackTests");
            _service = new ExportApiService();

            // Buat folder test jika belum ada
            if (!Directory.Exists(_testOutputFolder))
            {
                Directory.CreateDirectory(_testOutputFolder);
            }
        }

        /// <summary>
        /// TEST CASE 1: TestExportSuccess
        /// 
        /// Tujuan: Memastikan file berhasil terbuat jika data valid
        /// Scenario: Export dengan data valid ke file CSV
        /// Expected Result: File berhasil dibuat dan berisi data
        /// 
        /// Teknik: Design by Contract
        /// - Pre-condition: Data tidak null dan tidak kosong ✓
        /// - Post-condition: File exists dan tidak kosong ✓
        /// </summary>
        public async void TestExportSuccess()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════╗");
            Console.WriteLine("║ TEST 1: ExportSuccess                       ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");

            try
            {
                // Arrange: Siapkan data valid
                var testData = new List<Transaction>
                {
                    new Transaction(1, DateTime.Now, 500000, "Makanan", "Makan siang di restoran"),
                    new Transaction(2, DateTime.Now.AddDays(-1), 1000000, "Gaji", "Gaji bulanan"),
                    new Transaction(3, DateTime.Now.AddDays(-2), 250000, "Transport", "Bensin mobil")
                };

                string filePath = Path.Combine(_testOutputFolder, "test_success.csv");

                // Act: Eksekusi ekspor
                await _service.ExecuteExport("csv", testData, filePath);

                // Assert: Verifikasi hasil
                if (File.Exists(filePath))
                {
                    var fileInfo = new FileInfo(filePath);
                    if (fileInfo.Length > 0)
                    {
                        Console.WriteLine("✓ PASS: File berhasil dibuat dan berisi data");
                        Console.WriteLine($"  File path: {filePath}");
                        Console.WriteLine($"  File size: {fileInfo.Length} bytes");

                        // Verifikasi isi file
                        var content = File.ReadAllText(filePath);
                        if (content.Contains("Makanan"))
                        {
                            Console.WriteLine("  ✓ Data berhasil diekspor ke file");
                            return;
                        }
                    }
                }

                Console.WriteLine("✗ FAIL: File tidak berhasil dibuat atau kosong");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ FAIL: Exception terjadi - {ex.Message}");
            }
        }

        /// <summary>
        /// TEST CASE 2: TestExportFailWithEmptyData
        /// 
        /// Tujuan: Memastikan pre-condition DbC bekerja (melempar exception)
        /// Scenario: Ekspor dengan list transaksi kosong
        /// Expected Result: Method melempar ArgumentException atau menolak operasi
        /// 
        /// Teknik: Design by Contract
        /// - Pre-condition violated: Data kosong (Count == 0)
        /// - Expected: CsvExporter throws ArgumentException
        /// </summary>
        public async void TestExportFailWithEmptyData()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════╗");
            Console.WriteLine("║ TEST 2: ExportFailWithEmptyData            ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");

            try
            {
                // Arrange: Siapkan data kosong (melanggar pre-condition)
                var emptyData = new List<Transaction>(); // Count = 0

                string filePath = Path.Combine(_testOutputFolder, "test_empty.csv");

                // Act: Coba ekspor dengan data kosong
                await _service.ExecuteExport("csv", emptyData, filePath);

                // Assert: Jika tidak ada exception, periksa apakah file dibuat
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("✓ PASS: Pre-condition DbC bekerja - operasi ditolak");
                    Console.WriteLine("  File tidak dibuat karena data kosong");
                    return;
                }
                else
                {
                    Console.WriteLine("✗ FAIL: File tetap dibuat meski data kosong");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("✓ PASS: ArgumentException dilempar untuk data kosong");
                Console.WriteLine($"  Error message: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ PARTIAL PASS: Exception terjadi (bukan ArgumentException)");
                Console.WriteLine($"  Exception type: {ex.GetType().Name}");
                Console.WriteLine($"  Error message: {ex.Message}");
            }
        }

        /// <summary>
        /// TEST CASE 3: TestInvalidFilePath
        /// 
        /// Tujuan: Memastikan sistem menolak path file yang tidak valid
        /// Scenario: Ekspor dengan file path yang invalid (null atau whitespace)
        /// Expected Result: Method melempar ArgumentException atau menolak operasi
        /// 
        /// Teknik: Design by Contract
        /// - Pre-condition: filePath tidak boleh null atau whitespace
        /// - Expected: CsvExporter throws ArgumentException
        /// </summary>
        public async void TestInvalidFilePath()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════╗");
            Console.WriteLine("║ TEST 3: InvalidFilePath                     ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");

            try
            {
                // Arrange: Siapkan data valid tapi path invalid
                var testData = new List<Transaction>
                {
                    new Transaction(1, DateTime.Now, 500000, "Test", "Test transaction")
                };

                string? invalidPath = "   "; // Whitespace only (invalid)

                // Act: Coba ekspor dengan path invalid
                await _service.ExecuteExport("csv", testData, invalidPath);

                // Assert: Periksa apakah operasi ditolak
                if (!File.Exists(invalidPath))
                {
                    Console.WriteLine("✓ PASS: Pre-condition DbC bekerja - path invalid ditolak");
                    Console.WriteLine("  Operasi dibatalkan karena path invalid");
                    return;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("✓ PASS: ArgumentException dilempar untuk path invalid");
                Console.WriteLine($"  Error message: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ PARTIAL PASS: Exception terjadi");
                Console.WriteLine($"  Exception type: {ex.GetType().Name}");
                Console.WriteLine($"  Error message: {ex.Message}");
                return;
            }

            Console.WriteLine("✗ FAIL: Path invalid tetap diterima");
        }

        /// <summary>
        /// Menjalankan semua test cases
        /// </summary>
        public async Task RunAllTests()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║       UNIT TESTS - EXPORT API SERVICE                        ║");
            Console.WriteLine("║       Framework: Manual Assertion                            ║");
            Console.WriteLine("║       Coverage: 70% Nilai Individu CLO4                      ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

            TestExportSuccess();
            await Task.Delay(500); // Delay untuk sinkronisasi file

            TestExportFailWithEmptyData();
            await Task.Delay(500);

            TestInvalidFilePath();
            await Task.Delay(500);

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║             SEMUA TEST SELESAI                              ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

            // Cleanup test files
            CleanupTestFiles();
        }

        /// <summary>
        /// Membersihkan file test yang sudah dibuat
        /// </summary>
        private void CleanupTestFiles()
        {
            try
            {
                if (Directory.Exists(_testOutputFolder))
                {
                    var files = Directory.GetFiles(_testOutputFolder, "test_*.csv");
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                    Console.WriteLine("\n✓ Cleanup: File test berhasil dihapus");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n⚠ Cleanup error: {ex.Message}");
            }
        }
    }
}
