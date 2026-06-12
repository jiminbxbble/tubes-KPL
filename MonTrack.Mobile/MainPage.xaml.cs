using System.Diagnostics;
using MonTrack.Models;
using MonTrack.Services;
using PencatatanKeuangan.Services;
using PencatatanKeuangan.Repositories;

namespace MonTrack.Mobile;

public partial class MainPage : ContentPage
{
	private ExportApiService _exportService = null!;
	private TransactionManager _financeManager = null!;

	public MainPage()
	{
		InitializeComponent();

		_exportService = new ExportApiService();
		var repo = new DataRepository<PencatatanKeuangan.Models.Transaction>();
		_financeManager = new TransactionManager(repo);

		// Seed initial data
		_financeManager.RecordTransaction(15000000, "Gaji", "Monthly Salary");
		_financeManager.RecordTransaction(50000, "Makan", "Coffee");
	}

	private async void BtnExport_Clicked(object sender, EventArgs e)
	{
		try
		{
			// Simulated data for export
			var exportData = new List<Transaction>();
			for (int i = 1; i <= 20; i++)
			{
				exportData.Add(new Transaction {
					Id = i,
					Date = DateTime.Now.AddDays(-i),
					Amount = 100000 * i,
					Category = i % 2 == 0 ? "Income" : "Expense",
					Description = $"Mobile Transaction #{i}"
				});
			}

			string projectRoot = @"d:\4. Thoriq_KULIAH\4. Matkul\Semester 4\LKPL\TUBES-Thoriq\tubes-KPL";
			string exportFolder = Path.Combine(projectRoot, "_Output", "Reports");
			Directory.CreateDirectory(exportFolder);
			
			string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
			string csvPath = Path.Combine(exportFolder, $"report_{timeStamp}.csv");
			string pdfPath = Path.Combine(exportFolder, $"report_{timeStamp}.pdf");
			
			// Ekspor ke CSV
			await _exportService.ExecuteExport("CSV", exportData, csvPath, true);
			
			// Ekspor ke PDF
			await _exportService.ExecuteExport("PDF", exportData, pdfPath, true);
			
			await DisplayAlert("Export Success", $"Laporan berhasil dibuat dalam 2 format:\n1. CSV: report_{timeStamp}.csv\n2. PDF: report_{timeStamp}.pdf\n\nLokasi: _Output/Reports", "Selesai");
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}
	}

	private async void BtnPerfTest_Clicked(object sender, EventArgs e)
	{
		try
		{
			BtnPerfTest.IsEnabled = false;
			BtnPerfTest.Text = "Running Stress Test...";
			
			int count = 100000;
			var stopwatch = Stopwatch.StartNew();

			await Task.Run(() => {
				for (int i = 0; i < count; i++)
				{
					_financeManager.RecordTransaction(1000 * (i % 10 + 1), "Test", $"Performance Record #{i}");
				}
			});

			stopwatch.Stop();
			await DisplayAlert("Performance Result", 
				$"Successfully processed and saved {count:N0} records.\n" +
				$"Total Time: {stopwatch.ElapsedMilliseconds} ms\n" +
				$"Avg Speed: {stopwatch.ElapsedMilliseconds / (double)count:F4} ms/record", "Great!");
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}
		finally
		{
			BtnPerfTest.IsEnabled = true;
			BtnPerfTest.Text = "Run Stress Test (100K Records)";
		}
	}
}
