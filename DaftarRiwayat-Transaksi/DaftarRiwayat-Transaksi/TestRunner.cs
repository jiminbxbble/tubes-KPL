using System;
using System.Collections.Generic;
using System.Diagnostics; 
using DaftarRiwayat_Transaksi.Models;
using DaftarRiwayat_Transaksi.Services;
using DaftarRiwayat_Transaksi.Configs;

namespace DaftarRiwayat_Transaksi
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
            Console.WriteLine("Semua pengujian selesai. Tekan Enter...");
            Console.ReadLine();
        }

        static void RunUnitTests()
        {
            Console.WriteLine("[UNIT TEST] Memulai pengujian modul...");
            RiwayatManager<Transaction> testManager = new RiwayatManager<Transaction>();

            // Test A: Validasi Filter 
            Console.Write("- Testing Filter Kategori: ");
            testManager.AddItem(new Transaction(1, 1000, "A", DateTime.Now));
            testManager.AddItem(new Transaction(2, 2000, "B", DateTime.Now));
            var result = testManager.FilterItems(t => t.Category == "A");

            if (result.Count == 1) Console.WriteLine("Berhasil");
            else Console.WriteLine("Gagal");

            // Test B: Validasi DbC 
            Console.Write("- Testing DbC (Negative Amount): ");
            try
            {
                new Transaction(3, -500, "C", DateTime.Now);
                Console.WriteLine("Gagal");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Berhasil");
            }
        }

        static void RunPerformanceTest()
        {
            Console.WriteLine("[PERFORMANCE TEST] Memproses 10.000 data fiktif...");
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