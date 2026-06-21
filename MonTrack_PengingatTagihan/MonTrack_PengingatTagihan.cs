using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonTrack_PengingatTagihan
{
    // Konstanta untuk menghindari magic strings pada kategori
    public static class KategoriTagihan
    {
        public const string Utilitas = "Utilitas";
        public const string LayananDigital = "Layanan digital";
        public const string Pendidikan = "Pendidikan";
        public const string Finansial = "Finansial & Cicilan";
        public const string Asuransi = "Asuransi & Kesehatan";
    }

    // --- Table-driven construction untuk data ---
    public class KonfigurasiTagihan
    {
        public int TenggatWaktuDefault { get; set; }
    }

    // --- Automata untuk state transition ---
    public enum TagihanState { Tersedia, Terlambat, Lunas }
    public enum Trigger { WaktuJatuhTempo, BayarSekarang }

    public class TransisiState
    {
        public TagihanState StateAwal { get; set; }
        public Trigger Trigger { get; set; }
        public TagihanState StateAkhir { get; set; }
    }

    public class PengingatTagihan
    {
        public int Id { get; private set; }
        public string Nama { get; private set; }
        public string Kategori { get; private set; }
        public int Nominal { get; private set; }
        public DateTime TanggalDibuat { get; private set; }
        public DateTime TanggalJatuhTempo { get; private set; }
        public TagihanState StatusSaatIni { get; private set; }

        // Table-driven construction: Menggunakan konstanta untuk menghindari magic strings.
        private static readonly Dictionary<string, KonfigurasiTagihan> TabelKonfigurasi =
            new Dictionary<string, KonfigurasiTagihan>(StringComparer.OrdinalIgnoreCase)
        {
            { KategoriTagihan.Utilitas, new KonfigurasiTagihan { TenggatWaktuDefault = 30 } },
            { KategoriTagihan.LayananDigital, new KonfigurasiTagihan { TenggatWaktuDefault = 30 } },
            { KategoriTagihan.Pendidikan, new KonfigurasiTagihan { TenggatWaktuDefault = 30 } },
            { KategoriTagihan.Finansial, new KonfigurasiTagihan { TenggatWaktuDefault = 30 } },
            { KategoriTagihan.Asuransi, new KonfigurasiTagihan { TenggatWaktuDefault = 30 } }
        };

        // Table-driven automata untuk state transition
        private static readonly TransisiState[] TabelTransisiAutomata = new[]
        {
            new TransisiState { StateAwal = TagihanState.Tersedia, Trigger = Trigger.WaktuJatuhTempo, StateAkhir = TagihanState.Terlambat },
            new TransisiState { StateAwal = TagihanState.Tersedia, Trigger = Trigger.BayarSekarang, StateAkhir = TagihanState.Lunas },
            new TransisiState { StateAwal = TagihanState.Terlambat, Trigger = Trigger.BayarSekarang, StateAkhir = TagihanState.Lunas }
        };

        public PengingatTagihan(int id, string nama, string kategori, int nominal, DateTime tanggalDibuat, DateTime? tanggalJatuhTempo)
        {
            Id = id;
            StatusSaatIni = TagihanState.Tersedia; // Initial State
            SetupDetail(nama, kategori, nominal, tanggalDibuat, tanggalJatuhTempo);
        }

        private void SetupDetail(string nama, string kategori, int nominal, DateTime tanggalDibuat, DateTime? tanggalJatuhTempo)
        {
            // Design by Contract: Memastikan input valid menggunakan Debug.Assert supaya saat pembentukan objek, semua kondisi terpenuhi.
            // Jika tidak, akan melempar error yang jelas ke UI layer.
            Debug.Assert(nominal > 0, "Nominal harus lebih besar dari nol!");

            // Defensive Programming: Validasi kategori menggunakan tabel konfigurasi untuk memastikan hanya kategori yang valid yang diterima.
            if (!TabelKonfigurasi.ContainsKey(kategori))
            {
                throw new ArgumentException($"Kategori '{kategori}' tidak ditemukan!");
            }

            Nama = nama;
            Kategori = kategori;
            Nominal = nominal;
            TanggalDibuat = tanggalDibuat;
            TanggalJatuhTempo = HitungTanggalJatuhTempo(kategori, tanggalDibuat, tanggalJatuhTempo);
        }

        // Method untuk menghitung tanggal jatuh tempo berdasarkan kategori dan tanggal dibuat, dengan menggunakan konfigurasi dari tabel.
        private DateTime HitungTanggalJatuhTempo(string kategori, DateTime tanggalDibuat, DateTime? tanggalJatuhTempo)
        {
            if (!tanggalJatuhTempo.HasValue)
            {
                var config = TabelKonfigurasi[kategori];
                return tanggalDibuat.AddDays(config.TenggatWaktuDefault);
            }

            // Design by Contract: Memastikan tanggal jatuh tempo valid (harus setelah tanggal dibuat). Jika tidak, akan melempar error yang jelas ke UI layer.
            Debug.Assert(tanggalJatuhTempo.Value > tanggalDibuat, "Tanggal jatuh tempo harus setelah tanggal dibuat!");
            return tanggalJatuhTempo.Value;
        }

        // Method untuk mengubah detail tagihan, sekaligus memeriksa apakah perubahan tersebut mempengaruhi status (misal: mengubah tanggal jatuh tempo).
        public void UpdateDetail(string namaBaru, string kategoriBaru, int nominalBaru, DateTime ubahTanggal, DateTime? ubahTenggat)
        {
            SetupDetail(namaBaru, kategoriBaru, nominalBaru, ubahTanggal, ubahTenggat);
            CekWaktuJatuhTempo();
        }

        // Automata untuk mengubah state berdasarkan trigger yang terjadi
        private void UbahState(Trigger trigger)
        {
            var transisi = TabelTransisiAutomata.FirstOrDefault(t => t.StateAwal == StatusSaatIni && t.Trigger == trigger);
            if (transisi != null)
            {
                StatusSaatIni = transisi.StateAkhir;
            }
        }

        // Method untuk memeriksa apakah tagihan sudah melewati tanggal jatuh tempo, dan jika iya, otomatis mengubah status menjadi Terlambat.
        public void CekWaktuJatuhTempo()
        {
            if (StatusSaatIni != TagihanState.Lunas && DateTime.Now > TanggalJatuhTempo)
            {
                UbahState(Trigger.WaktuJatuhTempo);
            }
        }

        // Method untuk menandai tagihan sebagai lunas, sekaligus memeriksa apakah status sudah Lunas (untuk menghindari perubahan state yang tidak perlu).
        public void TandaiLunas()
        {
            if (StatusSaatIni == TagihanState.Lunas) return;
            UbahState(Trigger.BayarSekarang);
        }
    }

    // --- CRUD ---
    public class TagihanManager
    {
        // Penamaan private field menggunakan underscore standar C#
        private readonly List<PengingatTagihan> _daftarTagihan = new List<PengingatTagihan>();
        private int _nextId = 1;

        public void CreateTagihan(string nama, string kategori, int nominal, DateTime tanggalDibuat, DateTime? tanggalJatuhTempo)
        {
            // Memastikan bahwa setiap tagihan yang dibuat memiliki ID unik yang otomatis bertambah, sehingga tidak perlu khawatir tentang duplikasi ID.
            var tagihanBaru = new PengingatTagihan(_nextId++, nama, kategori, nominal, tanggalDibuat, tanggalJatuhTempo);
            _daftarTagihan.Add(tagihanBaru);
        }

        // Defensive Programming: Mengembalikan IReadOnlyList agar list hanya bisa dibaca dan tidak bisa di-modifikasi sembarangan (Add/Remove) dari luar.
        public IReadOnlyList<PengingatTagihan> GetSemuaTagihan()
        {
            foreach (var t in _daftarTagihan)
            {
                t.CekWaktuJatuhTempo();
            }
            return _daftarTagihan.AsReadOnly();
        }

        public void UpdateTagihan(int id, string namaBaru, string kategoriBaru, int nominalBaru, DateTime tanggalBaru, DateTime? tenggatBaru)
        {
            var tagihan = _daftarTagihan.FirstOrDefault(t => t.Id == id);

            // Defensive Programming: Memastikan bahwa tagihan yang ingin diupdate benar-benar ada sebelum mencoba mengupdate-nya
            if (tagihan == null)
            {
                throw new ArgumentException($"Tagihan dengan ID '{id}' tidak ditemukan!");
            }

            // Jika validasi internal (DbC) gagal, ia akan otomatis melempar error ke UI layer.
            tagihan.UpdateDetail(namaBaru, kategoriBaru, nominalBaru, tanggalBaru, tenggatBaru);
        }

        public void DeleteTagihan(int id)
        {
            var tagihan = _daftarTagihan.FirstOrDefault(t => t.Id == id);

            // Defensive Programming: Memastikan bahwa tagihan yang ingin dihapus benar-benar ada sebelum mencoba menghapusnya
            // sehingga tidak terjadi error yang tidak jelas di UI layer.
            if (tagihan == null)
            {
                throw new ArgumentException($"Tagihan dengan ID '{id}' tidak ditemukan!");
            }

            _daftarTagihan.Remove(tagihan);
        }
    }
}