using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonTrack_PengingatTagihan
{
    public class PengingatTagihan
    {
        // Automata untuk status tagihan
        public enum TagihanState { Tersedia, Lunas, Terlambat }
        public TagihanState StatusSaatIni { get; private set; }

        // Table-driven design untuk biaya tambahan berdasarkan kategori tagihan
        private static readonly Dictionary<string, int> TabelBiayaTambahan = new Dictionary<string, int>
        {
            { "Listrik", 5000 },
            { "Air", 2500 },
            { "Internet", 10000 }
        };

        public string Nama { get; set; }
        public int Nominal { get; set; }

        public PengingatTagihan(string nama, int nominal)
        {
            // DBC (Design by Contract)
            Debug.Assert(nominal > 0, "Nominal harus lebih besar dari nol!");

            this.Nama = nama;
            this.Nominal = nominal;
            this.StatusSaatIni = TagihanState.Tersedia; // Initial State
        }

        public void Bayar(string kategori, int uangBayar)
        {
            // Mengambil biaya tambahan dari Tabel (Table-driven)
            int tambahan = TabelBiayaTambahan.ContainsKey(kategori) ? TabelBiayaTambahan[kategori] : 0;
            int totalHarusDibayar = Nominal + tambahan;

            // Design by Contract: Pastikan uang bayar mencukupi
            Debug.Assert(uangBayar >= totalHarusDibayar, "Uang pembayaran tidak mencukupi!");

            // Transisi State (Automata)
            if (StatusSaatIni == TagihanState.Tersedia || StatusSaatIni == TagihanState.Terlambat)
            {
                StatusSaatIni = TagihanState.Lunas;
                Console.WriteLine($"[SUKSES] {Nama} berhasil dibayar (Admin: {tambahan}).");
            }
        }
    }
}