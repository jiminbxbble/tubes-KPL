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

        public MainForm()
        {
            // Initialize Core Services
            _repo = new DataRepository<Transaction>();
            _financeManager = new TransactionManager(_repo);
            _exportService = new ExportApiService();

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
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
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
            tabExport = new TabPage { Text = " Data Export ", BackColor = Color.FromArgb(26, 26, 46) };

            tabControl.TabPages.Add(tabTransactions);
            tabControl.TabPages.Add(tabReminders);
            tabControl.TabPages.Add(tabExport);

            // --- Tab 1: Transactions Layout ---
            // Left Column: Entry Form
            Panel inputPanel = new Panel
            {
                Location = new Point(15, 15),
                Size = new Size(280, 480),
                BackColor = Color.FromArgb(34, 34, 59),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
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
                Location = new Point(15, 55),
                Size = new Size(250, 20)
            };

            txtAmount = new EntryTextBox
            {
                Location = new Point(15, 75),
                Size = new Size(250, 28)
            };

            lblCategory = new Label
            {
                Text = "Category",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 115),
                Size = new Size(250, 20)
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(15, 135),
                Size = new Size(250, 28),
                BackColor = Color.FromArgb(22, 22, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.Items.AddRange(new object[] { "Pemasukan", "Pengeluaran" });
            cmbCategory.SelectedIndex = 0;

            lblDescription = new Label
            {
                Text = "Description",
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(15, 175),
                Size = new Size(250, 20)
            };

            txtDescription = new EntryTextBox
            {
                Location = new Point(15, 195),
                Size = new Size(250, 28)
            };

            btnRecord = new Button
            {
                Text = "Record Transaction",
                BackColor = Color.FromArgb(78, 49, 170),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(15, 250),
                Size = new Size(250, 45),
                FlatStyle = FlatStyle.Flat
            };
            btnRecord.FlatAppearance.BorderSize = 0;
            btnRecord.Click += BtnRecord_Click;

            lblTxStatus = new Label
            {
                ForeColor = Color.FromArgb(255, 118, 117),
                Location = new Point(15, 310),
                Size = new Size(250, 50),
                TextAlign = ContentAlignment.TopLeft
            };

            inputPanel.Controls.Add(lblNewTxTitle);
            inputPanel.Controls.Add(lblAmount);
            inputPanel.Controls.Add(txtAmount);
            inputPanel.Controls.Add(lblCategory);
            inputPanel.Controls.Add(cmbCategory);
            inputPanel.Controls.Add(lblDescription);
            inputPanel.Controls.Add(txtDescription);
            inputPanel.Controls.Add(btnRecord);
            inputPanel.Controls.Add(lblTxStatus);

            // Right Column: Filter Buttons & ListView
            Panel filterPanel = new Panel
            {
                Location = new Point(310, 15),
                Size = new Size(535, 40),
                BackColor = Color.FromArgb(26, 26, 46),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
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
                BorderStyle = BorderStyle.None,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
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
                BorderStyle = BorderStyle.None,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
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
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            btnMarkPaid.FlatAppearance.BorderSize = 0;
            btnMarkPaid.Click += BtnMarkPaid_Click;

            lblReminderStatus = new Label
            {
                ForeColor = Color.FromArgb(85, 239, 196),
                Location = new Point(245, 440),
                Size = new Size(590, 45),
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            tabReminders.Controls.Add(lvReminders);
            tabReminders.Controls.Add(btnMarkPaid);
            tabReminders.Controls.Add(lblReminderStatus);

            // --- Tab 3: Data Export Layout ---
            exportCard = new Panel
            {
                Location = new Point(225, 75),
                Size = new Size(410, 350),
                BackColor = Color.FromArgb(34, 34, 59),
                Anchor = AnchorStyles.None
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

            // Add main panels
            this.Controls.Add(tabControl);
            this.Controls.Add(headerPanel);
        }

        private void RefreshData()
        {
            // Update Balance Display
            double balance = _financeManager.GetCurrentBalance();
            lblBalance.Text = $"Current Balance: Rp {balance:N0}";

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

        private void BtnRecord_Click(object? sender, EventArgs e)
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

        private void BtnMarkPaid_Click(object? sender, EventArgs e)
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

        private async void BtnExport_Click(object? sender, EventArgs e)
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
            string projectRoot = @"d:\4. Thoriq_KULIAH\4. Matkul\Semester 4\LKPL\TUBES-Thoriq\tubes-KPL";
            string exportFolder = Path.Combine(projectRoot, "_Output", "Reports");
            Directory.CreateDirectory(exportFolder);

            btnExport.Text = "Exporting data...";
            lblExportStatus.ForeColor = Color.White;
            lblExportStatus.Text = "Preparing file structure...";

            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                if (format == "CSV")
                {
                    string path = Path.Combine(exportFolder, $"export_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                    await _exportService.ExecuteExport("CSV", exportData, path);
                    stopwatch.Stop();
                    DisplayExportResult(path, stopwatch.ElapsedMilliseconds, exportData.Count, "CSV");
                }
                else if (format == "PDF")
                {
                    string path = Path.Combine(exportFolder, $"export_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
                    await _exportService.ExecuteExport("PDF", exportData, path);
                    stopwatch.Stop();
                    DisplayExportResult(path, stopwatch.ElapsedMilliseconds, exportData.Count, "PDF");
                }
                else // Both
                {
                    string csvPath = Path.Combine(exportFolder, $"export_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                    string pdfPath = Path.Combine(exportFolder, $"export_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
                    
                    await _exportService.ExecuteExport("CSV", exportData, csvPath);
                    await _exportService.ExecuteExport("PDF", exportData, pdfPath);
                    
                    stopwatch.Stop();
                    lblExportStatus.ForeColor = Color.FromArgb(85, 239, 196);
                    lblExportStatus.Text = $"✓ Both CSV & PDF exported successfully!\n" +
                                           $"  Records: {exportData.Count}\n" +
                                           $"  Time: {stopwatch.ElapsedMilliseconds} ms\n" +
                                           $"  Folder: {exportFolder}";
                }
            }
            catch (Exception ex)
            {
                lblExportStatus.ForeColor = Color.FromArgb(255, 118, 117);
                lblExportStatus.Text = $"Export failed: {ex.Message}";
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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit(); // Ensure full process shutdown when main form is closed
        }
    }
}
