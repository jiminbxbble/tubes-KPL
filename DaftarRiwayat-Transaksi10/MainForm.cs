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
        // panggil class manager dan config
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
            int limit = (_config != null && _config.MaxDisplayItems > 0) ? _config.MaxDisplayItems : 10000;
            var dataYangDitampilkan = data.Take(limit).ToList();

            dgvRiwayat.DataSource = dataYangDitampilkan;

            // format angka ribuan dengan "N0" untuk memisah ribuan 
            dgvRiwayat.Columns["Amount"].DefaultCellStyle.Format = "N0";

            // kalkulasi pengeluaran dengan LINQ dari data pada di tabel 
            decimal totalPengeluaran = (decimal)dataYangDitampilkan.Sum(t => t.Amount);

            // tampilan label sesuai format Config JSON
            string mataUang = _config != null ? _config.DefaultCurrency : "IDR";
            lblTotal.Text = $"Total: {mataUang} {totalPengeluaran:N0}";

            // styling tabel
            dgvRiwayat.RowHeadersVisible = false;
            dgvRiwayat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRiwayat.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // load Runtime Config
            _config = ConfigService.LoadConfig();
            lblInfo.Text = $"Currency: {_config.DefaultCurrency} | Limit: {_config.MaxDisplayItems}";

            // Seed Data
            // pemasukan
            _manager.AddItem(new Transaction(1, 2000000, "Gaji", DateTime.Now.AddDays(-15), TransactionType.Pemasukan, "Kiriman bulanan orang tua"));
            _manager.AddItem(new Transaction(2, 1500000, "Proyek", DateTime.Now.AddDays(-12), TransactionType.Pemasukan, "Dana PKM-KC cair"));
            _manager.AddItem(new Transaction(3, 500000, "Proyek", DateTime.Now.AddDays(-10), TransactionType.Pemasukan, "DP project web RuangRintis"));
            _manager.AddItem(new Transaction(4, 750000, "Gaji", DateTime.Now.AddDays(-5), TransactionType.Pemasukan, "Honor freelance UI/UX HydroPoMe"));

            // pengeluaran
            // makanan
            _manager.AddItem(new Transaction(1, 15000, "Makanan", DateTime.Now.AddDays(-60), TransactionType.Pengeluaran, "Makan siang Warkop Dayeuhkolot"));
            _manager.AddItem(new Transaction(2, 22000, "Makanan", DateTime.Now.AddDays(-59), TransactionType.Pengeluaran, "Kopi lembur tugas KPL"));
            _manager.AddItem(new Transaction(3, 18000, "Makanan", DateTime.Now.AddDays(-57), TransactionType.Pengeluaran, "Nasi goreng depan kosan"));
            _manager.AddItem(new Transaction(4, 35000, "Makanan", DateTime.Now.AddDays(-55), TransactionType.Pengeluaran, "Snack rapat tim DarahCepat"));
            _manager.AddItem(new Transaction(5, 20000, "Makanan", DateTime.Now.AddDays(-53), TransactionType.Pengeluaran, "Makan siang Kantin Telkom University"));
            _manager.AddItem(new Transaction(6, 25000, "Makanan", DateTime.Now.AddDays(-52), TransactionType.Pengeluaran, "Pecel lele malam"));
            _manager.AddItem(new Transaction(7, 15000, "Makanan", DateTime.Now.AddDays(-50), TransactionType.Pengeluaran, "Es kopi susu"));
            _manager.AddItem(new Transaction(8, 40000, "Makanan", DateTime.Now.AddDays(-48), TransactionType.Pengeluaran, "Makan malam bareng teman"));
            _manager.AddItem(new Transaction(9, 12000, "Makanan", DateTime.Now.AddDays(-47), TransactionType.Pengeluaran, "Roti bakar"));
            _manager.AddItem(new Transaction(10, 22000, "Makanan", DateTime.Now.AddDays(-45), TransactionType.Pengeluaran, "Beli air galon kosan"));
            _manager.AddItem(new Transaction(11, 18000, "Makanan", DateTime.Now.AddDays(-42), TransactionType.Pengeluaran, "Seblak"));
            _manager.AddItem(new Transaction(12, 45000, "Makanan", DateTime.Now.AddDays(-40), TransactionType.Pengeluaran, "Makan siang di Bekasi"));
            _manager.AddItem(new Transaction(13, 30000, "Makanan", DateTime.Now.AddDays(-38), TransactionType.Pengeluaran, "Martabak manis"));
            

            // tagihan
            _manager.AddItem(new Transaction(26, 300000, "Tagihan", DateTime.Now.AddDays(-58), TransactionType.Pengeluaran, "Langganan n8n pro bulanan"));
            _manager.AddItem(new Transaction(27, 150000, "Tagihan", DateTime.Now.AddDays(-55), TransactionType.Pengeluaran, "Kuota internet bulanan"));
            _manager.AddItem(new Transaction(28, 100000, "Tagihan", DateTime.Now.AddDays(-45), TransactionType.Pengeluaran, "Token listrik kosan"));
            _manager.AddItem(new Transaction(29, 30000, "Tagihan", DateTime.Now.AddDays(-35), TransactionType.Pengeluaran, "Tagihan air"));
            _manager.AddItem(new Transaction(30, 200000, "Tagihan", DateTime.Now.AddDays(-25), TransactionType.Pengeluaran, "Sewa Server RuangRintis"));
            _manager.AddItem(new Transaction(31, 35000, "Tagihan", DateTime.Now.AddDays(-20), TransactionType.Pengeluaran, "BPJS Kesehatan"));
            _manager.AddItem(new Transaction(32, 250000, "Tagihan", DateTime.Now.AddDays(-15), TransactionType.Pengeluaran, "Tagihan internet rumah"));
            _manager.AddItem(new Transaction(33, 150000, "Tagihan", DateTime.Now.AddDays(-10), TransactionType.Pengeluaran, "Biaya langganan cloud hosting"));
            _manager.AddItem(new Transaction(34, 100000, "Tagihan", DateTime.Now.AddDays(-5), TransactionType.Pengeluaran, "Tagihan pascabayar HP"));
            _manager.AddItem(new Transaction(35, 50000, "Tagihan", DateTime.Now.AddDays(-1), TransactionType.Pengeluaran, "Service charge kebersihan kos"));

            // cicilan
            _manager.AddItem(new Transaction(36, 500000, "Cicilan", DateTime.Now.AddDays(-56), TransactionType.Pengeluaran, "Cicilan laptop coding"));
            _manager.AddItem(new Transaction(37, 850000, "Cicilan", DateTime.Now.AddDays(-51), TransactionType.Pengeluaran, "Cicilan motor Honda"));
            _manager.AddItem(new Transaction(38, 1000000, "Cicilan", DateTime.Now.AddDays(-41), TransactionType.Pengeluaran, "Cicilan UKT semester 4"));
            _manager.AddItem(new Transaction(39, 300000, "Cicilan", DateTime.Now.AddDays(-31), TransactionType.Pengeluaran, "Cicilan kelas UI/UX"));
            _manager.AddItem(new Transaction(40, 400000, "Cicilan", DateTime.Now.AddDays(-21), TransactionType.Pengeluaran, "Cicilan Smartphone"));
            _manager.AddItem(new Transaction(41, 150000, "Cicilan", DateTime.Now.AddDays(-16), TransactionType.Pengeluaran, "Cicilan monitor eksternal"));
            _manager.AddItem(new Transaction(42, 250000, "Cicilan", DateTime.Now.AddDays(-11), TransactionType.Pengeluaran, "Cicilan kursi ergonomis"));
            _manager.AddItem(new Transaction(43, 200000, "Cicilan", DateTime.Now.AddDays(-8), TransactionType.Pengeluaran, "Cicilan meja kerja"));
            _manager.AddItem(new Transaction(44, 300000, "Cicilan", DateTime.Now.AddDays(-4), TransactionType.Pengeluaran, "Cicilan perangkat jaringan"));
            _manager.AddItem(new Transaction(45, 120000, "Cicilan", DateTime.Now.AddDays(-1), TransactionType.Pengeluaran, "Cicilan sepatu lari"));

            // belanja
            _manager.AddItem(new Transaction(46, 450000, "Belanja", DateTime.Now.AddDays(-54), TransactionType.Pengeluaran, "Baterai iPhone 13 original (Toko Bekasi)"));
            _manager.AddItem(new Transaction(47, 85000, "Belanja", DateTime.Now.AddDays(-49), TransactionType.Pengeluaran, "Buku Rekayasa Perangkat Lunak"));
            _manager.AddItem(new Transaction(48, 150000, "Belanja", DateTime.Now.AddDays(-46), TransactionType.Pengeluaran, "Kado ulang tahun sepupu cewe"));
            _manager.AddItem(new Transaction(49, 50000, "Belanja", DateTime.Now.AddDays(-43), TransactionType.Pengeluaran, "Strap Apple Watch"));
            _manager.AddItem(new Transaction(50, 175000, "Belanja", DateTime.Now.AddDays(-39), TransactionType.Pengeluaran, "Mouse wireless"));
            _manager.AddItem(new Transaction(51, 350000, "Belanja", DateTime.Now.AddDays(-36), TransactionType.Pengeluaran, "Keyboard mekanik"));
            _manager.AddItem(new Transaction(52, 120000, "Belanja", DateTime.Now.AddDays(-33), TransactionType.Pengeluaran, "Baju kaos kampus"));
            _manager.AddItem(new Transaction(53, 150000, "Belanja", DateTime.Now.AddDays(-29), TransactionType.Pengeluaran, "Celana panjang"));
            _manager.AddItem(new Transaction(54, 200000, "Belanja", DateTime.Now.AddDays(-26), TransactionType.Pengeluaran, "Jaket lari"));
            _manager.AddItem(new Transaction(55, 45000, "Belanja", DateTime.Now.AddDays(-23), TransactionType.Pengeluaran, "Sabun mandi dan sampo"));
            _manager.AddItem(new Transaction(56, 75000, "Belanja", DateTime.Now.AddDays(-19), TransactionType.Pengeluaran, "Skincare bulanan"));
            _manager.AddItem(new Transaction(57, 180000, "Belanja", DateTime.Now.AddDays(-17), TransactionType.Pengeluaran, "Kemeja untuk presentasi PIMNAS"));
            _manager.AddItem(new Transaction(58, 450000, "Belanja", DateTime.Now.AddDays(-14), TransactionType.Pengeluaran, "Sepatu sneakers"));
           

            // transportasi
            _manager.AddItem(new Transaction(66, 15000, "Transportasi", DateTime.Now.AddDays(-58), TransactionType.Pengeluaran, "Gojek ke Telkom University"));
            _manager.AddItem(new Transaction(67, 35000, "Transportasi", DateTime.Now.AddDays(-54), TransactionType.Pengeluaran, "GrabCar ke stasiun"));
            _manager.AddItem(new Transaction(68, 30000, "Transportasi", DateTime.Now.AddDays(-49), TransactionType.Pengeluaran, "Isi bensin motor"));
            _manager.AddItem(new Transaction(69, 150000, "Transportasi", DateTime.Now.AddDays(-44), TransactionType.Pengeluaran, "Tiket kereta Bandung-Bekasi"));
            _manager.AddItem(new Transaction(70, 5000, "Transportasi", DateTime.Now.AddDays(-40), TransactionType.Pengeluaran, "Angkot ke pasar"));
            _manager.AddItem(new Transaction(71, 3000, "Transportasi", DateTime.Now.AddDays(-37), TransactionType.Pengeluaran, "Parkir motor kampus"));
            _manager.AddItem(new Transaction(72, 15000, "Transportasi", DateTime.Now.AddDays(-34), TransactionType.Pengeluaran, "Tambal ban motor"));
            _manager.AddItem(new Transaction(73, 65000, "Transportasi", DateTime.Now.AddDays(-29), TransactionType.Pengeluaran, "Ganti oli motor rutin"));
            _manager.AddItem(new Transaction(74, 120000, "Transportasi", DateTime.Now.AddDays(-26), TransactionType.Pengeluaran, "Tiket travel ke Bekasi"));
            _manager.AddItem(new Transaction(75, 12000, "Transportasi", DateTime.Now.AddDays(-22), TransactionType.Pengeluaran, "GoRide ke Warkop Dayeuhkolot"));
            _manager.AddItem(new Transaction(76, 5000, "Transportasi", DateTime.Now.AddDays(-19), TransactionType.Pengeluaran, "Parkir mall"));
            _manager.AddItem(new Transaction(77, 15000, "Transportasi", DateTime.Now.AddDays(-16), TransactionType.Pengeluaran, "Cuci motor"));
           

            // hiburan
            _manager.AddItem(new Transaction(86, 250000, "Hiburan", DateTime.Now.AddDays(-59), TransactionType.Pengeluaran, "Top up Diamond MLBB (Beli hero Granger)"));
            _manager.AddItem(new Transaction(87, 120000, "Hiburan", DateTime.Now.AddDays(-53), TransactionType.Pengeluaran, "Langganan streaming UCL City vs Madrid"));
            _manager.AddItem(new Transaction(88, 75000, "Hiburan", DateTime.Now.AddDays(-47), TransactionType.Pengeluaran, "Nonton bioskop film Harry Potter"));
            _manager.AddItem(new Transaction(89, 40000, "Hiburan", DateTime.Now.AddDays(-42), TransactionType.Pengeluaran, "Patungan sewa lapangan lari"));
            _manager.AddItem(new Transaction(90, 150000, "Hiburan", DateTime.Now.AddDays(-37), TransactionType.Pengeluaran, "Beli skin Epic Irithel MLBB"));
            _manager.AddItem(new Transaction(91, 50000, "Hiburan", DateTime.Now.AddDays(-30), TransactionType.Pengeluaran, "Tiket masuk kolam renang"));
            _manager.AddItem(new Transaction(92, 55000, "Hiburan", DateTime.Now.AddDays(-27), TransactionType.Pengeluaran, "Langganan Spotify Premium"));
            _manager.AddItem(new Transaction(93, 350000, "Hiburan", DateTime.Now.AddDays(-24), TransactionType.Pengeluaran, "Tiket nonton konser musik"));
            _manager.AddItem(new Transaction(94, 75000, "Hiburan", DateTime.Now.AddDays(-20), TransactionType.Pengeluaran, "Main billiard bareng teman"));
            

            ApplyFilters();

            // dropbox tipe
            cmbTipe.Items.Clear();
            cmbTipe.Items.Add("Semua Tipe");
            cmbTipe.Items.Add("Pemasukan");
            cmbTipe.Items.Add("Pengeluaran");
            cmbTipe.SelectedIndex = 0;

            // dropbox kategori 
            cmbKategori.Items.Clear();
            cmbKategori.Items.Add("Semua Kategori");
            cmbKategori.Items.Add("Gaji");
            cmbKategori.Items.Add("Proyek");
            cmbKategori.Items.Add("Hiburan");
            cmbKategori.Items.Add("Makanan");
            cmbKategori.Items.Add("Tagihan");
            cmbKategori.Items.Add("Cicilan");
            cmbKategori.Items.Add("Belanja");
            cmbKategori.Items.Add("Transportasi");
            cmbKategori.Items.Add("Hiburan");
            cmbKategori.SelectedIndex = 0;

            cmbTipe.SelectedIndexChanged += cmbTipe_SelectedIndexChanged;
            cmbKategori.SelectedIndexChanged += cmbKategori_SelectedIndexChanged;

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            // ambil semua data dari manager
            var seluruhData = _manager.FilterItems(t => true);

            // filter tipe
            if (cmbTipe.SelectedIndex == 1) // pemasukan
            {
                seluruhData = seluruhData.Where(t => t.Type == TransactionType.Pemasukan).ToList();
            }
            else if (cmbTipe.SelectedIndex == 2) // pengeluaran
            {
                seluruhData = seluruhData.Where(t => t.Type == TransactionType.Pengeluaran).ToList();
            }

            // filter kategori
            if (cmbKategori.SelectedIndex > 0)
            {
                string kategoriDipilih = cmbKategori.SelectedItem.ToString();
                seluruhData = seluruhData.Where(t => t.Category.Equals(kategoriDipilih, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // filter keyword
            if (!string.IsNullOrWhiteSpace(txtCari.Text))
            {
                string keyword = txtCari.Text.ToLower();


                seluruhData = seluruhData.Where(t => t.Description.ToLower().Contains(keyword)).ToList();
            }

            // kirim data hasil saringan
            RefreshTabel(seluruhData);
        }

        private void cmbTipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters(); 
        }

        private void cmbKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters(); 
        }

        private void lblInfo_Click(object sender, EventArgs e)
        {

        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
    }
}
