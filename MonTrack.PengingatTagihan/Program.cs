using System;
using System.Collections.Generic;
using System.Linq;
using MonTrack_PengingatTagihan;

class Program
{
    static void Main()
    {
        List<PengingatTagihan> semuaTagihan = new List<PengingatTagihan>
        {
            new PengingatTagihan("Langganan Biznet", "Internet", 300000, DateTime.Now.AddDays(-35)),
            new PengingatTagihan("PDAM Bulan Ini", "Air", 50000, DateTime.Now.AddDays(-5)),
            new PengingatTagihan("Token PLN", "Listrik", 150000, DateTime.Now.AddDays(-10)),
            new PengingatTagihan("Kost Bulanan", "Sewa Rumah", 2000000, DateTime.Now.AddDays(-8)),
            new PengingatTagihan("Netflix Premium", "Netflix", 75000, DateTime.Now)
        };

        Console.WriteLine("============= Daftar Tagihan Pribadi =============");
        Console.WriteLine("\n[!] DAFTAR TANGGUNGAN (BELUM BAYAR):");

        foreach (var t in semuaTagihan.Where(x => x.StatusSaatIni != PengingatTagihan.TagihanState.Lunas))
        {
            t.UpdateStatusBerdasarkanWaktu();
            Console.WriteLine($"- {t.Nama} | Kategori: {t.Kategori} ({t.Kelompok})");
            Console.WriteLine($"  Rp {t.Nominal} | Status: {t.StatusSaatIni} | Deadline: {t.Deadline.ToShortDateString()}\n");
        }

        Console.WriteLine("==================================================");

        Console.WriteLine("============= Proses Penyelesaian Tagihan =============");
        semuaTagihan[1].TandaiLunas();

        Console.WriteLine("============= Daftar Tagihan Telah Lunas =============");
        var daftarLunas = semuaTagihan.Where(x => x.StatusSaatIni == PengingatTagihan.TagihanState.Lunas).ToList();

        if (daftarLunas.Count == 0)
        {
            Console.WriteLine("Belum ada tagihan yang ditandai lunas.");
        }
        else
        {
            foreach (var lunas in daftarLunas)
            {
                Console.WriteLine($"- {lunas.Nama} | Status: Selesai Dicatat | Tanggal Selesai: {DateTime.Now.ToShortDateString()}");
            }
        }

        Console.WriteLine("\n==================================================");
        Console.WriteLine("Tetap disiplin mengatur keuangan, ya!");
    }
}