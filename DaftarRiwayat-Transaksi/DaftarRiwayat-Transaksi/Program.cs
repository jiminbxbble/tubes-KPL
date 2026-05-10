using System;
using DaftarRiwayat_Transaksi.Models;
using DaftarRiwayat_Transaksi.Services;

namespace DaftarRiwayat_Transaksi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("   SISTEM DAFTAR RIWAYAT TRANSAKSI - DANI    ");
            Console.WriteLine("=============================================");

            RiwayatManager<Transaction> manager = new RiwayatManager<Transaction>();

            try
            {
                Console.WriteLine("\n[1] Menjalankan Test: Input Data Valid...");

                manager.AddItem(new Transaction(1, 50000, "Makanan", DateTime.Now, "Beli nasi padang"));
                manager.AddItem(new Transaction(2, 15000, "Transport", DateTime.Now, "Ojek online"));
                manager.AddItem(new Transaction(3, 100000, "Listrik", DateTime.Now, "Token rumah"));

                manager.DisplayAll();

                Console.WriteLine("[2] Menjalankan Test: Design by Contract (DbC)[cite: 12]...");

                Console.WriteLine("Mencoba input nominal negatif (-5000)...");
                manager.AddItem(new Transaction(4, -5000, "Ilegal", DateTime.Now));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"\n>> DbC Berhasil Menangkap Pelanggaran Kontrak: {ex.ParamName} -> {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n>> Error Terdeteksi: {ex.Message}");
            }

            Console.WriteLine("\n=============================================");
            Console.WriteLine("          PENGUJIAN SELESAI                  ");
            Console.WriteLine("=============================================");
        }
    }
}