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
            ((System.ComponentModel.ISupportInitialize)dgvRiwayat).BeginInit();
            SuspendLayout();
            // 
            // dgvRiwayat
            // 
            dgvRiwayat.AccessibleRole = AccessibleRole.None;
            dgvRiwayat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRiwayat.Location = new Point(62, 126);
            dgvRiwayat.Name = "dgvRiwayat";
            dgvRiwayat.RowHeadersWidth = 51;
            dgvRiwayat.Size = new Size(677, 188);
            dgvRiwayat.TabIndex = 0;
            // 
            // txtCari
            // 
            txtCari.Location = new Point(62, 93);
            txtCari.Name = "txtCari";
            txtCari.Size = new Size(577, 27);
            txtCari.TabIndex = 1;
            txtCari.TextChanged += txtCari_TextChanged;
            // 
            // btnCari
            // 
            btnCari.Location = new Point(645, 91);
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
            lblInfo.Location = new Point(62, 333);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(35, 20);
            lblInfo.TabIndex = 3;
            lblInfo.Text = "Info";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblInfo);
            Controls.Add(btnCari);
            Controls.Add(txtCari);
            Controls.Add(dgvRiwayat);
            Name = "MainForm";
            Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)dgvRiwayat).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvRiwayat;
        private TextBox txtCari;
        private Button btnCari;
        private Label lblInfo;
    }
}