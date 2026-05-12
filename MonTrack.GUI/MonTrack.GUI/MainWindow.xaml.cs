using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using MonTrack.Models;
using MonTrack.Services;

namespace MonTrack.GUI;

public partial class MainWindow : Window
{
    private ExportApiService _exportService;
    private string _tempFolder;

    public MainWindow()
    {
        InitializeComponent();
        _exportService = new ExportApiService();
        _tempFolder = Path.Combine(Path.GetTempPath(), "MonTrack-GUI-Test");
        Directory.CreateDirectory(_tempFolder);
        
        // Update display saat slider berubah
        RecordCountSlider.ValueChanged += (s, e) => 
            RecordCountDisplay.Text = ((int)e.NewValue).ToString();
    }

    private async void BtnExport_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            BtnExport.IsEnabled = false;
            BtnExport.Content = "⏳ Processing...";
            
            int recordCount = (int)RecordCountSlider.Value;
            var dummyData = GenerateDummyTransactions(recordCount);
            var stopwatch = Stopwatch.StartNew();

            // Reset status
            CsvStatus.Text = "Status: Processing...";
            CsvDetails.Text = "";
            PdfStatus.Text = "Status: Processing...";
            PdfDetails.Text = "";
            SummaryText.Text = $"Processing {recordCount} records...";

            bool csvSuccess = false, pdfSuccess = false;
            string csvFile = "", pdfFile = "";

            // CSV Export
            if ((bool)CheckExportCSV.IsChecked)
            {
                var csvStopwatch = Stopwatch.StartNew();
                try
                {
                    csvFile = Path.Combine(_tempFolder, $"export_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                    await _exportService.ExecuteExport("CSV", dummyData, csvFile);
                    csvStopwatch.Stop();
                    
                    var fileInfo = new FileInfo(csvFile);
                    CsvStatus.Text = "✓ CSV Export Successful!";
                    CsvDetails.Text = $"File: {Path.GetFileName(csvFile)}\n" +
                                    $"Size: {fileInfo.Length / 1024.0:F2} KB\n" +
                                    $"Time: {csvStopwatch.ElapsedMilliseconds} ms";
                    csvSuccess = true;
                }
                catch (Exception ex)
                {
                    CsvStatus.Text = "✗ CSV Export Failed!";
                    CsvDetails.Text = ex.Message;
                }
            }

            // PDF Export
            if ((bool)CheckExportPDF.IsChecked)
            {
                var pdfStopwatch = Stopwatch.StartNew();
                try
                {
                    pdfFile = Path.Combine(_tempFolder, $"export_{DateTime.Now:yyyyMMdd_HHmmss_fff}.pdf");
                    await _exportService.ExecuteExport("PDF", dummyData, pdfFile);
                    pdfStopwatch.Stop();
                    
                    var fileInfo = new FileInfo(pdfFile);
                    PdfStatus.Text = "✓ PDF Export Successful!";
                    PdfDetails.Text = $"File: {Path.GetFileName(pdfFile)}\n" +
                                    $"Size: {fileInfo.Length / 1024.0:F2} KB\n" +
                                    $"Time: {pdfStopwatch.ElapsedMilliseconds} ms";
                    pdfSuccess = true;
                }
                catch (Exception ex)
                {
                    PdfStatus.Text = "✗ PDF Export Failed!";
                    PdfDetails.Text = ex.Message;
                }
            }

            stopwatch.Stop();

            // Summary
            int successCount = (csvSuccess ? 1 : 0) + (pdfSuccess ? 1 : 0);
            SummaryText.Text = $"✓ Export Completed!\n" +
                             $"Records: {recordCount:N0} | " +
                             $"Success: {successCount}/2 | " +
                             $"Total Time: {stopwatch.ElapsedMilliseconds} ms\n" +
                             $"Temp Folder: {_tempFolder}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            SummaryText.Text = $"Error: {ex.Message}";
        }
        finally
        {
            BtnExport.IsEnabled = true;
            BtnExport.Content = "▶ Start Export";
        }
    }

    private List<Transaction> GenerateDummyTransactions(int count)
    {
        var list = new List<Transaction>();
        for (int i = 1; i <= count; i++)
        {
            list.Add(new Transaction
            {
                Id = i,
                Date = DateTime.Now.AddMinutes(-i),
                Amount = i * 100.5,
                Category = (i % 2 == 0) ? "Income" : "Expense",
                Description = $"Dummy Transaction Record #{i}"
            });
        }
        return list;
    }
}