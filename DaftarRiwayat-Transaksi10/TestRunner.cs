using DaftarRiwayat_Transaksi10.Configs;
using DaftarRiwayat_Transaksi10.Models;
using DaftarRiwayat_Transaksi10.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using Transaction = DaftarRiwayat_Transaksi10.Models.Transaction;

namespace DaftarRiwayat_Transaksi10
{
    class TestRunner
    {
        static void Main(string[] args)
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

            testManager.AddItem(new Transaction(1, 1000, "A", DateTime.Now));
            testManager.AddItem(new Transaction(2, 2000, "B", DateTime.Now));
            testManager.AddItem(new Transaction(3, 3000, "C", DateTime.Now));
            testManager.AddItem(new Transaction(4, 4000, "D", DateTime.Now));

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
                testManager.AddItem(new Transaction(5, 25000, "Makanan", DateTime.Now, "Nasi Padang Ayam"));
                testManager.AddItem(new Transaction(6, 15000, "Makanan", DateTime.Now, "Nasi Goreng"));
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

            // Test ID <= 0
            Console.WriteLine("\n[Test 1] ID <= 0: ");
            try
            {
                new Transaction(0, 50000, "Makanan", DateTime.Now);
                Console.WriteLine("Status: FAILED");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($">> DbC Berhasil Menangkap Pelanggaran Kontrak -> {ex.Message}");
                Console.WriteLine($"Status: PASSED");
            }

            // Test Nominal <= 0
            Console.WriteLine("\n[Test 2] Nominal <= 0: ");
            try
            {
                new Transaction(1, -15000, "Transport", DateTime.Now);
                Console.WriteLine("Status: FAILED");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($">> DbC Berhasil Menangkap Pelanggaran Kontrak -> {ex.Message}");
                Console.WriteLine($"Status: PASSED");
            }

            // Test Kategori Kosong
            Console.WriteLine("\n[Test 3] Kategori Kosong: ");
            try
            {
                new Transaction(2, 20000, " ", DateTime.Now);
                Console.WriteLine("Status: FAILED");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($">> DbC Berhasil Menangkap Pelanggaran Kontrak -> {ex.Message}");
                Console.WriteLine($"Status: PASSED");
            }

            // Test Tanggal Masa Depan
            Console.WriteLine("\n[Test 4] Tanggal Masa Depan: ");
            try
            {
                new Transaction(3, 30000, "Hiburan", DateTime.Now.AddDays(1));
                Console.WriteLine("Status: FAILED");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($">> DbC Berhasil Menangkap Pelanggaran Kontrak -> {ex.Message}");
                Console.WriteLine($"Status: PASSED");
            }

            // Test Deskripsi Terlalu Panjang
            Console.WriteLine("\n[Test 5] Deskripsi Terlalu Panjang: ");
            try
            {
                string longDesc = new string('A', 101);
                new Transaction(4, 40000, "Lainnya", DateTime.Now, longDesc);
                Console.WriteLine("Status: FAILED");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($">> DbC Berhasil Menangkap Pelanggaran Kontrak -> {ex.Message}");
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
                perfManager.AddItem(new Transaction(i, i * 10, "Test", DateTime.Now));
            }

            sw.Start();
            var search = perfManager.FilterItems(t => t.Id == 9999);
            sw.Stop();

            Console.WriteLine($"- Waktu Pencarian: {sw.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"- Status: {(sw.Elapsed.TotalMilliseconds < 100 ? "OPTIMAL" : "PERLU OPTIMASI")}");
        }
    }
}