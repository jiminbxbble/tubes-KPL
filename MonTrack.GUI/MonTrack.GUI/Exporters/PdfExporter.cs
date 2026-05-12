using System;
using System.Collections.Generic;
using System.IO;
using MonTrack.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace MonTrack.Exporters
{
    /// <summary>
    /// PDF Exporter implementation using iText7 library.
    /// Demonstrates Design by Contract: Pre-conditions and Post-conditions.
    /// </summary>
    public class PdfExporter : IDataExporter
    {
        /// <summary>
        /// Exports transactions to a PDF file.
        /// Design by Contract (DbC):
        /// - Precondition: transactions list must not be null, filePath must not be empty
        /// - Postcondition: PDF file must be created at the specified path
        /// </summary>
        public void Export(List<Transaction> transactions, string filePath)
        {
            // Precondition checks (Design by Contract)
            if (transactions == null)
                throw new ArgumentNullException(nameof(transactions), "Transactions list cannot be null");
            
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty", nameof(filePath));

            if (transactions.Count == 0)
                throw new ArgumentException("Transactions list cannot be empty", nameof(transactions));

            try
            {
                // Create PDF document
                using (var pdfWriter = new PdfWriter(filePath))
                using (var pdfDocument = new PdfDocument(pdfWriter))
                using (var document = new Document(pdfDocument))
                {
                    // Add title
                    var title = new Paragraph("MONTRACK - TRANSACTION EXPORT REPORT")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(16)
                        .SetBold();
                    document.Add(title);

                    // Add generation date
                    var dateInfo = new Paragraph($"Generated: {DateTime.Now:F}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(10);
                    document.Add(dateInfo);

                    // Add summary
                    var summary = new Paragraph($"Total Records: {transactions.Count} transactions\n")
                        .SetFontSize(11);
                    document.Add(summary);

                    // Create table
                    var table = new Table(5); // 5 columns: Id, Date, Amount, Category, Description
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    // Add header row
                    table.AddHeaderCell(CreateHeaderCell("Id"));
                    table.AddHeaderCell(CreateHeaderCell("Date"));
                    table.AddHeaderCell(CreateHeaderCell("Amount"));
                    table.AddHeaderCell(CreateHeaderCell("Category"));
                    table.AddHeaderCell(CreateHeaderCell("Description"));

                    // Add data rows
                    foreach (var transaction in transactions)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Id.ToString())));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Date.ToString("g"))));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Amount.ToString("N2"))));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Category)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.Description)));
                    }

                    document.Add(table);
                }

                // Postcondition check: File must exist
                if (!File.Exists(filePath))
                    throw new InvalidOperationException($"PDF file was not created at {filePath}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to export to PDF: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Helper method to create header cell with styling.
        /// </summary>
        private static Cell CreateHeaderCell(string text)
        {
            return new Cell()
                .Add(new Paragraph(text).SetBold())
                .SetBackgroundColor(new iText.Kernel.Colors.DeviceRgb(200, 200, 200))
                .SetTextAlignment(TextAlignment.CENTER);
        }
    }
}
