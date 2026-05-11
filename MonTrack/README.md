# MonTrack - Pengelola Keuangan

Aplikasi pengelola keuangan berbasis C# [.NET] yang fokus pada fitur ekspor data transaksi dengan implementasi prinsip SOLID dan Design by Contract.

## Fitur Utama
- **FR-08: Ekspor Data Transaksi**: Mendukung format CSV dan PDF.
  - **CSV Export**: Menggunakan library `CsvHelper` untuk ekspor ke format CSV.
  - **PDF Export**: Menggunakan library `iText7` untuk menghasilkan laporan PDF profesional.
- **Strategy Pattern**: Arsitektur yang memudahkan penambahan format ekspor baru (JSON, Excel, dll).
- **Interactive Menu**: Pilih format ekspor (CSV, PDF, atau keduanya) atau jalankan performance test.
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
   git clone https://github.com/jiminbxbble/tubes-KPL.git
   cd tubes-KPL/MonTrack
   ```
2. Restore package:
   ```bash
   dotnet restore
   ```
3. Jalankan aplikasi (Interactive Menu):
   ```bash
   dotnet run
   ```
   - Pilihan 1: Export ke CSV
   - Pilihan 2: Export ke PDF
   - Pilihan 3: Export ke CSV dan PDF (keduanya)
   - Pilihan 4: Performance test dengan 10,000 records

4. Jalankan unit test:
   ```bash
   dotnet test
   ```

## Contoh Output

### CSV Export
- File: `export_data.csv`
- Format: Id, Date, Amount, Category, Description
- Ukuran: ~67 KB untuk 1000 records
- Waktu: ~111 ms

### PDF Export
- File: `export_data.pdf`
- Format: Laporan profesional dengan tabel terstruktur
- Ukuran: ~88 KB untuk 1000 records
- Waktu: ~9.8 detik

### Performance Test (10,000 Records)
```
Jumlah Data     : 10,000 transaksi
Waktu Eksekusi  : 140 ms
Throughput      : ~71,428 records/second
Ukuran File     : ~720 KB
```

## Struktur Proyek

```
MonTrack/
├── Exporters/
│   ├── IDataExporter.cs       # Interface untuk semua exporter
│   ├── CsvExporter.cs         # CSV export implementation
│   └── PdfExporter.cs         # PDF export implementation (NEW!)
├── Models/
│   └── Transaction.cs         # Model data transaksi
├── Services/
│   └── ExportApiService.cs    # API service dengan Strategy Pattern
├── Tests/
│   └── ExportTests.cs         # Unit tests
├── Program.cs                 # Entry point dengan interactive menu
├── BRACH.csproj              # Project configuration
└── README.md                 # This file
```

## tugas
Tugas Besar MonTrack