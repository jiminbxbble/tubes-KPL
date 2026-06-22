using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PencatatanKeuangan.Models;
using PencatatanKeuangan.Repositories;
using PencatatanKeuangan.Services;
using MonTrack.Services;
using MonTrack_PengingatTagihan;

namespace MonTrack.WinForms
{
    public class MainForm : Form
    {
        // Services
        private DataRepository<Transaction> _repo;
        private TransactionManager _financeManager;
        private ExportApiService _exportService;
        private List<PengingatTagihan> _reminders;

        private enum FilterType
        {
            All,
            Pemasukan,
            Pengeluaran
        }

        private FilterType _currentFilter = FilterType.All;

        // UI Controls
        private Panel headerPanel;
        private Label lblHeaderTitle;
        private Label lblHeaderSubtitle;
        private Label lblBalance;
        private TabControl tabControl;
        private Panel inputPanel;
        private Panel filterPanel;
        private string _userEmail = string.Empty;
        private Label lblType;
        private ComboBox cmbType;
        private TabPage tabReports;
        private Panel pnlChart;

        // Tab 1: Transactions
        private TabPage tabTransactions;
        private ListView lvTransactions;
        private Label lblNewTxTitle;
        private Label lblAmount;
        private EntryTextBox txtAmount;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblDescription;
        private EntryTextBox txtDescription;
        private Button btnRecord;
        private Label lblTxStatus;

        // Tab 2: Bill Reminders
        private TabPage tabReminders;
        private ListView lvReminders;
        private Button btnMarkPaid;
        private Label lblReminderStatus;

        // Tab 3: Data Export
        private TabPage tabExport;
        private Panel exportCard;
        private Label lblExportTitle;
        private Label lblExportFormat;
        private ComboBox cmbExportFormat;
        private Button btnExport;
        private Label lblExportStatus;

        /// <summary>
        /// Konstruktor utama MainForm.
        /// Menerapkan Pemisahan Berkas per Pengguna (Multi-User Isolation) untuk privasi data dan keamanan.
        /// Mengapa: Agar setiap pengguna memiliki berkas transaksi terisolasi sendiri (transactions_[email].json)
        /// dan tidak terjadi kebocoran data atau tabrakan data (collision) antar akun yang berbeda pada sistem yang sama.
        /// </summary>
        /// <param name="userEmail">Email pengguna yang berhasil login dari LoginForm.</param>
        public MainForm(string userEmail = "")
        {
            _userEmail = string.IsNullOrEmpty(userEmail) ? "default" : userEmail;
            
            // Mengapa karakter non-alfanumerik disanitasi:
            // Mencegah error penulisan nama file di sistem operasi Windows (karena simbol '@' dan '.' 
            // dapat memicu masalah keamanan path traversal atau ketidakkompatibilitas sistem berkas).
            string safeEmail = _userEmail.Replace("@", "_").Replace(".", "_");
            string userDbFile = $"transactions_{safeEmail}.json";

            // Initialize Core Services dengan database terisolasi milik user
            _repo = new DataRepository<Transaction>(userDbFile);
            _financeManager = new TransactionManager(_repo);
            _exportService = new ExportApiService();

            // Seed default data jika database user baru masih kosong
            // Mengapa: Memberikan data dummy awal yang representatif (5 Pemasukan & 5 Pengeluaran)
            // agar tampilan UI dashboard tidak kosong melongpong saat pertama kali digunakan (UX-Friendly).
            if (_repo.GetAll().Count == 0)
            {
                SeedDefaultTransactions();
            }

            // Initialize Reminders Seed Data
            _reminders = new List<PengingatTagihan>
            {
                new PengingatTagihan("Langganan Biznet", "Internet", 300000, DateTime.Now.AddDays(-35)),
                new PengingatTagihan("PDAM Bulan Ini", "Air", 50000, DateTime.Now.AddDays(-5)),
                new PengingatTagihan("Token PLN", "Listrik", 150000, DateTime.Now.AddDays(-10)),
                new PengingatTagihan("Kost Bulanan", "Sewa Rumah", 2000000, DateTime.Now.AddDays(-8)),
                new PengingatTagihan("Netflix Premium", "Netflix", 75000, DateTime.Now)
            };

            InitializeComponent();
            RefreshData();
        }

