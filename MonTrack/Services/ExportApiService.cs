using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonTrack.Exporters;
using MonTrack.Models;

namespace MonTrack.Services
{
    /// <summary>
    /// Class sebagai implementasi teknik API internal untuk proses ekspor data.
    /// </summary>
    public class ExportApiService
    {
        private readonly Dictionary<string, IDataExporter> _exporters;

        public ExportApiService()
        {
            // Strategy Pattern: Menyiapkan berbagai strategi ekspor yang didukung.
            _exporters = new Dictionary<string, IDataExporter>(StringComparer.OrdinalIgnoreCase)
            {
                { "CSV", new CsvExporter() },
                { "PDF", new PdfExporter() }
                // Bisa ditambahkan exporter lain tanpa mengubah class ini (OCP).
            };
        }

        /// <summary>
        /// Mengeksekusi proses ekspor secara aman (Security validation).
        /// </summary>
        public async Task ExecuteExport(string format, List<Transaction> data, string path, bool isAuthenticated = true)
        {
            // Security Validation (Phase 3 Requirement)
            if (!isAuthenticated)
            {
                throw new UnauthorizedAccessException("Sesi tidak valid. Silakan login kembali untuk melakukan ekspor data.");
            }

            if (!_exporters.ContainsKey(format))
            {
                throw new NotSupportedException($"Format '{format}' is not supported.");
            }

            var exporter = _exporters[format];
            await Task.Run(() => exporter.Export(data, path));
        }
    }
}
