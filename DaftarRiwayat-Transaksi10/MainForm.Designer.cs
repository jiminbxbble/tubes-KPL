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
            btnCari = new Button();
            lblInfo = new Label();
            lblTotal = new Label();
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
            dgvRiwayat.Size = new Size(776, 302);
            dgvRiwayat.TabIndex = 0;
            // 
            // txtCari
            // 
            txtCari.Location = new Point(12, 31);
            txtCari.Name = "txtCari";
            txtCari.Size = new Size(676, 27);
            txtCari.TabIndex = 1;
            // 
            // btnCari
            // 
            btnCari.Location = new Point(694, 29);
            btnCari.Name = "btnCari";
            btnCari.Size = new Size(94, 29);
            btnCari.TabIndex = 2;
            btnCari.Text = "Cari";
            btnCari.UseVisualStyleBackColor = true;
            btnCari.Click += btnCari_Click;
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
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblTotal);
            Controls.Add(lblInfo);
            Controls.Add(btnCari);
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
        private Button btnCari;
        private Label lblInfo;
        private Label lblTotal;
    }
}