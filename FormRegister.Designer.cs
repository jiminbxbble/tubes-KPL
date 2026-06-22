namespace MonTrack.Auth.GUI
{
    partial class FormRegister
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
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            textBox3 = new TextBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(250, 60);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(281, 31);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(250, 119);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(281, 31);
            textBox2.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(331, 295);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 2;
            button1.Text = "Register";
            button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(250, 32);
            label1.Name = "label1";
            label1.Size = new Size(54, 25);
            label1.TabIndex = 3;
            label1.Text = "Email";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(250, 94);
            label2.Name = "label2";
            label2.Size = new Size(87, 25);
            label2.TabIndex = 4;
            label2.Text = "Password";
            label2.Click += label2_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(250, 178);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(281, 31);
            textBox3.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(250, 153);
            label3.Name = "label3";
            label3.Size = new Size(176, 25);
            label3.TabIndex = 6;
            label3.Text = "Konfirmasi Password";
            // 
            // FormRegister
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(textBox3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "FormRegister";
            Text = "FormRegister";
            Load += FormRegister_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private Label label1;
        private Label label2;
        private TextBox textBox3;
        private Label label3;
    }
}