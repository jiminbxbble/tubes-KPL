using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonTrack.Exporters;
using MonTrack.Models;

namespace MonTrack.Services
{
    /// <summary>
    /// Layanan API internal untuk memfasilitasi ekspor data transaksi.
    /// Menerapkan Strategy Pattern untuk memenuhi Open/Closed Principle (OCP) pada spesifikasi CLO4.
    /// Mengapa: Agar penambahan format ekspor baru (misal: Excel/JSON) di masa depan tidak perlu mengubah
    /// logika kelas ini, melainkan cukup membuat implementasi baru dari interface IDataExporter.
    /// </summary>
    public class ExportApiService
    {
        private readonly Dictionary<string, IDataExporter> _exporters;

        public ExportApiService()
        {
            // Mengapa menggunakan Dictionary sebagai map strategi:
            // Memudahkan penambahan strategi baru secara dinamis dan pencarian dengan kompleksitas O(1)
            // tanpa memerlukan struktur pengkondisian percabangan (switch/if-else) yang rumit.
            _exporters = new Dictionary<string, IDataExporter>(StringComparer.OrdinalIgnoreCase)
            {
                { "CSV", new CsvExporter() },
                { "PDF", new PdfExporter() }
            };
        }

        /// <summary>
        /// Mengeksekusi proses ekspor data secara asinkron berdasarkan format yang dipilih.
        /// Mengapa: Menjalankan ekspor di thread terpisah (Task.Run) agar thread UI utama tidak membeku (freeze)
        /// saat memproses data berukuran besar, menjaga user experience tetap responsif.
        /// </summary>
        /// <param name="format">Format ekspor target (CSV/PDF).</param>
        /// <param name="data">Daftar transaksi yang akan diekspor.</param>
        /// <param name="path">Jalur penyimpanan berkas output.</param>
        /// <param name="isAuthenticated">Status autentikasi sesi pengguna.</param>
        public async Task ExecuteExport(string format, List<Transaction> data, string path, bool isAuthenticated = true)
        {
            // Mengapa validasi keamanan diletakkan di sini:
            // Mencegah kebocoran data sensitif apabila API ini diakses dari luar sesi masuk resmi (Secure Coding).
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
