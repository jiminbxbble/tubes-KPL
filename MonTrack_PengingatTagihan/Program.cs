using MonTrack_PengingatTagihan;
using System;
using System.Globalization;
using System.Linq;

class Program
{
    static void Main()
    {
        TagihanManager manager = new TagihanManager();
        bool aplikasiJalan = true;

        while (aplikasiJalan)
        {
            Console.Clear();
            Console.WriteLine("=============================================");
            Console.WriteLine("      SISTEM PENGINGAT TAGIHAN MONTRACK      ");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. Lihat Daftar Tagihan");
            Console.WriteLine("2. Tambah Tagihan Baru (Create)");
            Console.WriteLine("3. Edit Tagihan (Update)");
            Console.WriteLine("4. Hapus Tagihan (Delete)");
            Console.WriteLine("5. Bayar Tagihan (Lunas)");
            Console.WriteLine("0. Keluar");
            Console.WriteLine("=============================================");
            Console.Write("Pilih menu (0-5): ");

            string pilihan = Console.ReadLine();
            Console.WriteLine();

            switch (pilihan)
            {
                case "1": // READ
                    TampilkanSemuaTagihan(manager);
                    break;

                case "2": // CREATE
                    Console.WriteLine("--- TAMBAH TAGIHAN ---");
                    Console.Write("Nama Tagihan : ");
                    string namaBaru = Console.ReadLine();

                    Console.WriteLine("Pilihan kategori: Utilitas, Layanan digital, Pendidikan, Finansial & Cicilan, Asuransi & Kesehatan");
                    Console.Write("Kategori : ");
                    string kategoriBaru = Console.ReadLine();

                    Console.Write("Nominal (Rp) : ");
                    if (!int.TryParse(Console.ReadLine(), out int nominalBaru))
                    {
                        Console.WriteLine("[ERROR] Nominal harus berupa angka valid!");
                    }
                    else
                    {
                        DateTime? tanggalDibuatRaw = InputTanggal("Tanggal Dibuat (Ketik (dd-MM-yyyy) / ENTER untuk hari ini): ");
                        DateTime tanggalDibuat = tanggalDibuatRaw ?? DateTime.Now;

                        DateTime? tanggalJatuhTempo = InputTanggal("Tanggal Jatuh Tempo (Ketik (dd-MM-yyyy) / ENTER untuk default 30 hari: ");

                        manager.CreateTagihan(namaBaru, kategoriBaru, nominalBaru, tanggalDibuat, tanggalJatuhTempo);
                    }
                    break;

                case "3": // UPDATE
                    Console.WriteLine("--- EDIT TAGIHAN ---");
                    TampilkanSemuaTagihan(manager);
                    Console.Write("Masukkan ID Tagihan yang mau diedit: ");

                    if (int.TryParse(Console.ReadLine(), out int idEdit))
                    {
                        Console.Write("Nama Baru : ");
                        string namaEdit = Console.ReadLine();

                        Console.WriteLine("Pilihan kategori: Utilitas, Layanan digital, Pendidikan, Finansial & Cicilan, Asuransi & Kesehatan");
                        Console.Write("Kategori Baru : ");
                        string kategoriEdit = Console.ReadLine();

                        Console.Write("Nominal Baru : ");

                        if (!int.TryParse(Console.ReadLine(), out int nominalEdit))
                        {
                            Console.WriteLine("[ERROR] Nominal harus berupa angka!");
                        }
                        else
                        {
                            DateTime? tanggalDibuatRaw = InputTanggal("Tanggal Dibuat (Ketik (dd-MM-yyyy) / ENTER untuk hari ini): ");
                            DateTime tanggalDibuat = tanggalDibuatRaw ?? DateTime.Now;

                            DateTime? tanggalJatuhTempo = InputTanggal("Tanggal Jatuh Tempo (Ketik (dd-MM-yyyy) / ENTER untuk default 30 hari: ");

                            manager.UpdateTagihan(idEdit, namaEdit, kategoriEdit, nominalEdit, tanggalDibuat, tanggalJatuhTempo);
                        }
                    }
                    else
                    {
                        Console.WriteLine("[ERROR] ID Tagihan harus berupa angka valid!");
                    }
                    break;

                case "4": // DELETE
                    Console.WriteLine("--- HAPUS TAGIHAN ---");
                    TampilkanSemuaTagihan(manager);
                    Console.Write("Masukkan ID Tagihan yang mau dihapus: ");
                    if (int.TryParse(Console.ReadLine(), out int idHapus))
                    {
                        manager.DeleteTagihan(idHapus);
                        Console.WriteLine("[SUKSES] Perintah hapus telah dieksekusi.");
                    }
                    else
                    {
                        Console.WriteLine("[ERROR] ID harus berupa angka!");
                    }
                    break;

                case "5": // LUNAS
                    Console.WriteLine("--- BAYAR TAGIHAN ---");
                    TampilkanSemuaTagihan(manager);
                    Console.Write("Masukkan ID Tagihan yang mau dibayar: ");

                    if (int.TryParse(Console.ReadLine(), out int idBayar))
                    {
                        var tagihanDibayar = manager.GetSemuaTagihan().FirstOrDefault(t => t.Id == idBayar);

                        if (tagihanDibayar != null)
                        {
                            tagihanDibayar.TandaiLunas();
                            Console.WriteLine($"[SUKSES] Tagihan {tagihanDibayar.Nama} berhasil dibayar!");
                        }
                        else
                        {
                            Console.WriteLine("[ERROR] ID Tagihan tidak ditemukan dalam daftar.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[ERROR] ID Tagihan harus berupa angka valid!");
                    }
                    break;

                case "0": // KELUAR
                    aplikasiJalan = false;
                    Console.WriteLine("Terima kasih telah menggunakan sistem ini!");
                    break;

                default:
                    Console.WriteLine("[ERROR] Pilihan menu tidak valid.");
                    break;
            }

            if (aplikasiJalan)
            {
                Console.WriteLine("\nTekan ENTER untuk kembali ke menu utama...");
                Console.ReadLine();
            }
        }
    }

    static void TampilkanSemuaTagihan(TagihanManager manager)
    {
        var daftar = manager.GetSemuaTagihan();
        if (daftar.Count == 0)
        {
            Console.WriteLine("[INFO] Belum ada tagihan yang dicatat.");
            return;
        }

        Console.WriteLine("ID | NAMA            | KATEGORI        | NOMINAL   | TANGGAL MULAI | TANGGAL JATUH TEMPO | STATUS");
        Console.WriteLine("-------------------------------------------------------------------------------------------------");
        foreach (var t in daftar)
        {
            Console.WriteLine($"{t.Id,-2} | {t.Nama,-15} | {t.Kategori,-15} | Rp{t.Nominal,-7} | {t.TanggalDibuat.ToShortDateString(),-10} | {t.TanggalJatuhTempo.ToShortDateString(),-11} | {t.StatusSaatIni}");
        }
        Console.WriteLine("-----------------------------------------------------------------------------------------------\n");
    }

    // Defensive Programming untuk memastikan input tanggal valid
    static DateTime? InputTanggal(string pesanPrompt)
    {
        DateTime tanggalValid;

        string[] formatTanggal = { "dd-MM-yyyy", "dd/MM/yyyy", "dd MM yyyy" };

        while (true)
        {
            Console.Write(pesanPrompt);
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            // Inputan user akan dicocokkan dengan format tanggal yang telah dibuat,
            // kemudian akan di try-parse menggunakan DateTime.TryParseExact untuk memastikan formatnya benar.
            if (DateTime.TryParseExact(input, formatTanggal, CultureInfo.InvariantCulture,
                DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out tanggalValid))
            {
                return tanggalValid;
            }
            else
            {
                Console.WriteLine("    [ERROR] Format tanggal salah! Gunakan format Hari-Bulan-Tahun.");
            }
        }
    }
}