# MonTrack - Pengelola Keuangan

Aplikasi pengelola keuangan berbasis C# [.NET] yang fokus pada fitur ekspor data transaksi dengan implementasi prinsip SOLID dan Design by Contract.

## Fitur Utama
- **FR-08: Ekspor Data Transaksi**: Mendukung format CSV menggunakan library `CsvHelper`.
- **Strategy Pattern**: Arsitektur yang memudahkan penambahan format ekspor baru (JSON, PDF, dll).
- **Asynchronous Processing**: Proses ekspor dijalankan di background agar tidak memblokir thread utama.
- **Unit Testing**: Pengujian otomatis menggunakan NUnit.
- **Performance Testing**: Dilengkapi dengan pengukur waktu eksekusi untuk pengolahan data dalam jumlah besar.

## Arsitektur & Teknik
- **SOLID Principles**: Khususnya Open/Closed Principle pada interface `IDataExporter`.
- **Design by Contract (DbC)**: Validasi pre-condition pada parameter input.
- **Defensive Programming**: Penanganan error dengan blok try-catch yang kuat.
- **Code Reuse**: Pemanfaatan library pihak ketiga (CsvHelper).

## Cara Menjalankan
1. Clone repository:
   ```bash
   git clone https://github.com/Thbetyfu/RKPL-TUBES.git
   ```
2. Restore package:
   ```bash
   dotnet restore
   ```
3. Jalankan pengujian performa:
   ```bash
   dotnet run
   ```
4. Jalankan unit test:
   ```bash
   dotnet test
   ```

## tugas
Tugas Besar MonTrack