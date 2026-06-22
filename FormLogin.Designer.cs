namespace MonTrack.Auth.GUI
{
    partial class FormLogin
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
            txtPassword = new TextBox();
            btnLogin = new Button();
            txtEmail = new TextBox();
            labelEmail = new Label();
            labelPassword = new Label();
            SuspendLayout();
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(241, 117);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(265, 31);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(302, 179);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(112, 34);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(241, 51);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(265, 31);
            txtEmail.TabIndex = 0;
            txtEmail.TextChanged += textBox1_TextChanged;
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(241, 23);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(54, 25);
            labelEmail.TabIndex = 5;
            labelEmail.Text = "Email";
            labelEmail.Click += label1_Click;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(241, 89);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(87, 25);
            labelPassword.TabIndex = 6;
            labelPassword.Text = "Password";
            labelPassword.Click += label2_Click;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(labelPassword);
            Controls.Add(labelEmail);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(txtEmail);
            Name = "FormLogin";
            Text = "FormLogin";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtPassword;
        private Button btnLogin;
        private TextBox txtEmail;
        private Label labelEmail;
        private Label labelPassword;
    }
}