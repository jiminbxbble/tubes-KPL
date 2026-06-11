using DaftarRiwayat_Transaksi10.Configs;
using DaftarRiwayat_Transaksi10.Models;
using DaftarRiwayat_Transaksi10.Services;
using System;
using System.Collections.Generic;
using System.Transactions;
using Transaction = DaftarRiwayat_Transaksi10.Models.Transaction;

namespace DaftarRiwayat_Transaksi10
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load Config 
            AppConfig config = ConfigService.LoadConfig();
            RiwayatManager<Transaction> manager = new RiwayatManager<Transaction>();

            // SeedData untuk mengisi data awal
            SeedData(manager);

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=====================================");
                Console.WriteLine($"   DAFTAR RIWAYAT TRANSAKSI ({config.DefaultCurrency})");
                Console.WriteLine("=====================================");
                Console.WriteLine("1. Tampilkan Semua Riwayat");
                Console.WriteLine("2. Filter berdasarkan Kategori");
                Console.WriteLine("3. Cari berdasarkan Deskripsi");
                Console.WriteLine("0. Keluar");
                Console.Write("\nPilih Menu: ");

                // Gunakan ?? "" untuk menghindari warning null (CS8600)
                string menu = Console.ReadLine() ?? "";

                switch (menu)
                {
                    case "1":
                        manager.DisplayAll(config);
                        break;
                    case "2":
                        Console.Write("Masukkan Kategori (Makanan/Transport/dll): ");
                        string cat = Console.ReadLine() ?? "";
                        var filtered = manager.FilterItems(t => t.Category.ToLower() == cat.ToLower());
                        manager.DisplayCustomList(filtered, config, $"Hasil Filter: {cat}");
                        break;
                    case "3":
                        Console.Write("Kata kunci deskripsi: ");
                        string key = Console.ReadLine() ?? "";
                        var searchResult = manager.FilterItems(t => t.Description.ToLower().Contains(key.ToLower()));
                        manager.DisplayCustomList(searchResult, config, $"Hasil Pencarian: {key}");
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak tersedia.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nTekan Enter untuk kembali ke menu...");
                    Console.ReadLine();
                }
            }
        }

        // Method SeedData diletakkan di luar Main agar rapi
        static void SeedData(RiwayatManager<Transaction> manager)
        {
            manager.AddItem(new Transaction(1, 50000, "Makanan", DateTime.Now, "Beli Seblak"));
            manager.AddItem(new Transaction(2, 15000, "Transport", DateTime.Now, "Gojek ke Kampus"));
            manager.AddItem(new Transaction(3, 200000, "Belanja", DateTime.Now, "Beli Buku KPL"));
        }
    }
}