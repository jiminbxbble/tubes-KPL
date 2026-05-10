using System;
using PencatatanKeuangan.Models;
using PencatatanKeuangan.Repositories;
using PencatatanKeuangan.Services;

namespace PencatatanKeuangan
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new DataRepository<Transaction>();
            var manager = new TransactionManager(repo);
            bool isRunning = true;

            Console.WriteLine("=====================================");
            Console.WriteLine("    Selamat Datang di MonTrack!      ");
            Console.WriteLine("=====================================");

            while (isRunning)
            {
                Console.WriteLine("\n--- Menu Pencatatan Transaksi ---");
                Console.WriteLine($"Saldo Saat Ini: Rp {manager.GetCurrentBalance():N0}");
                Console.WriteLine("1. Catat Transaksi Baru");
                Console.WriteLine("2. Keluar");
                Console.Write("Pilih menu (1-2): ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("\nNominal (Rp): ");
                    if (!double.TryParse(Console.ReadLine(), out double amount))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Input nominal tidak valid, masukkan angka");
                        Console.ResetColor();
                        continue;
                    }

                    Console.Write("Kategori: ");
                    string category = Console.ReadLine();

                    Console.Write("Deskripsi Singkat: ");
                    string description = Console.ReadLine();

                    try
                    {
                        // Bagian ini bakal mancing pesan error DbC kalau inputnya ngaco
                        manager.RecordTransaction(amount, category, description);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[SUKSES] Transaksi berhasil dicatat!");
                        Console.ResetColor();
                    }
                    catch (Exception ex)
                    {
                        // Nangkep pesan error dari Defensive Programming
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n[ERROR] {ex.Message}");
                        Console.ResetColor();
                    }
                }
                else if (choice == "2")
                {
                    isRunning = false;
                    Console.WriteLine("Keluar dari sistem.");
                }
                else
                {
                    Console.WriteLine("Pilihan tidak valid.");
                }
            }
        }
    }
}