using System;

namespace DaftarRiwayat_Transaksi10.Configs
{
    public class AppConfig
    {
        public string DefaultCurrency { get; set; } = "IDR";
        public int MaxDisplayItems { get; set; } = 100;
        public string SortOrder { get; set; } = "Descending";
    }
}