        private void SeedDefaultTransactions()
        {
            // 5 Incomes (Pemasukan)
            _financeManager.RecordTransaction(15000000, "Gaji", "Monthly Salary");
            _financeManager.RecordTransaction(500000, "Uang Saku", "Bulanan dari Ortu");
            _financeManager.RecordTransaction(2000000, "Gaji", "Bonus Project");
            _financeManager.RecordTransaction(100000, "Uang Saku", "Hadiah Ulang Tahun");
            _financeManager.RecordTransaction(1500000, "Gaji", "Freelance Design");

            // 5 Expenses (Pengeluaran)
            _financeManager.RecordTransaction(50000, "Makanan dan Minuman", "Coffee Shop");
            _financeManager.RecordTransaction(200000, "Transportasi", "Bensin Mobil");
            _financeManager.RecordTransaction(1200000, "Belanja", "Belanja Bulanan");
            _financeManager.RecordTransaction(150000, "Makanan dan Minuman", "Dinner Malam Minggu");
            _financeManager.RecordTransaction(300000, "Transportasi", "Tiket Kereta Api");
        }

        private void InitializeComponent()
        {
            // Form Setup
            this.Size = new Size(880, 680);
            this.MinimumSize = new Size(880, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(26, 26, 46);
            this.Text = "MonTrack - Personal Finance Dashboard";

            // Header Panel
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = Color.FromArgb(78, 49, 170)
            };

            lblHeaderTitle = new Label
            {
                Text = "MONTRACK",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(25, 15),
                Size = new Size(200, 35)
            };

            lblHeaderSubtitle = new Label
            {
                Text = "Premium Financial Dashboard",
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                Location = new Point(25, 48),
                Size = new Size(250, 20)
            };

            lblBalance = new Label
            {
                Text = "Current Balance: Rp 0",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(400, 25),
                Size = new Size(450, 40),
                TextAlign = ContentAlignment.MiddleRight
            };

            headerPanel.Controls.Add(lblHeaderTitle);
            headerPanel.Controls.Add(lblHeaderSubtitle);
            headerPanel.Controls.Add(lblBalance);

            // Tab Control Setup
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };

            // Register Tab Pages
            tabTransactions = new TabPage { Text = " Transactions ", BackColor = Color.FromArgb(26, 26, 46) };
            tabReminders = new TabPage { Text = " Bill Reminders ", BackColor = Color.FromArgb(26, 26, 46) };
            tabReports = new TabPage { Text = " Analytics & Chart ", BackColor = Color.FromArgb(26, 26, 46) };
            tabExport = new TabPage { Text = " Data Export ", BackColor = Color.FromArgb(26, 26, 46) };

            tabControl.TabPages.Add(tabTransactions);
            tabControl.TabPages.Add(tabReminders);
            tabControl.TabPages.Add(tabReports);
            tabControl.TabPages.Add(tabExport);

            // --- Tab 1: Transactions Layout ---
            // Left Column: Entry Form
            inputPanel = new Panel
            {
                Location = new Point(15, 15),
                Size = new Size(280, 480),
                BackColor = Color.FromArgb(34, 34, 59)
            };

            lblNewTxTitle = new Label
            {
                Text = "Record New Transaction",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(15, 15),
                Size = new Size(250, 25)
            };

            lblAmount = new Label
            {
                Text = "Amount (Rp)",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 50),
                Size = new Size(250, 20)
            };

            txtAmount = new EntryTextBox
            {
                Location = new Point(15, 70),
                Size = new Size(250, 28)
            };

