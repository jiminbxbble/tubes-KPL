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
    public partial class FormLogin : Form
    {
        private AuthService _authService;

        public FormLogin()
        {
            InitializeComponent();
            _authService = new AuthService();
        }



        private void btnLogin_Click(object sender, EventArgs e)
        {
            var context = new AuthContext
            {
                Email = txtEmail.Text,
                Password = txtPassword.Text
            };

            var machine = new AuthStateMachine(context);

            machine.Run();

            MessageBox.Show(context.Message);

            if (context.IsSuccess)
            {
                MessageBox.Show("Login Berhasil!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}