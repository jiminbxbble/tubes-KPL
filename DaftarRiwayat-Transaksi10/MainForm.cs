using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using DaftarRiwayat_Transaksi10.Models;
using DaftarRiwayat_Transaksi10.Services;
using DaftarRiwayat_Transaksi10.Configs;

namespace DaftarRiwayat_Transaksi10
{
    public partial class MainForm : Form
    {
        // Panggil class manager dan config
        private RiwayatManager<Transaction> _manager;
        private AppConfig _config;

        public MainForm()
        {
            InitializeComponent();
            _manager = new RiwayatManager<Transaction>();
        }

        private void RefreshTabel(System.Collections.Generic.List<Transaction> data)
        {
            dgvRiwayat.DataSource = null;

            int limit = (_config != null && _config.MaxDisplayItems > 0) ? _config.MaxDisplayItems : 100;

            dgvRiwayat.DataSource = data.Take(limit).ToList();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load Runtime Config
            _config = ConfigService.LoadConfig();
            lblInfo.Text = $"Currency: {_config.DefaultCurrency} | Limit: {_config.MaxDisplayItems}";

            // Seed Data
            _manager.AddItem(new Transaction(1, 15000, "Makanan", DateTime.Now.AddDays(-10), "Makan Siang Warkop Dayeuhkolot"));
            _manager.AddItem(new Transaction(2, 250000, "Hiburan", DateTime.Now.AddDays(-9), "Top up Diamond MLBB (Beli Skin Lejen)"));
            _manager.AddItem(new Transaction(3, 350000, "Pendidikan", DateTime.Now.AddDays(-9), "Langganan n8n pro untuk project"));
            _manager.AddItem(new Transaction(4, 50000, "Transportasi", DateTime.Now.AddDays(-8), "GrabCar ke Telkom University"));
            _manager.AddItem(new Transaction(5, 450000, "Gadget", DateTime.Now.AddDays(-7), "Beli baterai iPhone 13 (Toko Bekasi)"));
            _manager.AddItem(new Transaction(6, 25000, "Makanan", DateTime.Now.AddDays(-6), "Beli snack untuk Hackathon"));
            _manager.AddItem(new Transaction(7, 40000, "Olahraga", DateTime.Now.AddDays(-5), "Patungan sewa lapangan lari"));
            _manager.AddItem(new Transaction(8, 120000, "Hiburan", DateTime.Now.AddDays(-4), "Langganan streaming UCL City vs Madrid"));
            _manager.AddItem(new Transaction(9, 18000, "Makanan", DateTime.Now.AddDays(-3), "Beli kopi lembur nugas KPL"));
            _manager.AddItem(new Transaction(10, 150000, "Sosial", DateTime.Now.AddDays(-3), "Beli kado ulang tahun sepupu"));
            _manager.AddItem(new Transaction(11, 20000, "Pendidikan", DateTime.Now.AddDays(-2), "Print poster presentasi kampus"));
            _manager.AddItem(new Transaction(12, 175000, "Gadget", DateTime.Now.AddDays(-2), "Beli mouse wireless"));
            _manager.AddItem(new Transaction(13, 30000, "Kebutuhan", DateTime.Now.AddDays(-1), "Laundry baju kiloan"));
            _manager.AddItem(new Transaction(14, 55000, "Hiburan", DateTime.Now.AddDays(-1), "Nonton bioskop"));
            _manager.AddItem(new Transaction(15, 100000, "Kebutuhan", DateTime.Now, "Beli kuota internet bulanan"));
            _manager.AddItem(new Transaction(16, 20000, "Sosial", DateTime.Now, "Bayar uang kas kelas RPL"));
            _manager.AddItem(new Transaction(17, 85000, "Pendidikan", DateTime.Now, "Beli buku Clean Architecture"));
            _manager.AddItem(new Transaction(18, 1800000, "Transportasi", DateTime.Now, "BORE UP SPEK 65"));
            _manager.AddItem(new Transaction(19, 12000, "Makanan", DateTime.Now, "Makan malam Pecel Lele"));
            _manager.AddItem(new Transaction(20, 15000, "Makanan", DateTime.Now, "Makan siang di kantin kampus"));

            // Tampilkan data ke tabel
            RefreshTabel(_manager.FilterItems(t => true));

            
        }
    }
}
