using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonTrack_PengingatTagihan
{
    public class KonfigurasiTagihan
    {
        public string Kelompok { get; set; }
        public int HariTenggatWaktu { get; set; }
    }

    public class PengingatTagihan
    {
        public enum TagihanState { Tersedia, Lunas, Terlambat }
        public TagihanState StatusSaatIni { get; private set; }

        private static readonly Dictionary<string, KonfigurasiTagihan> 
        TabelKonfigurasi = new Dictionary<string, KonfigurasiTagihan>(StringComparer.OrdinalIgnoreCase)
        {
            { "Listrik", new KonfigurasiTagihan { Kelompok = "Utilitas", HariTenggatWaktu = 20 } },
            { "Air", new KonfigurasiTagihan { Kelompok = "Utilitas", HariTenggatWaktu = 15 } },
            { "Internet", new KonfigurasiTagihan { Kelompok = "Layanan Digital", HariTenggatWaktu = 30 } },
            { "Sewa Rumah", new KonfigurasiTagihan { Kelompok = "Tempat Tinggal", HariTenggatWaktu = 7 } },
            { "Netflix", new KonfigurasiTagihan { Kelompok = "Hiburan", HariTenggatWaktu = 30 } }
        };

        public string Nama { get; set; }
        public string Kategori { get; set; }
        public string Kelompok { get; private set; }
        public int Nominal { get; set; }
        public DateTime TanggalDibuat { get; set; }
        public DateTime Deadline { get; private set; }

        public PengingatTagihan(string nama, string kategori, int nominal, DateTime tanggalDibuat)
        {
            Debug.Assert(nominal > 0, "Nominal harus lebih besar dari nol!");

            if (!TabelKonfigurasi.ContainsKey(kategori))
                throw new ArgumentException($"Kategori '{kategori}' tidak ditemukan di tabel konfigurasi!");

            this.Nama = nama;
            this.Kategori = kategori;
            this.Nominal = nominal;
            this.TanggalDibuat = tanggalDibuat;

            this.StatusSaatIni = TagihanState.Tersedia;

            var config = TabelKonfigurasi[kategori];
            this.Kelompok = config.Kelompok;
            this.Deadline = tanggalDibuat.AddDays(config.HariTenggatWaktu);

            UpdateStatusBerdasarkanWaktu();
        }

        public void UpdateStatusBerdasarkanWaktu()
        {
            if (StatusSaatIni != TagihanState.Lunas)
            {
                if (DateTime.Now > Deadline)
                {
                    StatusSaatIni = TagihanState.Terlambat;
                }
                else
                {
                    StatusSaatIni = TagihanState.Tersedia;
                }
            }
        }

        public void TandaiLunas()
        {
            UpdateStatusBerdasarkanWaktu();

            if (StatusSaatIni == TagihanState.Lunas)
            {
                Console.WriteLine($"[INFO] {Nama} sudah berstatus lunas sebelumnya.");
                return;
            }

            StatusSaatIni = TagihanState.Lunas;
            Console.WriteLine($"[SUKSES] Tagihan {Nama} (Kelompok: {Kelompok}) telah ditandai sebagai LUNAS.");
        }
    }
}