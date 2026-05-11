using System;
using System.Collections.Generic;
using MonTrack.Models;

namespace MonTrack.Services
{
    /// <summary>
    /// Interface IDataExporter mendefinisikan kontrak untuk semua implementasi eksporter.
    /// 
    /// Teknik: Design by Contract (DbC)
    /// - Pre-condition: Memastikan input valid sebelum proses
    /// - Post-condition: Memastikan output/efek samping sesuai ekspektasi
    /// - Invariant: Menjaga konsistensi state object
    /// 
    /// Prinsip: Open/Closed Principle (OCP)
    /// - Terbuka untuk ekstensi (bisa add eksporter baru)
    /// - Tertutup untuk modifikasi (interface tidak berubah)
    /// </summary>
    public interface IDataExporter
    {
        /// <summary>
        /// Mengekspor list transaksi ke file dengan format tertentu.
        /// 
        /// PRE-CONDITION (Design by Contract):
        /// - transactions tidak boleh null
        /// - transactions tidak boleh kosong (Count > 0)
        /// - filePath tidak boleh null
        /// - filePath tidak boleh whitespace only
        /// 
        /// POST-CONDITION:
        /// - File berhasil dibuat di filePath
        /// - File berisi data transaksi dalam format sesuai implementasi
        /// 
        /// </summary>
        /// <param name="transactions">List transaksi yang akan diekspor (tidak boleh null atau kosong)</param>
        /// <param name="filePath">Path file tujuan (tidak boleh null atau whitespace)</param>
        /// <returns>True jika ekspor berhasil, false jika gagal</returns>
        /// <exception cref="ArgumentNullException">Jika transactions atau filePath null</exception>
        /// <exception cref="ArgumentException">Jika transactions kosong atau filePath whitespace</exception>
        /// <exception cref="System.IO.IOException">Jika terjadi error saat menulis file</exception>
        bool Export(List<Transaction> transactions, string filePath);

        /// <summary>
        /// Mendapatkan deskripsi format eksporter ini.
        /// </summary>
        /// <returns>String deskripsi format (contoh: "Comma-Separated Values")</returns>
        string GetFormatDescription();

        /// <summary>
        /// Mendapatkan ekstension file yang dihasilkan eksporter ini.
        /// </summary>
        /// <returns>Ekstension file (contoh: ".csv")</returns>
        string GetFileExtension();
    }
}
