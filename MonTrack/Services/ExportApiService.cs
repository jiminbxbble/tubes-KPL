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
        /// Mengeksekusi proses ekspor secara asinkron (Performance quality attribute).
        /// </summary>
        public async Task ExecuteExport(string format, List<Transaction> data, string path)
        {
            if (!_exporters.ContainsKey(format))
            {
                throw new NotSupportedException($"Format '{format}' is not supported.");
            }

            // Memilih strategi exporter berdasarkan parameter format (Strategy Pattern).
            var exporter = _exporters[format];

            // Menjalankan di background thread agar tidak memblokir thread utama (Async/Performance).
            await Task.Run(() => exporter.Export(data, path));
        }
    }
}
