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
        private Button btnLogout;
        private bool _isLoggingOut = false;
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
        private Label lblDate;
        private DateTimePicker dtpDate;
        private Label lblAmount;
        private EntryTextBox txtAmount;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblDescription;
        private EntryTextBox txtDescription;
        private Button btnRecord;
        private Label lblTxStatus;
        private Label lblSearch;
        private EntryTextBox txtSearch;
        private ComboBox cmbFilterCategory;

        // Tab 2: Bill Reminders
        private TabPage tabReminders;
        private ListView lvReminders;
        private Button btnMarkPaid;
        private Label lblReminderStatus;
        private Panel reminderInputPanel;
        private Label lblReminderTitle;
        private Label lblReminderName;
        private EntryTextBox txtReminderName;
        private Label lblReminderCategory;
        private ComboBox cmbReminderCategory;
        private Label lblReminderAmount;
        private EntryTextBox txtReminderAmount;
        private Label lblReminderCreatedDate;
        private DateTimePicker dtpReminderCreatedDate;
        private Label lblReminderDeadline;
        private DateTimePicker dtpReminderDeadline;
        private Label lblReminderRepeat;
        private ComboBox cmbReminderRepeat;
        private Button btnSaveReminder;
        private Label lblDaftarTagihan;
        private Label lblSearchReminder;
        private EntryTextBox txtSearchReminder;
        private Button btnSearchReminder;
        private Button btnDeleteReminder;
        private int _selectedReminderIndex = -1;

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
                Location = new Point(200, 25),
                Size = new Size(650, 40),
                TextAlign = ContentAlignment.MiddleRight
            };

            btnLogout = new Button
            {
                Text = "Log Out",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(225, 112, 85),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(15, 460),
                Size = new Size(250, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += BtnLogout_Click;

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
                Size = new Size(280, 520),
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

            lblDate = new Label
            {
                Text = "Transaction Date",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 50),
                Size = new Size(250, 20)
            };

            dtpDate = new DateTimePicker
            {
                Location = new Point(15, 70),
                Size = new Size(250, 28),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd",
                MaxDate = DateTime.Now
            };
            dtpDate.ValueChanged += (s, e) => {
                if (pnlChart != null)
                {
                    pnlChart.Invalidate();
                }
            };

            lblAmount = new Label
            {
                Text = "Amount (Rp)",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 110),
                Size = new Size(250, 20)
            };

            txtAmount = new EntryTextBox
            {
                Location = new Point(15, 130),
                Size = new Size(250, 28)
            };

            lblType = new Label
            {
                Text = "Transaction Type",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 170),
                Size = new Size(250, 20)
            };

            cmbType = new ComboBox
            {
                Location = new Point(15, 190),
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
                Location = new Point(15, 230),
                Size = new Size(250, 20)
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(15, 250),
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
                Location = new Point(15, 290),
                Size = new Size(250, 20)
            };

            txtDescription = new EntryTextBox
            {
                Location = new Point(15, 310),
                Size = new Size(250, 28)
            };

            btnRecord = new Button
            {
                Text = "Record Transaction",
                BackColor = Color.FromArgb(78, 49, 170),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(15, 360),
                Size = new Size(250, 45),
                FlatStyle = FlatStyle.Flat
            };
            btnRecord.FlatAppearance.BorderSize = 0;
            btnRecord.Click += BtnRecord_Click;

            lblTxStatus = new Label
            {
                ForeColor = Color.FromArgb(255, 118, 117),
                Location = new Point(15, 420),
                Size = new Size(250, 35),
                TextAlign = ContentAlignment.TopLeft
            };

            inputPanel.Controls.Add(lblNewTxTitle);
            inputPanel.Controls.Add(lblDate);
            inputPanel.Controls.Add(dtpDate);
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
            inputPanel.Controls.Add(btnLogout);

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
                UpdateCategoryFilterDropdown(); 
                RefreshData();
            };

            btnFilterIncome.Click += (s, e) => {
                _currentFilter = FilterType.Pemasukan;
                HighlightActiveFilterButton(btnFilterIncome, btnFilterAll, btnFilterExpense);
                UpdateCategoryFilterDropdown(); 
                RefreshData();
            };

            btnFilterExpense.Click += (s, e) => {
                _currentFilter = FilterType.Pengeluaran;
                HighlightActiveFilterButton(btnFilterExpense, btnFilterAll, btnFilterIncome);
                UpdateCategoryFilterDropdown(); 
                RefreshData();
            };

            filterPanel.Controls.Add(btnFilterAll);
            filterPanel.Controls.Add(btnFilterIncome);
            filterPanel.Controls.Add(btnFilterExpense);

            
            // Category Filter Dropdown Setup
            cmbFilterCategory = new ComboBox
            {
                Location = new Point(310, 65), 
                Size = new Size(150, 28),
                BackColor = Color.FromArgb(22, 22, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            UpdateCategoryFilterDropdown(); // Inisialisasi awal list kategori
            cmbFilterCategory.SelectedIndexChanged += (s, e) => RefreshData();


            // Search Bar Setup
            lblSearch = new Label
            {
                Text = "Search:",
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 10),
                Location = new Point(470, 68),
                Size = new Size(80, 25),
                AutoSize = true
            };

            txtSearch = new EntryTextBox
            {
                Location = new Point(540, 65), 
                Size = new Size(315, 28)       
            };
            txtSearch.TextChanged += (s, e) => RefreshData();

            lvTransactions = new ListView
            {
                Location = new Point(310, 110),
                Size = new Size(535, 385),
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
            tabTransactions.Controls.Add(cmbFilterCategory);
            tabTransactions.Controls.Add(lblSearch);
            tabTransactions.Controls.Add(txtSearch);
            tabTransactions.Controls.Add(lvTransactions);

            // --- Tab 2: Bill Reminders Layout ---
            reminderInputPanel = new Panel
            {
                Location = new Point(15, 15),
                Size = new Size(280, 540),
                BackColor = Color.FromArgb(34, 34, 59)
            };

            lblReminderTitle = new Label
            {
                Text = "Kelola Tagihan",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(15, 15),
                Size = new Size(250, 25)
            };

            lblReminderName = new Label
            {
                Text = "Nama Tagihan:",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 50),
                Size = new Size(250, 20)
            };

            txtReminderName = new EntryTextBox
            {
                Location = new Point(15, 70),
                Size = new Size(250, 28)
            };

            lblReminderCategory = new Label
            {
                Text = "Kategori:",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 110),
                Size = new Size(250, 20)
            };

            cmbReminderCategory = new ComboBox
            {
                Location = new Point(15, 130),
                Size = new Size(250, 28),
                BackColor = Color.FromArgb(22, 22, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbReminderCategory.Items.AddRange(new object[] { "Internet", "Air", "Listrik", "Sewa Rumah", "Netflix" });
            cmbReminderCategory.SelectedIndex = 0;
            cmbReminderCategory.SelectedIndexChanged += (s, e) => UpdateDeadlineDisplay();

            lblReminderAmount = new Label
            {
                Text = "Nominal:",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 170),
                Size = new Size(250, 20)
            };

            txtReminderAmount = new EntryTextBox
            {
                Location = new Point(15, 190),
                Size = new Size(250, 28)
            };

            lblReminderCreatedDate = new Label
            {
                Text = "Tanggal Tagihan Dibuat:",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 230),
                Size = new Size(250, 20)
            };

            dtpReminderCreatedDate = new DateTimePicker
            {
                Location = new Point(15, 250),
                Size = new Size(250, 28),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd",
                MaxDate = DateTime.Today.AddYears(1)
            };
            dtpReminderCreatedDate.ValueChanged += (s, e) => UpdateDeadlineDisplay();

            lblReminderDeadline = new Label
            {
                Text = "Tanggal Jatuh Tempo:",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 290),
                Size = new Size(250, 20)
            };

            dtpReminderDeadline = new DateTimePicker
            {
                Location = new Point(15, 310),
                Size = new Size(250, 28),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd",
                Enabled = true
            };

            lblReminderRepeat = new Label
            {
                Text = "Pengulangan Tagihan:",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 350),
                Size = new Size(250, 20)
            };

            cmbReminderRepeat = new ComboBox
            {
                Location = new Point(15, 370),
                Size = new Size(250, 28),
                BackColor = Color.FromArgb(22, 22, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbReminderRepeat.Items.AddRange(new object[] { "Sekali", "Mingguan", "Bulanan" });
            cmbReminderRepeat.SelectedIndex = 0;

            btnSaveReminder = new Button
            {
                Text = "+ Tambah Tagihan",
                BackColor = Color.FromArgb(78, 49, 170),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(15, 415),
                Size = new Size(250, 45),
                FlatStyle = FlatStyle.Flat
            };
            btnSaveReminder.FlatAppearance.BorderSize = 0;
            btnSaveReminder.Click += BtnSaveReminder_Click;

            lblReminderStatus = new Label
            {
                ForeColor = Color.FromArgb(255, 118, 117),
                Location = new Point(15, 470),
                Size = new Size(250, 60),
                TextAlign = ContentAlignment.TopLeft
            };

            reminderInputPanel.Controls.Add(lblReminderTitle);
            reminderInputPanel.Controls.Add(lblReminderName);
            reminderInputPanel.Controls.Add(txtReminderName);
            reminderInputPanel.Controls.Add(lblReminderCategory);
            reminderInputPanel.Controls.Add(cmbReminderCategory);
            reminderInputPanel.Controls.Add(lblReminderAmount);
            reminderInputPanel.Controls.Add(txtReminderAmount);
            reminderInputPanel.Controls.Add(lblReminderCreatedDate);
            reminderInputPanel.Controls.Add(dtpReminderCreatedDate);
            reminderInputPanel.Controls.Add(lblReminderDeadline);
            reminderInputPanel.Controls.Add(dtpReminderDeadline);
            reminderInputPanel.Controls.Add(lblReminderRepeat);
            reminderInputPanel.Controls.Add(cmbReminderRepeat);
            reminderInputPanel.Controls.Add(btnSaveReminder);
            reminderInputPanel.Controls.Add(lblReminderStatus);

            lblDaftarTagihan = new Label
            {
                Text = "Daftar Tagihan",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(310, 15),
                Size = new Size(200, 30)
            };

            lblSearchReminder = new Label
            {
                Text = "Cari Tagihan:",
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 10),
                Location = new Point(310, 68),
                Size = new Size(100, 25)
            };

            txtSearchReminder = new EntryTextBox
            {
                Location = new Point(420, 65),
                Size = new Size(325, 28)
            };
            txtSearchReminder.TextChanged += (s, e) => RefreshData();

            btnSearchReminder = new Button
            {
                Text = "Search",
                BackColor = Color.FromArgb(34, 34, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(755, 65),
                Size = new Size(90, 28),
                FlatStyle = FlatStyle.Flat
            };
            btnSearchReminder.FlatAppearance.BorderSize = 0;
            btnSearchReminder.Click += (s, e) => RefreshData();

            lvReminders = new ListView
            {
                Location = new Point(310, 110),
                Size = new Size(535, 330),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BackColor = Color.FromArgb(34, 34, 59),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            lvReminders.Columns.Add("Id", 40);
            lvReminders.Columns.Add("Nama", 110);
            lvReminders.Columns.Add("Kategori", 90);
            lvReminders.Columns.Add("Nominal", 90);
            lvReminders.Columns.Add("TanggalDibuat", 95);
            lvReminders.Columns.Add("TanggalJatuhTempo", 95);
            lvReminders.Columns.Add("Repetisi", 80);
            lvReminders.Columns.Add("StatusSaatIni", 80);
            lvReminders.SelectedIndexChanged += LvReminders_SelectedIndexChanged;

            btnMarkPaid = new Button
            {
                Text = "Tandai Sudah Lunas \u2611",
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(310, 455),
                Size = new Size(200, 45),
                FlatStyle = FlatStyle.Flat
            };
            btnMarkPaid.FlatAppearance.BorderSize = 0;
            btnMarkPaid.Click += BtnMarkPaid_Click;

            btnDeleteReminder = new Button
            {
                Text = "Hapus",
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(705, 455),
                Size = new Size(140, 45),
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteReminder.FlatAppearance.BorderSize = 0;
            btnDeleteReminder.Click += BtnDeleteReminder_Click;

            tabReminders.Controls.Add(reminderInputPanel);
            tabReminders.Controls.Add(lblDaftarTagihan);
            tabReminders.Controls.Add(lblSearchReminder);
            tabReminders.Controls.Add(txtSearchReminder);
            tabReminders.Controls.Add(btnSearchReminder);
            tabReminders.Controls.Add(lvReminders);
            tabReminders.Controls.Add(btnMarkPaid);
            tabReminders.Controls.Add(btnDeleteReminder);

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

            if (cmbFilterCategory != null && cmbFilterCategory.SelectedIndex > 0) // Index 0 adalah "All Categories"
            {
                string selectedCat = cmbFilterCategory.SelectedItem.ToString();
                transactions = transactions.Where(t => t.Category == selectedCat).ToList();
            }

            if (txtSearch != null && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string searchKeyword = txtSearch.Text.Trim();
                transactions = transactions.Where(t => t.Description != null && t.Description.IndexOf(searchKeyword, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
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
            string searchReminder = txtSearchReminder != null ? txtSearchReminder.Text.Trim() : "";

            for (int i = 0; i < _reminders.Count; i++)
            {
                var r = _reminders[i];
                r.UpdateStatusBerdasarkanWaktu();

                // Apply search filter if active
                if (!string.IsNullOrEmpty(searchReminder))
                {
                    bool matchName = r.Nama != null && r.Nama.IndexOf(searchReminder, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool matchCat = r.Kategori != null && r.Kategori.IndexOf(searchReminder, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (!matchName && !matchCat)
                    {
                        continue;
                    }
                }

                var item = new ListViewItem((i + 1).ToString()); // Id is 1-based index
                item.SubItems.Add(r.Nama);
                item.SubItems.Add(r.Kategori);
                item.SubItems.Add($"Rp {r.Nominal:N0}");
                item.SubItems.Add(r.TanggalDibuat.ToString("yyyy-MM-dd"));
                item.SubItems.Add(r.Deadline.ToString("yyyy-MM-dd"));
                item.SubItems.Add(r.Repetisi ?? "Sekali");
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

                // Tag the item with its original index in _reminders
                item.Tag = i;

                lvReminders.Items.Add(item);
            }
        }

        private void UpdateCategoryFilterDropdown()
        {
            if (cmbFilterCategory == null) return;

            // Simpan pilihan saat ini agar tidak tereset jika kategorinya masih valid
            string currentSelection = cmbFilterCategory.SelectedItem?.ToString();

            cmbFilterCategory.Items.Clear();
            cmbFilterCategory.Items.Add("All Categories"); // Default opsi semua kategori

            if (_currentFilter == FilterType.All || _currentFilter == FilterType.Pemasukan)
            {
                cmbFilterCategory.Items.AddRange(new object[] { "Gaji", "Uang Saku" });
            }

            if (_currentFilter == FilterType.All || _currentFilter == FilterType.Pengeluaran)
            {
                cmbFilterCategory.Items.AddRange(new object[] {
                    "Makanan dan Minuman", "Tagihan", "Cicilan", "Belanja",
                    "Transportasi", "Hiburan", "Pendidikan dan Kesehatan"
                });
            }

            // Kembalikan ke pilihan sebelumnya jika ada, jika tidak default ke "All Categories"
            if (currentSelection != null && cmbFilterCategory.Items.Contains(currentSelection))
            {
                cmbFilterCategory.SelectedItem = currentSelection;
            }
            else
            {
                cmbFilterCategory.SelectedIndex = 0;
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
                _financeManager.RecordTransaction(amount, category, description, dtpDate.Value);
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

        private void UpdateDeadlineDisplay()
        {
            if (dtpReminderDeadline == null || dtpReminderCreatedDate == null) return;
            dtpReminderDeadline.Value = dtpReminderCreatedDate.Value.AddDays(30);
        }

        private void LvReminders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvReminders.SelectedItems.Count == 0)
            {
                _selectedReminderIndex = -1;
                txtReminderName.Text = "";
                txtReminderAmount.Text = "";
                cmbReminderCategory.SelectedIndex = 0;
                dtpReminderCreatedDate.Value = DateTime.Today;
                if (cmbReminderRepeat != null) cmbReminderRepeat.SelectedIndex = 0;
                btnSaveReminder.Text = "+ Tambah Tagihan";
                return;
            }

            var selectedItem = lvReminders.SelectedItems[0];
            if (selectedItem.Tag is int index && index >= 0 && index < _reminders.Count)
            {
                _selectedReminderIndex = index;
                var r = _reminders[index];
                txtReminderName.Text = r.Nama;
                txtReminderAmount.Text = r.Nominal.ToString();
                
                int catIndex = cmbReminderCategory.FindStringExact(r.Kategori);
                if (catIndex >= 0) cmbReminderCategory.SelectedIndex = catIndex;
                
                dtpReminderCreatedDate.Value = r.TanggalDibuat;
                dtpReminderDeadline.Value = r.Deadline;

                if (cmbReminderRepeat != null)
                {
                    int repIndex = cmbReminderRepeat.FindStringExact(r.Repetisi ?? "Sekali");
                    if (repIndex >= 0) cmbReminderRepeat.SelectedIndex = repIndex;
                    else cmbReminderRepeat.SelectedIndex = 0;
                }

                btnSaveReminder.Text = "Simpan Perubahan";
            }
        }

        private void BtnSaveReminder_Click(object sender, EventArgs e)
        {
            lblReminderStatus.Text = "";
            string name = txtReminderName.Text.Trim();
            string category = cmbReminderCategory.Text;
            string amountText = txtReminderAmount.Text.Trim();
            DateTime createdDate = dtpReminderCreatedDate.Value;
            DateTime deadline = dtpReminderDeadline.Value;
            string repetisi = cmbReminderRepeat != null ? cmbReminderRepeat.Text : "Sekali";

            if (string.IsNullOrEmpty(name))
            {
                lblReminderStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblReminderStatus.Text = "Nama tagihan harus diisi.";
                return;
            }

            if (string.IsNullOrEmpty(amountText) || !int.TryParse(amountText, out int nominal) || nominal <= 0)
            {
                lblReminderStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblReminderStatus.Text = "Nominal harus berupa angka positif.";
                return;
            }

            try
            {
                if (_selectedReminderIndex == -1)
                {
                    var newReminder = new PengingatTagihan(name, category, nominal, createdDate, deadline, repetisi);
                    _reminders.Add(newReminder);
                    lblReminderStatus.ForeColor = Color.FromArgb(85, 239, 196);
                    lblReminderStatus.Text = "Tagihan berhasil ditambahkan!";
                }
                else
                {
                    var updatedReminder = new PengingatTagihan(name, category, nominal, createdDate, deadline, repetisi);
                    _reminders[_selectedReminderIndex] = updatedReminder;
                    lblReminderStatus.ForeColor = Color.FromArgb(85, 239, 196);
                    lblReminderStatus.Text = "Tagihan berhasil diubah!";
                }

                // Reset inputs
                _selectedReminderIndex = -1;
                txtReminderName.Text = "";
                txtReminderAmount.Text = "";
                cmbReminderCategory.SelectedIndex = 0;
                dtpReminderCreatedDate.Value = DateTime.Today;
                if (cmbReminderRepeat != null) cmbReminderRepeat.SelectedIndex = 0;
                btnSaveReminder.Text = "+ Tambah Tagihan";

                RefreshData();
            }
            catch (Exception ex)
            {
                lblReminderStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblReminderStatus.Text = $"Error: {ex.Message}";
            }
        }

        private void BtnDeleteReminder_Click(object sender, EventArgs e)
        {
            lblReminderStatus.Text = "";

            if (lvReminders.SelectedItems.Count == 0)
            {
                lblReminderStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblReminderStatus.Text = "Pilih tagihan dari daftar untuk dihapus.";
                return;
            }

            var selectedItem = lvReminders.SelectedItems[0];
            if (selectedItem.Tag is int index && index >= 0 && index < _reminders.Count)
            {
                string billName = _reminders[index].Nama;
                _reminders.RemoveAt(index);
                
                lblReminderStatus.ForeColor = Color.FromArgb(85, 239, 196);
                lblReminderStatus.Text = $"Tagihan '{billName}' berhasil dihapus.";
                
                // Clear selection
                _selectedReminderIndex = -1;
                txtReminderName.Text = "";
                txtReminderAmount.Text = "";
                cmbReminderCategory.SelectedIndex = 0;
                dtpReminderCreatedDate.Value = DateTime.Today;
                btnSaveReminder.Text = "+ Tambah Tagihan";

                RefreshData();
            }
        }

        private void BtnMarkPaid_Click(object sender, EventArgs e)
        {
            lblReminderStatus.Text = "";

            if (lvReminders.SelectedItems.Count == 0)
            {
                lblReminderStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblReminderStatus.Text = "Pilih tagihan dari daftar untuk ditandai lunas.";
                return;
            }

            var selectedItem = lvReminders.SelectedItems[0];
            if (selectedItem.Tag is int index && index >= 0 && index < _reminders.Count)
            {
                var selectedReminder = _reminders[index];
                if (selectedReminder.StatusSaatIni == PengingatTagihan.TagihanState.Lunas)
                {
                    lblReminderStatus.ForeColor = Color.FromArgb(250, 177, 160);
                    lblReminderStatus.Text = $"[INFO] {selectedReminder.Nama} sudah berstatus lunas.";
                    return;
                }

                try
                {
                    // Rekam transaksi pengeluaran keuangan terlebih dahulu untuk tagihan ini
                    _financeManager.RecordTransaction(selectedReminder.Nominal, "Tagihan", $"Bayar Tagihan: {selectedReminder.Nama}", DateTime.Now);
                    
                    // Ubah status tagihan menjadi lunas
                    selectedReminder.TandaiLunas();
                    lblReminderStatus.ForeColor = Color.FromArgb(85, 239, 196);
                    lblReminderStatus.Text = $"[SUCCESS] Tagihan '{selectedReminder.Nama}' ditandai LUNAS dan dicatat ke keuangan.";
                    RefreshData();
                }
                catch (Exception ex)
                {
                    lblReminderStatus.ForeColor = Color.FromArgb(255, 118, 117);
                    lblReminderStatus.Text = $"Gagal membayar: {ex.Message}";
                }
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

            // Tab 2 Reminders Layout Anchors
            reminderInputPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lvReminders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnMarkPaid.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeleteReminder.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            txtSearchReminder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnSearchReminder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblReminderStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

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

            DateTime selectedDate = dtpDate != null ? dtpDate.Value.Date : DateTime.Today;
            int year = selectedDate.Year;
            int month = selectedDate.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Title
            using (var titleFont = new Font("Segoe UI", 14, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(Color.White))
            {
                g.DrawString($"Daily Transactions Trend: {selectedDate:MMMM yyyy}", titleFont, titleBrush, new PointF(25, 20));
            }

            // Retrieve all transactions for the selected month
            var monthTransactions = _repo.GetAll()
                .Where(t => t.Date.Year == year && t.Date.Month == month)
                .ToList();

            double totalIncome = monthTransactions.Where(t => t.Type == TransactionType.Pemasukan).Sum(t => t.Amount);
            double totalExpense = monthTransactions.Where(t => t.Type == TransactionType.Pengeluaran).Sum(t => t.Amount);
            double netDiff = totalIncome - totalExpense;

            // Draw Summary Cards
            int cardY = 60;
            int cardH = 65;
            int cardW = (width - 70) / 3;

            if (cardW > 10)
            {
                // Card 1: Monthly Income
                DrawSummaryCard(g, 25, cardY, cardW, cardH, "Monthly Income", totalIncome, Color.FromArgb(85, 239, 196));
                // Card 2: Monthly Expense
                DrawSummaryCard(g, 25 + cardW + 10, cardY, cardW, cardH, "Monthly Expense", totalExpense, Color.FromArgb(255, 118, 117));
                // Card 3: Net Balance
                DrawSummaryCard(g, 25 + (cardW + 10) * 2, cardY, cardW, cardH, "Net Balance", netDiff, netDiff >= 0 ? Color.FromArgb(85, 239, 196) : Color.FromArgb(255, 118, 117));
            }

            // Chart area bounds
            int paddingLeft = 70;
            int paddingRight = 30;
            int paddingTop = 150;
            int paddingBottom = 50;

            int chartWidth = width - paddingLeft - paddingRight;
            int chartHeight = height - paddingTop - paddingBottom;

            if (chartWidth <= 0 || chartHeight <= 0) return;

            // Group transactions by day of the month
            var dailyTotals = Enumerable.Range(1, daysInMonth).Select(d => new
            {
                Day = d,
                Income = monthTransactions.Where(t => t.Date.Day == d && t.Type == TransactionType.Pemasukan).Sum(t => t.Amount),
                Expense = monthTransactions.Where(t => t.Date.Day == d && t.Type == TransactionType.Pengeluaran).Sum(t => t.Amount)
            }).ToList();

            double maxVal = dailyTotals.Max(d => Math.Max(d.Income, d.Expense));
            if (maxVal == 0) maxVal = 100000; // default minimum scale

            // Draw Y-axis line & Grid lines
            using (var gridPen = new Pen(Color.FromArgb(40, 255, 255, 255), 1))
            using (var labelFont = new Font("Segoe UI", 8))
            using (var labelBrush = new SolidBrush(Color.FromArgb(189, 195, 199)))
            {
                gridPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                
                // Draw bottom line (Rp 0)
                float zeroY = paddingTop + chartHeight;
                g.DrawLine(new Pen(Color.FromArgb(100, 255, 255, 255), 1), paddingLeft, zeroY, width - paddingRight, zeroY);
                g.DrawString("Rp 0", labelFont, labelBrush, 10, zeroY - 6);

                // Draw Max line
                g.DrawLine(gridPen, paddingLeft, paddingTop, width - paddingRight, paddingTop);
                g.DrawString(FormatAmount(maxVal), labelFont, labelBrush, 10, paddingTop - 6);

                // Draw Half-Max line
                float midY = paddingTop + (chartHeight / 2f);
                g.DrawLine(gridPen, paddingLeft, midY, width - paddingRight, midY);
                g.DrawString(FormatAmount(maxVal / 2f), labelFont, labelBrush, 10, midY - 6);
            }

            // Draw Daily Bars
            float spacing = chartWidth / (float)daysInMonth;
            float groupWidth = spacing * 0.8f;
            float barW = groupWidth / 2f - 1f;
            barW = Math.Max(2f, barW);

            using (var incomeBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, 100, 100), Color.FromArgb(85, 239, 196), Color.FromArgb(46, 204, 113), 90F))
            using (var expenseBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, 100, 100), Color.FromArgb(255, 118, 117), Color.FromArgb(231, 76, 60), 90F))
            using (var labelFont = new Font("Segoe UI", 7.5F))
            using (var labelBrush = new SolidBrush(Color.FromArgb(189, 195, 199)))
            using (var highlightPen = new Pen(Color.White, 1.5f))
            {
                for (int i = 0; i < daysInMonth; i++)
                {
                    var data = dailyTotals[i];
                    float centerX = paddingLeft + (i * spacing) + (spacing / 2f);
                    float groupLeft = centerX - (groupWidth / 2f);

                    float incomeH = (float)((data.Income / maxVal) * chartHeight);
                    float expenseH = (float)((data.Expense / maxVal) * chartHeight);

                    float zeroY = paddingTop + chartHeight;

                    // Draw Income Bar (Left side of group)
                    if (incomeH > 0)
                    {
                        float x = groupLeft;
                        float y = zeroY - incomeH;
                        incomeBrush.ResetTransform();
                        incomeBrush.TranslateTransform(x, y);
                        incomeBrush.ScaleTransform(barW / 100f, incomeH / 100f);
                        g.FillRectangle(incomeBrush, x, y, barW, incomeH);
                    }

                    // Draw Expense Bar (Right side of group)
                    if (expenseH > 0)
                    {
                        float x = groupLeft + barW + 1;
                        float y = zeroY - expenseH;
                        expenseBrush.ResetTransform();
                        expenseBrush.TranslateTransform(x, y);
                        expenseBrush.ScaleTransform(barW / 100f, expenseH / 100f);
                        g.FillRectangle(expenseBrush, x, y, barW, expenseH);
                    }

                    // Highlight the day currently selected in Date Picker
                    if (data.Day == selectedDate.Day)
                    {
                        g.DrawRectangle(highlightPen, groupLeft - 1, paddingTop - 5, groupWidth + 2, chartHeight + 10);
                    }

                    // Draw Date Label below X Axis
                    if (data.Day % 2 != 0 || daysInMonth <= 15 || data.Day == selectedDate.Day) // prevent crowding of labels
                    {
                        string dayStr = data.Day.ToString();
                        SizeF labelSize = g.MeasureString(dayStr, labelFont);
                        g.DrawString(dayStr, labelFont, labelBrush, centerX - (labelSize.Width / 2f), zeroY + 8);
                    }
                }
            }
        }

        private void DrawSummaryCard(Graphics g, int x, int y, int w, int h, string title, double amount, Color accentColor)
        {
            // Draw background card boundary
            using (var cardBg = new SolidBrush(Color.FromArgb(45, 45, 75)))
            using (var borderPen = new Pen(accentColor, 1.5f))
            {
                g.FillRectangle(cardBg, x, y, w, h);
                g.DrawRectangle(borderPen, x, y, w, h);
            }

            // Draw title text
            using (var titleFont = new Font("Segoe UI", 8.5F, FontStyle.Regular))
            using (var textBrush = new SolidBrush(Color.FromArgb(200, 200, 200)))
            {
                g.DrawString(title, titleFont, textBrush, x + 10, y + 8);
            }

            // Draw amount text
            using (var amountFont = new Font("Segoe UI", 12F, FontStyle.Bold))
            using (var amountBrush = new SolidBrush(Color.White))
            {
                string sign = amount >= 0 && title == "Net Balance" ? "+" : "";
                string amtStr = $"{sign}Rp {amount:N0}";
                g.DrawString(amtStr, amountFont, amountBrush, x + 10, y + 28);
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

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            _isLoggingOut = true;
            this.Close();
            var loginForm = new LoginForm();
            loginForm.Show();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (!_isLoggingOut)
            {
                Application.Exit(); // Ensure full process shutdown when main form is closed
            }
        }
    }
}
