using DaftarRiwayat_Transaksi10.Models;
using DaftarRiwayat_Transaksi10.Configs;
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DaftarRiwayat_Transaksi10.Services
{
    public class RiwayatManager<T>
    {
        private List<T> riwayatItems;

        public RiwayatManager()
        {
            riwayatItems = new List<T>();
        }

        // Fitur: Filter Data
        public List<T> FilterItems(Func<T, bool> kriteria)
        {
            return riwayatItems.Where(kriteria).ToList();
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
        public void DisplayAll(AppConfig config)
        {
            Console.WriteLine("\n--- DAFTAR RIWAYAT TRANSAKSI ---");

            int count = 0;
            foreach (var item in riwayatItems)
            {
                if (count >= config.MaxDisplayItems) break;
                Console.WriteLine(item.ToString());
                count++;
            }

            if (riwayatItems.Count == 0)
            {
                Console.WriteLine("Belum ada data yang tercatat.");
                return;
            }

            Console.WriteLine("--------------------------------\n");
        }

        // Fitur: Menampilkan list hasil filter secara khusus
        public void DisplayCustomList(List<T> list, AppConfig config, string judul)
        {
            Console.WriteLine($"\n--- {judul.ToUpper()} ---");
            if (list.Count == 0)
            {
                Console.WriteLine("Data tidak ditemukan.");
                return;
            }

            foreach (var item in list.Take(config.MaxDisplayItems))
            {
                string output = item?.ToString() ?? "";
                Console.WriteLine(output.Replace("Rp", config.DefaultCurrency ?? "IDR"));
            }
        }
    }
}