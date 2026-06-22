using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonTrack_PengingatTagihan
{
    public static class KategoriTagihan
    {
        public const string Utilitas = "Utilitas";
        public const string LayananDigital = "Layanan digital";
        public const string Pendidikan = "Pendidikan";
        public const string Finansial = "Finansial & Cicilan";
        public const string Asuransi = "Asuransi & Kesehatan";
    }

    public class PengingatTagihan
    {
        public enum TagihanState { Tersedia, Lunas, Terlambat }
        public TagihanState StatusSaatIni { get; private set; }

        public static readonly HashSet<string> ValidKategori = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            KategoriTagihan.Utilitas,
            KategoriTagihan.LayananDigital,
            KategoriTagihan.Pendidikan,
            KategoriTagihan.Finansial,
            KategoriTagihan.Asuransi
        };

        public string Nama { get; set; }
        public string Kategori { get; set; }
        public string Kelompok { get; private set; }
        public int Nominal { get; set; }
        public DateTime TanggalDibuat { get; set; }
        public DateTime Deadline { get; set; }
        public string Repetisi { get; set; } = "Sekali";

        public PengingatTagihan(string nama, string kategori, int nominal, DateTime tanggalDibuat, DateTime? deadline = null, string repetisi = "Sekali")
        {
            Debug.Assert(nominal > 0, "Nominal harus lebih besar dari nol!");

            if (!ValidKategori.Contains(kategori))
                throw new ArgumentException($"Kategori '{kategori}' tidak ditemukan di daftar kategori valid!");

            this.Nama = nama;
            this.Kategori = kategori;
            this.Nominal = nominal;
            this.TanggalDibuat = tanggalDibuat;
            this.Repetisi = repetisi;

            this.StatusSaatIni = TagihanState.Tersedia;

            this.Kelompok = kategori;
            this.Deadline = deadline ?? tanggalDibuat.AddDays(30);

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