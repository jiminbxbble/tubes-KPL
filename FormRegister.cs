using MonTrack.Auth.Services;
using MonTrack.Auth.StateMachine;
using MonTrack.Auth.States;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MonTrack.Auth.GUI
{
    public partial class FormRegister : Form
    {
        private AuthService _authService;

        public FormRegister()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Password tidak cocok!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var response = _authService.Register(txtEmail.Text, txtPassword.Text);

                if (response.IsSuccess)
                {
                    MessageBox.Show("Registrasi berhasil! Silakan login.", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(response.Message, "Registrasi Gagal",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)

        {

        }
    }
}