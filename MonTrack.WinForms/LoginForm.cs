using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonTrack.Auth.Api;

namespace MonTrack.WinForms
{
    public class LoginForm : Form
    {
        private AuthApiSimulator _authApi;
        private bool _isSignUpMode = false;

        // UI Controls
        private Panel cardPanel;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblEmail;
        private EntryTextBox txtEmail;
        private Label lblPassword;
        private EntryTextBox txtPassword;
        private Label lblConfirmPassword;
        private EntryTextBox txtConfirmPassword;
        private Button btnSubmit;
        private Label lblStatus;
        private Label lblFooterText;
        private Label lblFooterAction;

        public LoginForm()
        {
            _authApi = new AuthApiSimulator();

            // Seed users for testing
            try
            {
                _authApi.Register("user@test.com", "password123");
                _authApi.Register("admin1@gmail.com", "admin123");
            }
            catch { /* Ignore seed duplicates */ }

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Form Setup
            this.Size = new Size(420, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(26, 26, 46); // Dark background
            this.Text = "MonTrack - Authentication";

            // Title
            lblTitle = new Label
            {
                Text = "MonTrack",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                Location = new Point(30, 20),
                Size = new Size(340, 45),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Subtitle
            lblSubtitle = new Label
            {
                Text = "Your Premium Finance Companion",
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                Location = new Point(30, 65),
                Size = new Size(340, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Card Panel
            cardPanel = new Panel
            {
                BackColor = Color.FromArgb(34, 34, 59),
                Location = new Point(30, 105),
                Size = new Size(344, 250), // Will grow dynamically
                Padding = new Padding(20)
            };

            // Email Label
            lblEmail = new Label
            {
                Text = "Email Address",
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(20, 15),
                Size = new Size(300, 20)
            };

            // Email TextBox
            txtEmail = new EntryTextBox
            {
                Location = new Point(20, 35),
                Size = new Size(304, 28),
                PlaceholderText = "name@email.com"
            };

            // Password Label
            lblPassword = new Label
            {
                Text = "Password",
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(20, 75),
                Size = new Size(300, 20)
            };

            // Password TextBox
            txtPassword = new EntryTextBox
            {
                Location = new Point(20, 95),
                Size = new Size(304, 28),
                UseSystemPasswordChar = true,
                PlaceholderText = "••••••••"
            };

            // Confirm Password Label (Hidden by default)
            lblConfirmPassword = new Label
            {
                Text = "Confirm Password",
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(20, 135),
                Size = new Size(300, 20),
                Visible = false
            };

            // Confirm Password TextBox (Hidden by default)
            txtConfirmPassword = new EntryTextBox
            {
                Location = new Point(20, 155),
                Size = new Size(304, 28),
                UseSystemPasswordChar = true,
                PlaceholderText = "••••••••",
                Visible = false
            };

            // Submit Button
            btnSubmit = new Button
            {
                Text = "Login Now",
                BackColor = Color.FromArgb(78, 49, 170),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(20, 140), // Will adjust dynamically
                Size = new Size(304, 45),
                FlatStyle = FlatStyle.Flat
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.Click += BtnSubmit_Click;

            // Add Controls to Card Panel
            cardPanel.Controls.Add(lblEmail);
            cardPanel.Controls.Add(txtEmail);
            cardPanel.Controls.Add(lblPassword);
            cardPanel.Controls.Add(txtPassword);
            cardPanel.Controls.Add(lblConfirmPassword);
            cardPanel.Controls.Add(txtConfirmPassword);
            cardPanel.Controls.Add(btnSubmit);

            // Status Label
            lblStatus = new Label
            {
                ForeColor = Color.FromArgb(255, 118, 117),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(30, 365),
                Size = new Size(340, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Footer Stack Simulation
            lblFooterText = new Label
            {
                Text = "Don't have an account?",
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(30, 410),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };

            lblFooterAction = new Label
            {
                Text = "Sign Up",
                ForeColor = Color.FromArgb(55, 149, 189),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(235, 410),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            lblFooterAction.Click += ToggleMode_Click;

            // Add controls to Form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(cardPanel);
            this.Controls.Add(lblStatus);
            this.Controls.Add(lblFooterText);
            this.Controls.Add(lblFooterAction);

            // Set initial dynamic positions
            UpdateLayout();
        }

        private void ToggleMode_Click(object? sender, EventArgs e)
        {
            _isSignUpMode = !_isSignUpMode;
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            lblStatus.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";

            if (_isSignUpMode)
            {
                lblConfirmPassword.Visible = true;
                txtConfirmPassword.Visible = true;
                btnSubmit.Location = new Point(20, 205);
                btnSubmit.Text = "Sign Up Now";
                cardPanel.Size = new Size(344, 275);
                lblStatus.Location = new Point(30, 390);
                lblFooterText.Location = new Point(30, 430);
                lblFooterAction.Location = new Point(235, 430);
                this.Size = new Size(420, 540);
                lblFooterText.Text = "Already have an account?";
                lblFooterAction.Text = "Login";
            }
            else
            {
                lblConfirmPassword.Visible = false;
                txtConfirmPassword.Visible = false;
                btnSubmit.Location = new Point(20, 140);
                btnSubmit.Text = "Login Now";
                cardPanel.Size = new Size(344, 210);
                lblStatus.Location = new Point(30, 325);
                lblFooterText.Location = new Point(30, 370);
                lblFooterAction.Location = new Point(235, 370);
                this.Size = new Size(420, 480);
                lblFooterText.Text = "Don't have an account?";
                lblFooterAction.Text = "Sign Up";
            }
        }

        private async void BtnSubmit_Click(object? sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblStatus.Text = "Please fill in all fields.";
                return;
            }

            btnSubmit.Enabled = false;
            lblStatus.Text = "";

            if (_isSignUpMode)
            {
                string confirmPassword = txtConfirmPassword.Text;
                if (string.IsNullOrEmpty(confirmPassword))
                {
                    lblStatus.ForeColor = Color.FromArgb(255, 118, 117);
                    lblStatus.Text = "Please confirm your password.";
                    btnSubmit.Enabled = true;
                    return;
                }
                if (password != confirmPassword)
                {
                    lblStatus.ForeColor = Color.FromArgb(255, 118, 117);
                    lblStatus.Text = "Passwords do not match.";
                    btnSubmit.Enabled = true;
                    return;
                }

                btnSubmit.Text = "Registering...";

                var response = await Task.Run(() => _authApi.Register(email, password));

                if (response.IsSuccess)
                {
                    lblStatus.ForeColor = Color.FromArgb(85, 239, 196);
                    lblStatus.Text = "Registration successful! You can now login.";
                    _isSignUpMode = false;
                    UpdateLayout();
                }
                else
                {
                    lblStatus.ForeColor = Color.FromArgb(255, 118, 117);
                    lblStatus.Text = response.Message;
                }
                btnSubmit.Enabled = true;
            }
            else
            {
                btnSubmit.Text = "Authenticating...";

                var response = await Task.Run(() => _authApi.Login(email, password));

                if (response.IsSuccess)
                {
                    lblStatus.ForeColor = Color.FromArgb(85, 239, 196);
                    lblStatus.Text = "Access Granted!";
                    await Task.Delay(500);

                    // Open Dashboard
                    var mainForm = new MainForm(email);
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    lblStatus.ForeColor = Color.FromArgb(255, 118, 117);
                    lblStatus.Text = response.Message;
                    btnSubmit.Text = "Login Now";
                    btnSubmit.Enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// Custom Styled TextBox to look modern and flat.
    /// </summary>
    public class EntryTextBox : TextBox
    {
        public EntryTextBox()
        {
            this.BackColor = Color.FromArgb(22, 22, 37);
            this.ForeColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        }
    }
}
