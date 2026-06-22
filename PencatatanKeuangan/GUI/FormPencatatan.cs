using System;
using System.Windows.Forms;
using PencatatanKeuangan.Models;
using PencatatanKeuangan.Services;

namespace PencatatanKeuangan.GUI
{
    public partial class FormPencatatan : Form
    {
        private TransactionManager _manager;

        // Constructor untuk menerima manager
        public FormPencatatan(TransactionManager manager)
        {
            InitializeComponent();
            _manager = manager;
            SetupKategori();
        }

        private void SetupKategori()
        {
            cmbKategori.Items.Add("Uang Saku");
            cmbKategori.Items.Add("Gaji");
            cmbKategori.Items.Add("Makan");
            cmbKategori.Items.Add("Transport");
            cmbKategori.SelectedIndex = 0;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // SECURE CODING: Validasi Input
            if (!double.TryParse(txtNominal.Text, out double nominal))
            {
                MessageBox.Show("Nominal harus berupa angka!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string kategori = cmbKategori.SelectedItem?.ToString();
            string deskripsi = txtDeskripsi.Text;

            // EXCEPTION HANDLING: Menangkap error dari DbC
            try
            {
                _manager.RecordTransaction(nominal, kategori, deskripsi);
                MessageBox.Show("Transaksi berhasil dicatat!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtNominal.Clear();
                txtDeskripsi.Clear();
                cmbKategori.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close(); // Kembali ke form pemanggil
        }
    }
}