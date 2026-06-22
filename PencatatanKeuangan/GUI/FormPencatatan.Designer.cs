namespace PencatatanKeuangan.GUI
{
    partial class FormPencatatan
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtNominal = new TextBox();
            txtDeskripsi = new TextBox();
            cmbKategori = new ComboBox();
            btnSimpan = new Button();
            btnKembali = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(80, 78);
            label1.Name = "label1";
            label1.Size = new Size(69, 20);
            label1.TabIndex = 0;
            label1.Text = "Nominal:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(80, 171);
            label2.Name = "label2";
            label2.Size = new Size(69, 20);
            label2.TabIndex = 1;
            label2.Text = "Kategori:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(80, 257);
            label3.Name = "label3";
            label3.Size = new Size(72, 20);
            label3.TabIndex = 2;
            label3.Text = "Deskripsi:";
            // 
            // txtNominal
            // 
            txtNominal.Location = new Point(88, 109);
            txtNominal.Name = "txtNominal";
            txtNominal.Size = new Size(125, 27);
            txtNominal.TabIndex = 3;
            // 
            // txtDeskripsi
            // 
            txtDeskripsi.Location = new Point(89, 285);
            txtDeskripsi.Name = "txtDeskripsi";
            txtDeskripsi.Size = new Size(125, 27);
            txtDeskripsi.TabIndex = 4;
            // 
            // cmbKategori
            // 
            cmbKategori.FormattingEnabled = true;
            cmbKategori.Location = new Point(88, 196);
            cmbKategori.Name = "cmbKategori";
            cmbKategori.Size = new Size(151, 28);
            cmbKategori.TabIndex = 5;
            // 
            // btnSimpan
            // 
            btnSimpan.Location = new Point(86, 373);
            btnSimpan.Name = "btnSimpan";
            btnSimpan.Size = new Size(94, 29);
            btnSimpan.TabIndex = 6;
            btnSimpan.Text = "Simpan";
            btnSimpan.UseVisualStyleBackColor = true;
            // 
            // btnKembali
            // 
            btnKembali.Location = new Point(289, 373);
            btnKembali.Name = "btnKembali";
            btnKembali.Size = new Size(94, 29);
            btnKembali.TabIndex = 7;
            btnKembali.Text = "Kembali";
            btnKembali.UseVisualStyleBackColor = true;
            // 
            // FormPencatatan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnKembali);
            Controls.Add(btnSimpan);
            Controls.Add(cmbKategori);
            Controls.Add(txtDeskripsi);
            Controls.Add(txtNominal);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "FormPencatatan";
            Text = "FormPencatatan";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtNominal;
        private TextBox txtDeskripsi;
        private ComboBox cmbKategori;
        private Button btnSimpan;
        private Button btnKembali;
    }
}