            lblType = new Label
            {
                Text = "Transaction Type",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 110),
                Size = new Size(250, 20)
            };

            cmbType = new ComboBox
            {
                Location = new Point(15, 130),
                Size = new Size(250, 28),
                BackColor = Color.FromArgb(22, 22, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbType.Items.AddRange(new object[] { "Pemasukan", "Pengeluaran" });
            cmbType.SelectedIndex = 0;
            cmbType.SelectedIndexChanged += CmbType_SelectedIndexChanged;

            lblCategory = new Label
            {
                Text = "Category",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 170),
                Size = new Size(250, 20)
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(15, 190),
                Size = new Size(250, 28),
                BackColor = Color.FromArgb(22, 22, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // Populate initial categories based on Pemasukan
            cmbCategory.Items.AddRange(new object[] { "Gaji", "Uang Saku" });
            cmbCategory.SelectedIndex = 0;

            lblDescription = new Label
            {
                Text = "Description",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 230),
                Size = new Size(250, 20)
            };

            txtDescription = new EntryTextBox
            {
                Location = new Point(15, 250),
                Size = new Size(250, 28)
            };

            btnRecord = new Button
            {
                Text = "Record Transaction",
                BackColor = Color.FromArgb(78, 49, 170),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(15, 300),
                Size = new Size(250, 45),
                FlatStyle = FlatStyle.Flat
            };
            btnRecord.FlatAppearance.BorderSize = 0;
            btnRecord.Click += BtnRecord_Click;

            lblTxStatus = new Label
            {
                ForeColor = Color.FromArgb(255, 118, 117),
                Location = new Point(15, 360),
                Size = new Size(250, 80),
                TextAlign = ContentAlignment.TopLeft
            };

            inputPanel.Controls.Add(lblNewTxTitle);
            inputPanel.Controls.Add(lblAmount);
            inputPanel.Controls.Add(txtAmount);
            inputPanel.Controls.Add(lblType);
            inputPanel.Controls.Add(cmbType);
            inputPanel.Controls.Add(lblCategory);
            inputPanel.Controls.Add(cmbCategory);
            inputPanel.Controls.Add(lblDescription);
            inputPanel.Controls.Add(txtDescription);
            inputPanel.Controls.Add(btnRecord);
            inputPanel.Controls.Add(lblTxStatus);

            // Right Column: Filter Buttons & ListView
            filterPanel = new Panel
            {
                Location = new Point(310, 15),
                Size = new Size(535, 40),
                BackColor = Color.FromArgb(26, 26, 46)
            };

            Button btnFilterAll = new Button
            {
                Text = "All Transactions",
                Size = new Size(170, 32),
                Location = new Point(0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(78, 49, 170),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnFilterAll.FlatAppearance.BorderSize = 0;

            Button btnFilterIncome = new Button
            {
                Text = "Income (Pemasukan)",
                Size = new Size(170, 32),
                Location = new Point(180, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(34, 34, 59),
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            btnFilterIncome.FlatAppearance.BorderSize = 0;

            Button btnFilterExpense = new Button
            {
                Text = "Expense (Pengeluaran)",
                Size = new Size(170, 32),
                Location = new Point(360, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(34, 34, 59),
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            btnFilterExpense.FlatAppearance.BorderSize = 0;

            btnFilterAll.Click += (s, e) => {
                _currentFilter = FilterType.All;
                HighlightActiveFilterButton(btnFilterAll, btnFilterIncome, btnFilterExpense);
                RefreshData();
            };

            btnFilterIncome.Click += (s, e) => {
                _currentFilter = FilterType.Pemasukan;
                HighlightActiveFilterButton(btnFilterIncome, btnFilterAll, btnFilterExpense);
                RefreshData();
            };

            btnFilterExpense.Click += (s, e) => {
                _currentFilter = FilterType.Pengeluaran;
                HighlightActiveFilterButton(btnFilterExpense, btnFilterAll, btnFilterIncome);
                RefreshData();
            };

            filterPanel.Controls.Add(btnFilterAll);
            filterPanel.Controls.Add(btnFilterIncome);
            filterPanel.Controls.Add(btnFilterExpense);

            lvTransactions = new ListView
            {
                Location = new Point(310, 65),
                Size = new Size(535, 430),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BackColor = Color.FromArgb(34, 34, 59),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            lvTransactions.Columns.Add("ID", 50);
            lvTransactions.Columns.Add("Date", 130);
            lvTransactions.Columns.Add("Type", 100);
            lvTransactions.Columns.Add("Amount", 110);
            lvTransactions.Columns.Add("Category", 100);
            lvTransactions.Columns.Add("Description", 120);

            // Dynamically resize Description column to fill remaining space
            lvTransactions.Resize += (s, e) => {
                int totalWidth = 0;
                for (int i = 0; i < lvTransactions.Columns.Count - 1; i++)
                {
                    totalWidth += lvTransactions.Columns[i].Width;
                }
                int remaining = lvTransactions.Width - totalWidth - 4;
                if (remaining > 120)
                {
                    lvTransactions.Columns[5].Width = remaining;
                }
                else
                {
                    lvTransactions.Columns[5].Width = 120;
                }
            };

            tabTransactions.Controls.Add(inputPanel);
            tabTransactions.Controls.Add(filterPanel);
            tabTransactions.Controls.Add(lvTransactions);

            // --- Tab 2: Bill Reminders Layout ---
            lvReminders = new ListView
            {
                Location = new Point(25, 20),
                Size = new Size(810, 400),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BackColor = Color.FromArgb(34, 34, 59),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            lvReminders.Columns.Add("Bill Name", 180);
            lvReminders.Columns.Add("Category", 120);
            lvReminders.Columns.Add("Group", 120);
            lvReminders.Columns.Add("Amount (Rp)", 120);
            lvReminders.Columns.Add("Deadline", 120);
            lvReminders.Columns.Add("Status", 130);

            btnMarkPaid = new Button
            {
                Text = "Mark Selected as Paid",
                BackColor = Color.FromArgb(55, 149, 189),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(25, 440),
                Size = new Size(200, 45),
                FlatStyle = FlatStyle.Flat
            };
            btnMarkPaid.FlatAppearance.BorderSize = 0;
            btnMarkPaid.Click += BtnMarkPaid_Click;

            lblReminderStatus = new Label
            {
                ForeColor = Color.FromArgb(85, 239, 196),
                Location = new Point(245, 440),
                Size = new Size(590, 45),
                TextAlign = ContentAlignment.MiddleLeft
            };

            tabReminders.Controls.Add(lvReminders);
            tabReminders.Controls.Add(btnMarkPaid);
            tabReminders.Controls.Add(lblReminderStatus);

            // --- Tab 3: Data Export Layout ---
            exportCard = new Panel
            {
                Location = new Point(225, 75),
                Size = new Size(410, 350),
                BackColor = Color.FromArgb(34, 34, 59)
            };

            lblExportTitle = new Label
            {
                Text = "Export Transaction Report",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(30, 30),
                Size = new Size(350, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblExportFormat = new Label
            {
                Text = "Select Export Format:",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(30, 95),
                Size = new Size(350, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            cmbExportFormat = new ComboBox
            {
                Location = new Point(30, 120),
                Size = new Size(350, 28),
                BackColor = Color.FromArgb(22, 22, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbExportFormat.Items.AddRange(new object[] { "CSV", "PDF", "Both (CSV + PDF)" });
            cmbExportFormat.SelectedIndex = 0;

            btnExport = new Button
            {
                Text = "Export Report Now",
                BackColor = Color.FromArgb(85, 239, 196),
                ForeColor = Color.FromArgb(26, 26, 46),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(30, 180),
                Size = new Size(350, 50),
                FlatStyle = FlatStyle.Flat
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;

            lblExportStatus = new Label
            {
                ForeColor = Color.FromArgb(85, 239, 196),
                Location = new Point(30, 245),
                Size = new Size(350, 90),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 8.5F)
            };

            exportCard.Controls.Add(lblExportTitle);
            exportCard.Controls.Add(lblExportFormat);
            exportCard.Controls.Add(cmbExportFormat);
            exportCard.Controls.Add(btnExport);
            exportCard.Controls.Add(lblExportStatus);

            tabExport.Controls.Add(exportCard);

            // --- Tab 4: Analytics & Chart Layout ---
            pnlChart = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(34, 34, 59)
            };
            pnlChart.Paint += PnlChart_Paint;
            pnlChart.Resize += (s, e) => pnlChart.Invalidate();
            tabReports.Controls.Add(pnlChart);

            // Add main panels
            this.Controls.Add(tabControl);
            this.Controls.Add(headerPanel);
        }

        private void RefreshData()
        {
            // Update Balance Display
            double balance = _financeManager.GetCurrentBalance();
            lblBalance.Text = $"Current Balance: Rp {balance:N0}";

            // Refresh Chart Display
            if (pnlChart != null)
            {
                pnlChart.Invalidate();
            }

            // Refresh ListView Transactions
            lvTransactions.Items.Clear();
            var transactions = _repo.GetAll();
            if (_currentFilter == FilterType.Pemasukan)
            {
                transactions = transactions.Where(t => t.Type == TransactionType.Pemasukan).ToList();
            }
            else if (_currentFilter == FilterType.Pengeluaran)
            {
                transactions = transactions.Where(t => t.Type == TransactionType.Pengeluaran).ToList();
            }

            foreach (var tx in transactions)
            {
                var item = new ListViewItem(tx.Id.ToString());
                item.SubItems.Add(tx.Date.ToString("yyyy-MM-dd HH:mm"));
                item.SubItems.Add(tx.Type.ToString());
                item.SubItems.Add($"Rp {tx.Amount:N0}");
                item.SubItems.Add(tx.Category);
                item.SubItems.Add(tx.Description);

                // Add colors to distinguish Income vs Expense
                if (tx.Type == TransactionType.Pemasukan)
                {
                    item.ForeColor = Color.FromArgb(85, 239, 196);
                }
                else
                {
                    item.ForeColor = Color.FromArgb(255, 118, 117);
                }

                lvTransactions.Items.Add(item);
            }

            // Refresh ListView Reminders
            lvReminders.Items.Clear();
            foreach (var r in _reminders)
            {
                r.UpdateStatusBerdasarkanWaktu();
                var item = new ListViewItem(r.Nama);
                item.SubItems.Add(r.Kategori);
                item.SubItems.Add(r.Kelompok);
                item.SubItems.Add($"Rp {r.Nominal:N0}");
                item.SubItems.Add(r.Deadline.ToShortDateString());
                item.SubItems.Add(r.StatusSaatIni.ToString());

                // Color coding for status
                if (r.StatusSaatIni == PengingatTagihan.TagihanState.Lunas)
                {
                    item.ForeColor = Color.FromArgb(85, 239, 196);
                }
                else if (r.StatusSaatIni == PengingatTagihan.TagihanState.Terlambat)
                {
                    item.ForeColor = Color.FromArgb(255, 118, 117);
                }
                else
                {
                    item.ForeColor = Color.FromArgb(250, 177, 160);
                }

                lvReminders.Items.Add(item);
            }
        }

        private void BtnRecord_Click(object sender, EventArgs e)
        {
            lblTxStatus.Text = "";
            string amountText = txtAmount.Text;
            string category = cmbCategory.Text;
            string description = txtDescription.Text;

            if (string.IsNullOrEmpty(amountText))
            {
                lblTxStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblTxStatus.Text = "Please enter an amount.";
                return;
            }

            if (!double.TryParse(amountText, out double amount) || amount <= 0)
            {
                lblTxStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblTxStatus.Text = "Please enter a valid amount greater than 0.";
                return;
            }

            try
            {
                _financeManager.RecordTransaction(amount, category, description);
                lblTxStatus.ForeColor = Color.FromArgb(85, 239, 196);
                lblTxStatus.Text = "Transaction recorded successfully!";
                txtAmount.Text = "";
                txtDescription.Text = "";
                RefreshData();
            }
            catch (Exception ex)
            {
                lblTxStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblTxStatus.Text = $"Error: {ex.Message}";
            }
        }

        private void BtnMarkPaid_Click(object sender, EventArgs e)
        {
            lblReminderStatus.Text = "";

            if (lvReminders.SelectedItems.Count == 0)
            {
                lblReminderStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblReminderStatus.Text = "Please select a bill reminder from the list.";
                return;
            }

            string selectedBillName = lvReminders.SelectedItems[0].Text;
            var selectedReminder = _reminders.FirstOrDefault(x => x.Nama == selectedBillName);

            if (selectedReminder != null)
            {
                if (selectedReminder.StatusSaatIni == PengingatTagihan.TagihanState.Lunas)
                {
                    lblReminderStatus.ForeColor = Color.FromArgb(250, 177, 160);
                    lblReminderStatus.Text = $"[INFO] {selectedReminder.Nama} was already marked as paid.";
                    return;
                }

                selectedReminder.TandaiLunas();
                lblReminderStatus.ForeColor = Color.FromArgb(85, 239, 196);
                lblReminderStatus.Text = $"[SUCCESS] Bill '{selectedReminder.Nama}' marked as PAID.";
                RefreshData();
            }
        }

        private async void BtnExport_Click(object sender, EventArgs e)
        {
            lblExportStatus.Text = "";
            btnExport.Enabled = false;

            var allTx = _repo.GetAll();
            if (allTx.Count == 0)
            {
                lblExportStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblExportStatus.Text = "Error: No transaction records found to export. Please record some transactions first.";
                btnExport.Enabled = true;
                return;
            }

            // Map PencatatanKeuangan.Models.Transaction to MonTrack.Models.Transaction
            var exportData = allTx.Select(t => new MonTrack.Models.Transaction
            {
                Id = t.Id,
                Amount = t.Amount,
                Category = t.Category,
                Description = t.Description,
                Date = t.Date
            }).ToList();

            string format = cmbExportFormat.Text;
            string defaultFileName = $"export_{DateTime.Now:yyyyMMdd_HHmmss}";

            // Mengapa menggunakan SaveFileDialog secara interaktif:
            // Dibandingkan hardcoding absolute path (yang akan menyebabkan I/O Crash jika folder target
            // tidak eksis di laptop lain), SaveFileDialog memberikan fleksibilitas penuh bagi pengguna
            // untuk memilih lokasi penyimpanan secara portabel di perangkat mana saja secara aman.
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                if (format == "CSV")
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv";
                    sfd.FileName = defaultFileName + ".csv";
                }
                else if (format == "PDF")
                {
                    sfd.Filter = "PDF files (*.pdf)|*.pdf";
                    sfd.FileName = defaultFileName + ".pdf";
                }
                else // Keduanya (CSV & PDF)
                {
                    sfd.Filter = "All files (*.*)|*.*";
                    sfd.FileName = defaultFileName; // Nama dasar tanpa ekstensi
                    sfd.Title = "Select base name and folder for CSV & PDF exports";
                }

                // Mengapa hasil dialog diperiksa:
                // Menangani skenario di mana pengguna membatalkan dialog ekspor (Defensive Programming)
                // agar UI tidak crash dan tombol kembali diaktifkan kembali secara wajar.
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    btnExport.Enabled = true;
                    lblExportStatus.ForeColor = Color.FromArgb(255, 118, 117);
                    lblExportStatus.Text = "Export cancelled by user.";
                    return;
                }

                string targetPath = sfd.FileName;

                btnExport.Text = "Exporting data...";
                lblExportStatus.ForeColor = Color.White;
                lblExportStatus.Text = "Preparing file structure...";

                try
                {
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                    if (format == "CSV")
                    {
                        await _exportService.ExecuteExport("CSV", exportData, targetPath);
                        stopwatch.Stop();
                        DisplayExportResult(targetPath, stopwatch.ElapsedMilliseconds, exportData.Count, "CSV");
                    }
                    else if (format == "PDF")
                    {
                        await _exportService.ExecuteExport("PDF", exportData, targetPath);
                        stopwatch.Stop();
                        DisplayExportResult(targetPath, stopwatch.ElapsedMilliseconds, exportData.Count, "PDF");
                    }
                    else // Both
                    {
                        string csvPath = Path.ChangeExtension(targetPath, ".csv");
                        string pdfPath = Path.ChangeExtension(targetPath, ".pdf");
                        
                        await _exportService.ExecuteExport("CSV", exportData, csvPath);
                        await _exportService.ExecuteExport("PDF", exportData, pdfPath);
                        
                        stopwatch.Stop();
                        lblExportStatus.ForeColor = Color.FromArgb(85, 239, 196);
                        lblExportStatus.Text = $"✓ Both CSV & PDF exported successfully!\n" +
                                               $"  Records: {exportData.Count}\n" +
                                               $"  Time: {stopwatch.ElapsedMilliseconds} ms\n" +
                                               $"  Folder: {Path.GetDirectoryName(targetPath)}";
                    }
                }
                catch (Exception ex)
                {
                    lblExportStatus.ForeColor = Color.FromArgb(255, 118, 117);
                    lblExportStatus.Text = $"Export failed: {ex.Message}";
                }
            }

            btnExport.Text = "Export Report Now";
            btnExport.Enabled = true;
        }

        private void DisplayExportResult(string filePath, long ms, int count, string format)
        {
            var info = new FileInfo(filePath);
            lblExportStatus.ForeColor = Color.FromArgb(85, 239, 196);
            lblExportStatus.Text = $"✓ {format} Export Successful!\n" +
                                   $"  File: {Path.GetFileName(filePath)}\n" +
                                   $"  Size: {info.Length / 1024.0:F2} KB\n" +
                                   $"  Records: {count}\n" +
                                   $"  Time: {ms} ms";
        }

        private void HighlightActiveFilterButton(Button active, Button inactive1, Button inactive2)
        {
            active.BackColor = Color.FromArgb(78, 49, 170);
            active.ForeColor = Color.White;
            active.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            inactive1.BackColor = Color.FromArgb(34, 34, 59);
            inactive1.ForeColor = Color.FromArgb(189, 195, 199);
            inactive1.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            inactive2.BackColor = Color.FromArgb(34, 34, 59);
            inactive2.ForeColor = Color.FromArgb(189, 195, 199);
            inactive2.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Set anchors after layout has initialized to runtime dimensions
            inputPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            filterPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lvTransactions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            lvReminders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnMarkPaid.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblReminderStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            exportCard.Anchor = AnchorStyles.None;
            lblBalance.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbCategory.Items.Clear();
            if (cmbType.Text == "Pemasukan")
            {
                cmbCategory.Items.AddRange(new object[] { "Gaji", "Uang Saku" });
            }
            else
            {
                cmbCategory.Items.AddRange(new object[] { 
                    "Makanan dan Minuman", 
                    "Tagihan", 
                    "Cicilan", 
                    "Belanja", 
                    "Transportasi", 
                    "Hiburan", 
                    "Pendidikan dan Kesehatan" 
                });
            }
            if (cmbCategory.Items.Count > 0)
            {
                cmbCategory.SelectedIndex = 0;
            }
        }

        private void PnlChart_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int width = pnlChart.Width;
            int height = pnlChart.Height;

            // Draw Background card
            using (var bgBrush = new SolidBrush(Color.FromArgb(34, 34, 59)))
            {
                g.FillRectangle(bgBrush, 0, 0, width, height);
            }

            // Title
            using (var titleFont = new Font("Segoe UI", 14, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(Color.White))
            {
                g.DrawString("Monthly Difference (Pemasukan - Pengeluaran)", titleFont, titleBrush, new PointF(25, 20));
            }

            // Retrieve Data
            var monthlyData = _repo.GetAll()
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .Select(gGroup => new
                {
                    MonthKey = $"{gGroup.Key.Year}-{gGroup.Key.Month:D2}",
                    MonthName = new DateTime(gGroup.Key.Year, gGroup.Key.Month, 1).ToString("MMM yyyy"),
                    Income = gGroup.Where(t => t.Type == TransactionType.Pemasukan).Sum(t => t.Amount),
                    Expense = gGroup.Where(t => t.Type == TransactionType.Pengeluaran).Sum(t => t.Amount)
                })
                .Select(x => new
                {
                    x.MonthKey,
                    x.MonthName,
                    x.Income,
                    x.Expense,
                    Difference = x.Income - x.Expense
                })
                .OrderBy(m => m.MonthKey)
                .ToList();

            if (monthlyData.Count == 0)
            {
                using (var infoFont = new Font("Segoe UI", 12, FontStyle.Italic))
                using (var infoBrush = new SolidBrush(Color.FromArgb(189, 195, 199)))
                {
                    string msg = "Tidak ada data transaksi untuk ditampilkan.";
                    SizeF size = g.MeasureString(msg, infoFont);
                    g.DrawString(msg, infoFont, infoBrush, (width - size.Width) / 2, (height - size.Height) / 2);
                }
                return;
            }

            // Chart area bounds
            int paddingLeft = 60;
            int paddingRight = 40;
            int paddingTop = 80;
            int paddingBottom = 60;

            int chartWidth = width - paddingLeft - paddingRight;
            int chartHeight = height - paddingTop - paddingBottom;

            if (chartWidth <= 0 || chartHeight <= 0) return;

            // Find max absolute difference to scale
            double maxVal = monthlyData.Max(m => Math.Abs(m.Difference));
            if (maxVal == 0) maxVal = 100000; // prevent division by zero

            // Y-axis middle (0 line)
            float zeroY = paddingTop + (chartHeight / 2f);

            // Draw Y Grid lines & Labels
            using (var gridPen = new Pen(Color.FromArgb(50, 255, 255, 255), 1))
            using (var labelFont = new Font("Segoe UI", 8))
            using (var labelBrush = new SolidBrush(Color.FromArgb(189, 195, 199)))
            {
                gridPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                // Draw 0 line
                g.DrawLine(new Pen(Color.FromArgb(150, 255, 255, 255), 1), paddingLeft, zeroY, width - paddingRight, zeroY);
                g.DrawString("Rp 0", labelFont, labelBrush, 10, zeroY - 6);

                // Draw +Max and -Max lines
                float topY = paddingTop;
                float bottomY = height - paddingBottom;

                g.DrawLine(gridPen, paddingLeft, topY, width - paddingRight, topY);
                g.DrawString($"+{FormatAmount(maxVal)}", labelFont, labelBrush, 10, topY - 6);

                g.DrawLine(gridPen, paddingLeft, bottomY, width - paddingRight, bottomY);
                g.DrawString($"-{FormatAmount(maxVal)}", labelFont, labelBrush, 10, bottomY - 6);
            }

            // Draw Bars
            int count = monthlyData.Count;
            float barWidth = Math.Max(20f, (chartWidth / (float)count) * 0.5f);
            float spacing = chartWidth / (float)count;

            using (var surplusBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, 100, 100), Color.FromArgb(85, 239, 196), Color.FromArgb(46, 204, 113), 90F))
            using (var deficitBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, 100, 100), Color.FromArgb(255, 118, 117), Color.FromArgb(231, 76, 60), 90F))
            using (var labelFont = new Font("Segoe UI", 8.5F))
            using (var textFont = new Font("Segoe UI", 8, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.White))
            using (var labelBrush = new SolidBrush(Color.FromArgb(189, 195, 199)))
            {
                for (int i = 0; i < count; i++)
                {
                    var data = monthlyData[i];
                    float centerX = paddingLeft + (i * spacing) + (spacing / 2f);
                    float x = centerX - (barWidth / 2f);

                    // Calculate bar height scaled to chart area
                    float valH = (float)((Math.Abs(data.Difference) / maxVal) * (chartHeight / 2f));
                    float y = 0;

                    System.Drawing.Drawing2D.LinearGradientBrush barBrush;
                    if (data.Difference >= 0)
                    {
                        y = zeroY - valH;
                        barBrush = surplusBrush;
                    }
                    else
                    {
                        y = zeroY;
                        barBrush = deficitBrush;
                    }

                    // Reset gradient brush bounds to match the bar
                    barBrush.ResetTransform();
                    barBrush.TranslateTransform(x, y);
                    barBrush.ScaleTransform(barWidth / 100f, valH / 100f);

                    // Draw bar
                    g.FillRectangle(barBrush, x, y, barWidth, Math.Max(2, valH));

                    // Draw value above/below bar
                    string valStr = (data.Difference >= 0 ? "+" : "") + FormatAmount(data.Difference);
                    SizeF valSize = g.MeasureString(valStr, textFont);
                    float valY = data.Difference >= 0 ? y - valSize.Height - 4 : y + valH + 4;
                    g.DrawString(valStr, textFont, textBrush, centerX - (valSize.Width / 2f), valY);

                    // Draw Month Label below X Axis
                    SizeF labelSize = g.MeasureString(data.MonthName, labelFont);
                    float labelY = height - paddingBottom + 10;
                    g.DrawString(data.MonthName, labelFont, labelBrush, centerX - (labelSize.Width / 2f), labelY);

                    // Draw a small dot on X axis
                    g.FillEllipse(textBrush, centerX - 2, zeroY - 2, 4, 4);
                }
            }
        }

        private string FormatAmount(double val)
        {
            double absVal = Math.Abs(val);
            if (absVal >= 1000000000)
                return $"Rp {(val / 1000000000.0):F1}M";
            if (absVal >= 1000000)
                return $"Rp {(val / 1000000.0):F1}jt";
            if (absVal >= 1000)
                return $"Rp {(val / 1000.0):F0}rb";
            return $"Rp {val:N0}";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit(); // Ensure full process shutdown when main form is closed
        }
    }
}
