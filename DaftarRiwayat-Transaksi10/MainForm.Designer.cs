namespace DaftarRiwayat_Transaksi10
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvRiwayat = new DataGridView();
            txtCari = new TextBox();
            lblInfo = new Label();
            lblTotal = new Label();
            cmbTipe = new ComboBox();
            cmbKategori = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvRiwayat).BeginInit();
            SuspendLayout();
            // 
            // dgvRiwayat
            // 
            dgvRiwayat.AccessibleRole = AccessibleRole.None;
            dgvRiwayat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRiwayat.Location = new Point(12, 64);
            dgvRiwayat.Name = "dgvRiwayat";
            dgvRiwayat.RowHeadersWidth = 51;
            dgvRiwayat.Size = new Size(890, 302);
            dgvRiwayat.TabIndex = 0;
            // 
            // txtCari
            // 
            txtCari.ForeColor = SystemColors.ControlText;
            txtCari.Location = new Point(326, 31);
            txtCari.Name = "txtCari";
            txtCari.Size = new Size(576, 27);
            txtCari.TabIndex = 1;
            txtCari.TextChanged += txtCari_TextChanged;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(12, 389);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(35, 20);
            lblInfo.TabIndex = 3;
            lblInfo.Text = "Info";
            lblInfo.Click += lblInfo_Click;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(12, 369);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(50, 20);
            lblTotal.TabIndex = 4;
            lblTotal.Text = "label1";
            lblTotal.Click += lblTotal_Click;
            // 
            // cmbTipe
            // 
            cmbTipe.FormattingEnabled = true;
            cmbTipe.Location = new Point(12, 30);
            cmbTipe.Name = "cmbTipe";
            cmbTipe.Size = new Size(151, 28);
            cmbTipe.TabIndex = 5;
            cmbTipe.Text = "Tipe";
            // 
            // cmbKategori
            // 
            cmbKategori.FormattingEnabled = true;
            cmbKategori.Location = new Point(169, 31);
            cmbKategori.Name = "cmbKategori";
            cmbKategori.Size = new Size(151, 28);
            cmbKategori.TabIndex = 6;
            cmbKategori.Text = "Kategori";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 450);
            Controls.Add(cmbKategori);
            Controls.Add(cmbTipe);
            Controls.Add(lblTotal);
            Controls.Add(lblInfo);
            Controls.Add(txtCari);
            Controls.Add(dgvRiwayat);
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvRiwayat).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvRiwayat;
        private TextBox txtCari;
        private Label lblInfo;
        private Label lblTotal;
        private ComboBox cmbTipe;
        private ComboBox cmbKategori;
    }
}