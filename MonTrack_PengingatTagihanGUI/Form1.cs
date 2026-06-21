using System;
using System.Linq;
using System.Windows.Forms;
using MonTrack_PengingatTagihan;

namespace MonTrack_PengingatTagihanGUI
{
    public partial class Form1 : Form
    {
        private TagihanManager _manager;
        private int? _idTagihanSedangDiedit = null;

        public Form1()
        {
            InitializeComponent();
            _manager = new TagihanManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetupAwal();
        }

        // --- Setup ---
        private void SetupAwal()
        {
            cbKategori.Items.Clear();
            cbKategori.Items.AddRange(new string[] {
                KategoriTagihan.Utilitas,
                KategoriTagihan.LayananDigital,
                KategoriTagihan.Pendidikan,
                KategoriTagihan.Finansial,
                KategoriTagihan.Asuransi
            });
            cbKategori.SelectedIndex = 0;

            BersihkanInput();

            btnBatal.Visible = false;

            RefreshTabel();
        }

        // --- Logika Fungsi ---
        private void RefreshTabel(string keyword = "")
        {
            dgvTagihan.DataSource = null;
            var data = _manager.GetSemuaTagihan();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                data = data.Where(t => t.Nama.ToLower().Contains(keyword) ||
                                     t.Kategori.ToLower().Contains(keyword)).ToList().AsReadOnly();
            }

            dgvTagihan.DataSource = data.ToList();

            // Format Rupiah
            if (dgvTagihan.Columns.Contains("Nominal"))
            {
                dgvTagihan.Columns["Nominal"].DefaultCellStyle.Format = "C0";
                dgvTagihan.Columns["Nominal"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("id-ID");
            }
            dgvTagihan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Untuk membersihkan input dan mengembalikan form ke state awal
        private void BersihkanInput()
        {
            txtNama.Clear();
            txtNominal.Clear();
            cbKategori.SelectedIndex = 0;
            dtpDibuat.Value = DateTime.Now;
            dtpJatuhTempo.Value = DateTime.Now.AddDays(30);

            _idTagihanSedangDiedit = null;
            btnBatal.Visible = false;
            btnTambah.Text = "+ Tambah Tagihan";
            btnTambah.BackColor = System.Drawing.Color.CornflowerBlue;
            txtNama.Focus();
        }

        // --- Event Handlers ---

        private void btnTambah_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtNominal.Text, out int nominal)) throw new Exception("Nominal harus angka!");

                if (_idTagihanSedangDiedit == null)
                {
                    _manager.CreateTagihan(txtNama.Text, cbKategori.Text, nominal, dtpDibuat.Value, dtpJatuhTempo.Value);
                }
                else
                {
                    _manager.UpdateTagihan(_idTagihanSedangDiedit.Value, txtNama.Text, cbKategori.Text, nominal, dtpDibuat.Value, dtpJatuhTempo.Value);
                }

                RefreshTabel();
                BersihkanInput();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dgvTagihan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvTagihan.Rows[e.RowIndex];
                _idTagihanSedangDiedit = (int)row.Cells["Id"].Value;

                txtNama.Text = row.Cells["Nama"].Value.ToString();
                cbKategori.Text = row.Cells["Kategori"].Value.ToString();
                txtNominal.Text = row.Cells["Nominal"].Value.ToString();
                dtpDibuat.Value = (DateTime)row.Cells["TanggalDibuat"].Value;
                dtpJatuhTempo.Value = (DateTime)row.Cells["TanggalJatuhTempo"].Value;

                btnBatal.Visible = true;
                btnTambah.Text = "💾 Simpan Edit";
                btnTambah.BackColor = System.Drawing.Color.Orange;
            }
        }

        private void btnBatal_Click(object sender, EventArgs e) { BersihkanInput(); }

        private void txtCari_TextChanged(object sender, EventArgs e) { RefreshTabel(txtCari.Text); }

        private void dtpDibuat_ValueChanged(object sender, EventArgs e)
        {
            if (_idTagihanSedangDiedit == null) dtpJatuhTempo.Value = dtpDibuat.Value.AddDays(30);
        }

        private void btnLunas_Click(object sender, EventArgs e)
        {
            if (dgvTagihan.CurrentRow == null) return;
            int id = (int)dgvTagihan.CurrentRow.Cells["Id"].Value;
            _manager.GetSemuaTagihan().First(t => t.Id == id).TandaiLunas();
            RefreshTabel();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvTagihan.CurrentRow == null) return;
            int id = (int)dgvTagihan.CurrentRow.Cells["Id"].Value;
            if (MessageBox.Show("Hapus?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _manager.DeleteTagihan(id);
                RefreshTabel();
                BersihkanInput();
            }
        }
    }
}