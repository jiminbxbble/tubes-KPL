using DaftarRiwayat_Transaksi10.Configs;
using DaftarRiwayat_Transaksi10.Models;
using DaftarRiwayat_Transaksi10.Services;
using System;
using System.Diagnostics;

namespace DaftarRiwayat_Transaksi10
{
    class TestRunner
    {
        static void MainTest(string[] args) // Ingat, ubah ke Main() jika ingin menjalankan file ini sebagai startup
        {
            Console.WriteLine("========== UNIT TESTING & PERFORMANCE TESTING ==========");

            // 1. UNIT TESTING 
            RunUnitTests();

            Console.WriteLine("\n---------------------------------------------------");

            // 2. PERFORMANCE TESTING 
            RunPerformanceTest();

            Console.WriteLine("\n===================================================");
        }

        static void RunUnitTests()
        {
            Console.WriteLine("[UNIT TEST] Memulai pengujian modul");
            RiwayatManager<Transaction> testManager = new RiwayatManager<Transaction>();
            AppConfig config = ConfigService.LoadConfig();

            // Penambahan parameter TransactionType pada data awal
            testManager.AddItem(new Transaction(1, 1000, "A", DateTime.Now, TransactionType.Pemasukan));
            testManager.AddItem(new Transaction(2, 2000, "B", DateTime.Now, TransactionType.Pengeluaran));
            testManager.AddItem(new Transaction(3, 3000, "C", DateTime.Now, TransactionType.Pemasukan));
            testManager.AddItem(new Transaction(4, 4000, "D", DateTime.Now, TransactionType.Pengeluaran));

            // Test A: Validasi Display
            Console.Write("\n---------[Test A] Testing Display---------");
            testManager.DisplayAll(config);
            Console.WriteLine("Status: PASSED");

            // Test B: Validasi Filter 
            Console.Write("\n---------[Test B] Testing Filter Kategori---------");
            var resultFilter = testManager.FilterItems(t => t.Category == "A");
            testManager.DisplayCustomList(resultFilter, config, $"Hasil Filter: A");

            if (resultFilter.Count == 1) Console.WriteLine("Status: PASSED");
            else Console.WriteLine("Status: FAILED");

            // Test C: Validasi Cari berdasarkan Deskripsi
            Console.Write("\n---------[Test C] Testing Cari berdasarkan Deskripsi---------");
            try
            {
                // Perhatikan urutan: TransactionType dahulu, baru Description
                testManager.AddItem(new Transaction(5, 25000, "Makanan", DateTime.Now, TransactionType.Pengeluaran, "Nasi Padang Ayam"));
                testManager.AddItem(new Transaction(6, 15000, "Makanan", DateTime.Now, TransactionType.Pengeluaran, "Nasi Goreng"));
                testManager.DisplayAll(config);

                string keyword = "Padang";
                var searchResult = testManager.FilterItems(t => t.Description.ToLower().Contains(keyword.ToLower()));

                if (searchResult.Count == 1 && searchResult[0].Description.Contains(keyword))
                {
                    testManager.DisplayCustomList(searchResult, config, $"Hasil Cari: {keyword}");
                    Console.WriteLine("Status: PASSED");
                }
                else
                {
                    Console.WriteLine($"Status: FAILED (Ditemukan {searchResult.Count} data)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Status: FAILED - Error: {ex.Message}");
            }

            // Test D: Validasi DbC 
            Console.Write("\n---------[Test D] Testing DbC---------");

            // Test 1: ID <= 0
            Console.WriteLine("\n[Test 1] ID <= 0: ");
            try
            {
                new Transaction(0, 50000, "Makanan", DateTime.Now, TransactionType.Pengeluaran);
                Console.WriteLine("Status: FAILED");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($">> DbC Berhasil Menangkap -> {ex.Message}");
                Console.WriteLine($"Status: PASSED");
            }

            // Test 2: Nominal <= 0
            Console.WriteLine("\n[Test 2] Nominal <= 0: ");
            try
            {
                new Transaction(1, -15000, "Transport", DateTime.Now, TransactionType.Pengeluaran);
                Console.WriteLine("Status: FAILED");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($">> DbC Berhasil Menangkap -> {ex.Message}");
                Console.WriteLine($"Status: PASSED");
            }

            // Test 3: Kategori Kosong
            Console.WriteLine("\n[Test 3] Kategori Kosong: ");
            try
            {
                new Transaction(2, 20000, " ", DateTime.Now, TransactionType.Pengeluaran);
                Console.WriteLine("Status: FAILED");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($">> DbC Berhasil Menangkap -> {ex.Message}");
                Console.WriteLine($"Status: PASSED");
            }

            // Test 4: Tanggal Masa Depan
            Console.WriteLine("\n[Test 4] Tanggal Masa Depan: ");
            try
            {
                new Transaction(3, 30000, "Hiburan", DateTime.Now.AddDays(1), TransactionType.Pengeluaran);
                Console.WriteLine("Status: FAILED");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($">> DbC Berhasil Menangkap -> {ex.Message}");
                Console.WriteLine($"Status: PASSED");
            }

            // Test 5: Deskripsi Terlalu Panjang
            Console.WriteLine("\n[Test 5] Deskripsi Terlalu Panjang: ");
            try
            {
                string longDesc = new string('A', 101);
                // Parameter enum diletakkan sebelum longDesc
                new Transaction(4, 40000, "Lainnya", DateTime.Now, TransactionType.Pengeluaran, longDesc);
                Console.WriteLine("Status: FAILED");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($">> DbC Berhasil Menangkap -> {ex.Message}");
                Console.WriteLine($"Status: PASSED");
            }

            // Test 6: Enum Tipe Tidak Valid (PENGUJIAN BARU)
            Console.WriteLine("\n[Test 6] Tipe Transaksi Tidak Valid (Enum Injection): ");
            try
            {
                new Transaction(5, 50000, "Lainnya", DateTime.Now, (TransactionType)99);
                Console.WriteLine("Status: FAILED");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($">> DbC Berhasil Menangkap -> {ex.Message}");
                Console.WriteLine($"Status: PASSED");
            }
        }

        static void RunPerformanceTest()
        {
            Console.WriteLine("\n[PERFORMANCE TEST] Memproses 10.000 data fiktif...");
            RiwayatManager<Transaction> perfManager = new RiwayatManager<Transaction>();
            Stopwatch sw = new Stopwatch();

            for (int i = 1; i <= 10000; i++)
            {
                // Menambahkan TransactionType pada loop
                perfManager.AddItem(new Transaction(i, i * 10, "Test", DateTime.Now, TransactionType.Pemasukan));
            }

            sw.Start();
            var search = perfManager.FilterItems(t => t.Id == 9999);
            sw.Stop();

            Console.WriteLine($"- Waktu Pencarian: {sw.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"- Status: {(sw.Elapsed.TotalMilliseconds < 100 ? "OPTIMAL" : "PERLU OPTIMASI")}");
        }
    }
}