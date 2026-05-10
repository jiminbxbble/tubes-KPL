using System;
using System.Collections.Generic;
using DaftarRiwayat_Transaksi.Models;

namespace DaftarRiwayat_Transaksi.Services
{
    public class RiwayatManager<T>
    {
        private List<T> riwayatItems;

        public RiwayatManager()
        {
            riwayatItems = new List<T>();
        }

        public void AddItem(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Data riwayat tidak boleh kosong (null).");
            }

            riwayatItems.Add(item);
        }

        // Fitur: Mendapatkan semua data - nyambungin fitur ekspor data
        public List<T> GetAllItems()
        {
            return riwayatItems;
        }

        // Fitur: Menampilkan semua data ke Console
        public void DisplayAll()
        {
            Console.WriteLine("\n--- DAFTAR RIWAYAT TRANSAKSI ---");

            if (riwayatItems.Count == 0)
            {
                Console.WriteLine("Belum ada data yang tercatat.");
                return;
            }

            foreach (var item in riwayatItems)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("--------------------------------\n");
        }
    }
}