using System;

namespace DaftarRiwayat_Transaksi10.Configs
{
    public class AppConfig
    {
        // mengatur mata uang default u
        public string DefaultCurrency { get; set; } = "IDR";
        // batas maksimal data yang ditampilkan di console
        public int MaxDisplayItems { get; set; } = 100;
        // urutan tampilan data (Ascending/Descending)
        public string SortOrder { get; set; } = "Descending";
    }
}
