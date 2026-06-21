namespace MonTrack_PengingatTagihanGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblNama = new Label();
            lblKategori = new Label();
            lblDibuat = new Label();
            lblNominal = new Label();
            cbKategori = new ComboBox();
            gbInput = new GroupBox();
            dtpDIbuat = new DateTimePicker();
            txtNominal = new TextBox();
            txtNama = new TextBox();
            txtCari = new TextBox();
            dateTimePicker2 = new DateTimePicker();
            lblJatuhTempo = new Label();
            btnTambah = new Button();
            btnBatal = new Button();
            btnHapus = new Button();
            lblDaftarTagihan = new Label();
            lblCari = new Label();
            btnCari = new Button();
            btnLunas = new Button();
            dgvTagihan = new DataGridView();
            gbInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTagihan).BeginInit();
            SuspendLayout();
            // 
            // lblNama
            // 
            lblNama.AutoSize = true;
            lblNama.Location = new Point(38, 62);
            lblNama.Name = "lblNama";
            lblNama.Size = new Size(107, 20);
            lblNama.TabIndex = 0;
            lblNama.Text = "Nama Tagihan:";
            // 
            // lblKategori
            // 
            lblKategori.AutoSize = true;
            lblKategori.Location = new Point(38, 134);
            lblKategori.Name = "lblKategori";
            lblKategori.Size = new Size(66, 20);
            lblKategori.TabIndex = 1;
            lblKategori.Text = "Kategori";
            // 
            // lblDibuat
            // 
            lblDibuat.AutoSize = true;
            lblDibuat.Location = new Point(38, 285);
            lblDibuat.Name = "lblDibuat";
            lblDibuat.Size = new Size(168, 20);
            lblDibuat.TabIndex = 2;
            lblDibuat.Text = "Tanggal Tagihan Dibuat:";
            // 
            // lblNominal
            // 
            lblNominal.AutoSize = true;
            lblNominal.Location = new Point(38, 208);
            lblNominal.Name = "lblNominal";
            lblNominal.Size = new Size(69, 20);
            lblNominal.TabIndex = 3;
            lblNominal.Text = "Nominal:";
            // 
            // cbKategori
            // 
            cbKategori.DropDownStyle = ComboBoxStyle.DropDownList;
            cbKategori.FormattingEnabled = true;
            cbKategori.Location = new Point(38, 157);
            cbKategori.Name = "cbKategori";
            cbKategori.Size = new Size(219, 28);
            cbKategori.TabIndex = 4;
            // 
            // gbInput
            // 
            gbInput.Controls.Add(btnBatal);
            gbInput.Controls.Add(btnTambah);
            gbInput.Controls.Add(lblJatuhTempo);
            gbInput.Controls.Add(dateTimePicker2);
            gbInput.Controls.Add(lblNama);
            gbInput.Controls.Add(dtpDIbuat);
            gbInput.Controls.Add(txtNominal);
            gbInput.Controls.Add(lblDibuat);
            gbInput.Controls.Add(lblNominal);
            gbInput.Controls.Add(txtNama);
            gbInput.Controls.Add(cbKategori);
            gbInput.Controls.Add(lblKategori);
            gbInput.Dock = DockStyle.Left;
            gbInput.Location = new Point(0, 0);
            gbInput.Name = "gbInput";
            gbInput.Padding = new Padding(10);
            gbInput.Size = new Size(300, 553);
            gbInput.TabIndex = 5;
            gbInput.TabStop = false;
            gbInput.Text = "Kelola Tagihan";
            // 
            // dtpDIbuat
            // 
            dtpDIbuat.Format = DateTimePickerFormat.Short;
            dtpDIbuat.Location = new Point(38, 308);
            dtpDIbuat.Name = "dtpDIbuat";
            dtpDIbuat.Size = new Size(219, 27);
            dtpDIbuat.TabIndex = 9;
            dtpDIbuat.ValueChanged += dtpDibuat_ValueChanged;
            // 
            // txtNominal
            // 
            txtNominal.Location = new Point(38, 231);
            txtNominal.Name = "txtNominal";
            txtNominal.Size = new Size(219, 27);
            txtNominal.TabIndex = 6;
            txtNominal.TextAlign = HorizontalAlignment.Right;
            // 
            // txtNama
            // 
            txtNama.Location = new Point(38, 85);
            txtNama.Name = "txtNama";
            txtNama.Size = new Size(219, 27);
            txtNama.TabIndex = 7;
            // 
            // txtCari
            // 
            txtCari.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCari.Location = new Point(440, 52);
            txtCari.Name = "txtCari";
            txtCari.PlaceholderText = "Ketik nama atau kategori...";
            txtCari.Size = new Size(325, 27);
            txtCari.TabIndex = 8;
            txtCari.TextChanged += txtCari_TextChanged;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Location = new Point(37, 384);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(220, 27);
            dateTimePicker2.TabIndex = 10;
            // 
            // lblJatuhTempo
            // 
            lblJatuhTempo.AutoSize = true;
            lblJatuhTempo.Location = new Point(38, 361);
            lblJatuhTempo.Name = "lblJatuhTempo";
            lblJatuhTempo.Size = new Size(152, 20);
            lblJatuhTempo.TabIndex = 9;
            lblJatuhTempo.Text = "Tanggal Jatuh Tempo:";
            // 
            // btnTambah
            // 
            btnTambah.BackColor = Color.CornflowerBlue;
            btnTambah.FlatStyle = FlatStyle.Flat;
            btnTambah.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTambah.ForeColor = Color.White;
            btnTambah.Location = new Point(59, 436);
            btnTambah.Name = "btnTambah";
            btnTambah.Size = new Size(177, 36);
            btnTambah.TabIndex = 9;
            btnTambah.Text = "+ Tambah Tagihan";
            btnTambah.UseVisualStyleBackColor = false;
            btnTambah.Click += btnTambah_Click;
            // 
            // btnBatal
            // 
            btnBatal.BackColor = Color.Crimson;
            btnBatal.FlatStyle = FlatStyle.Flat;
            btnBatal.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBatal.ForeColor = Color.White;
            btnBatal.Location = new Point(99, 478);
            btnBatal.Name = "btnBatal";
            btnBatal.Size = new Size(94, 29);
            btnBatal.TabIndex = 9;
            btnBatal.Text = "Batal";
            btnBatal.UseVisualStyleBackColor = false;
            btnBatal.Click += btnBatal_Click;
            // 
            // btnHapus
            // 
            btnHapus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnHapus.BackColor = Color.Crimson;
            btnHapus.FlatStyle = FlatStyle.Flat;
            btnHapus.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHapus.ForeColor = Color.White;
            btnHapus.Location = new Point(790, 464);
            btnHapus.Name = "btnHapus";
            btnHapus.Size = new Size(99, 32);
            btnHapus.TabIndex = 9;
            btnHapus.Text = "Hapus";
            btnHapus.UseVisualStyleBackColor = false;
            btnHapus.Click += btnHapus_Click;
            // 
            // lblDaftarTagihan
            // 
            lblDaftarTagihan.AutoSize = true;
            lblDaftarTagihan.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDaftarTagihan.Location = new Point(316, 9);
            lblDaftarTagihan.Name = "lblDaftarTagihan";
            lblDaftarTagihan.Size = new Size(153, 28);
            lblDaftarTagihan.TabIndex = 10;
            lblDaftarTagihan.Text = "Daftar Tagihan";
            // 
            // lblCari
            // 
            lblCari.AutoSize = true;
            lblCari.Location = new Point(316, 55);
            lblCari.Name = "lblCari";
            lblCari.Size = new Size(118, 20);
            lblCari.TabIndex = 11;
            lblCari.Text = "🔍 Cari Tagihan:";
            // 
            // btnCari
            // 
            btnCari.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCari.Font = new Font("Segoe UI", 9F);
            btnCari.Location = new Point(771, 52);
            btnCari.Name = "btnCari";
            btnCari.Size = new Size(94, 29);
            btnCari.TabIndex = 12;
            btnCari.Text = "Search";
            btnCari.UseVisualStyleBackColor = true;
            // 
            // btnLunas
            // 
            btnLunas.BackColor = Color.SeaGreen;
            btnLunas.FlatStyle = FlatStyle.Flat;
            btnLunas.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLunas.ForeColor = Color.White;
            btnLunas.Location = new Point(397, 461);
            btnLunas.Name = "btnLunas";
            btnLunas.Size = new Size(202, 35);
            btnLunas.TabIndex = 13;
            btnLunas.Text = "Tandai Sudah Lunas ✅";
            btnLunas.UseVisualStyleBackColor = false;
            btnLunas.Click += btnLunas_Click;
            // 
            // dgvTagihan
            // 
            dgvTagihan.AllowUserToAddRows = false;
            dgvTagihan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvTagihan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTagihan.BackgroundColor = Color.White;
            dgvTagihan.BorderStyle = BorderStyle.Fixed3D;
            dgvTagihan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTagihan.Location = new Point(316, 98);
            dgvTagihan.Name = "dgvTagihan";
            dgvTagihan.ReadOnly = true;
            dgvTagihan.RowHeadersWidth = 51;
            dgvTagihan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTagihan.Size = new Size(653, 325);
            dgvTagihan.TabIndex = 14;
            dgvTagihan.CellClick += dgvTagihan_CellClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(982, 553);
            Controls.Add(dgvTagihan);
            Controls.Add(btnLunas);
            Controls.Add(btnCari);
            Controls.Add(lblCari);
            Controls.Add(lblDaftarTagihan);
            Controls.Add(btnHapus);
            Controls.Add(txtCari);
            Controls.Add(gbInput);
            MinimumSize = new Size(1000, 600);
            Name = "Form1";
            Text = "MonTrack - PengingatTagihan";
            Load += Form1_Load;
            gbInput.ResumeLayout(false);
            gbInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTagihan).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNama;
        private Label lblKategori;
        private Label lblDibuat;
        private Label lblNominal;
        private ComboBox cbKategori;
        private GroupBox gbInput;
        private TextBox txtNama;
        private TextBox txtNominal;
        private TextBox txtCari;
        private DateTimePicker dtpDIbuat;
        private DateTimePicker dateTimePicker2;
        private Label lblJatuhTempo;
        private Button btnTambah;
        private Button btnBatal;
        private Button btnHapus;
        private Label lblDaftarTagihan;
        private Label lblCari;
        private Button btnCari;
        private Button btnLunas;
        private DataGridView dgvTagihan;
    }
}
