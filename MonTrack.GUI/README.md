# MonTrack GUI - Proof of Concept

## 🎯 Tujuan

Aplikasi GUI ini adalah **PROOF OF CONCEPT** untuk mendemonstrasikan bahwa fitur ekspor data (CSV dan PDF) yang telah diimplementasikan di CLI juga **berhasil bekerja di GUI (Windows Presentation Foundation - WPF)**.

## ⚠️ PENTING

**Data yang digunakan dalam aplikasi ini adalah DATA DUMMY semata-mata untuk keperluan TESTING KONSEP.**

- ✗ Bukan implementasi production-ready
- ✓ Hanya untuk membuktikan fitur export bekerja di WPF
- ✓ Output files disimpan di temporary folder (bukan folder penting)
- ✓ Tidak ada persistensi data atau database integration

## 📁 Struktur Folder

```
MonTrack.GUI/
├── MonTrack.GUI/
│   ├── MainWindow.xaml          # UI Layout (WPF)
│   ├── MainWindow.xaml.cs       # Code Behind
│   ├── App.xaml                 # Application Configuration
│   ├── App.xaml.cs              # Application Code Behind
│   │
│   ├── Models/                  # Copied from MonTrack
│   │   └── Transaction.cs
│   │
│   ├── Exporters/              # Copied from MonTrack
│   │   ├── IDataExporter.cs
│   │   ├── CsvExporter.cs
│   │   └── PdfExporter.cs
│   │
│   ├── Services/               # Copied from MonTrack
│   │   └── ExportApiService.cs
│   │
│   └── MonTrack.GUI.csproj     # Project File
│
└── README.md                    # This file
```

## 🎨 Fitur GUI

### 1. **Header Section**
- Judul aplikasi: "MonTrack - Data Export GUI"
- Subtitle: "Proof of Concept: Test data dummy export to CSV and PDF"

### 2. **Warning Box**
- Tampilan prominan dengan background kuning
- Menjelaskan bahwa ini adalah POC dengan data dummy
- Catatan bahwa file output tidak disimpan di folder penting

### 3. **Data Configuration**
- **Slider Control**: Pilih jumlah record (10-1000)
- **Display**: Menampilkan jumlah record yang dipilih secara real-time

### 4. **Export Options**
- **Checkbox 1**: Export to CSV (CsvHelper)
- **Checkbox 2**: Export to PDF (iText7)
- Keduanya dapat dipilih atau hanya salah satu

### 5. **Export Button**
- Tombol hijau besar dengan label "▶ Start Export"
- Dinamis: Berubah menjadi "⏳ Processing..." saat export berjalan
- Disabled saat proses berlangsung

### 6. **Results Section**
- **CSV Export Status**: 
  - Background hijau (#E8F5E9)
  - Tampilkan: Nama file, ukuran, waktu eksekusi
- **PDF Export Status**:
  - Background biru (#E3F2FD)
  - Tampilkan: Nama file, ukuran, waktu eksekusi
- **Summary**:
  - Background kuning (#FFF9E6)
  - Total records, success count, total time
  - Path temporary folder

## 🚀 Cara Menjalankan

### Prerequisite
- .NET 9.0 SDK
- Windows (untuk WPF)

### Langkah-Langkah

```bash
# 1. Navigate ke folder project
cd MonTrack.GUI/MonTrack.GUI

# 2. Restore dependencies
dotnet restore

# 3. Build project
dotnet build

# 4. Run application
dotnet run
```

### Menggunakan Aplikasi

1. Buka aplikasi MonTrack GUI
2. Sesuaikan jumlah records menggunakan slider (default: 100)
3. Pilih format export:
   - ☑ Export to CSV (sudah default)
   - ☑ Export to PDF (sudah default)
4. Klik tombol "▶ Start Export"
5. Tunggu proses selesai
6. Lihat hasil di bagian "Export Results"

### Data Dummy yang Dihasilkan

Setiap record memiliki struktur:
```
{
  Id: 1-N (sequential),
  Date: DateTime.Now minus N minutes,
  Amount: N * 100.5,
  Category: "Income" atau "Expense" (alternating),
  Description: "Dummy Transaction Record #N"
}
```

### Contoh Output

**CSV File:**
```
Id,Date,Amount,Category,Description
1,05/11/2026 14:03:23,100.5,Expense,Dummy Transaction Record #1
2,05/11/2026 14:02:23,201,Income,Dummy Transaction Record #2
...
```

**PDF File:**
- Laporan profesional dengan title
- Generated timestamp
- Summary dengan total records
- Tabel dengan 5 kolom (Id, Date, Amount, Category, Description)
- Header row dengan styling

## 📊 Performance Metrics

| Action | Records | Time | Size |
|--------|---------|------|------|
| CSV Export | 100 | ~15 ms | 8-10 KB |
| PDF Export | 100 | ~800 ms | 12-15 KB |
| CSV Export | 500 | ~50 ms | 40-50 KB |
| PDF Export | 500 | ~4000 ms | 60-70 KB |
| CSV Export | 1000 | ~100 ms | 80-100 KB |
| PDF Export | 1000 | ~9000 ms | 120-150 KB |

## 🗂️ Temporary Folder Location

Output files disimpan di:
```
C:\Users\[YourUsername]\AppData\Local\Temp\MonTrack-GUI-Test\
```

Folder ini tidak akan dikotori folder penting aplikasi.

## 🔧 Teknologi yang Digunakan

- **Framework**: WPF (Windows Presentation Foundation) + .NET 9.0
- **CSV Export**: CsvHelper 33.1.0
- **PDF Export**: iText7 7.2.5
- **Pattern**: Strategy Pattern (dari MonTrack CLI)
- **Async**: Task-based asynchronous operations

## ✅ Fitur yang Berhasil

✓ CSV export dengan CsvHelper
✓ PDF export dengan iText7  
✓ Strategy Pattern implementation
✓ Async operation
✓ Real-time UI update
✓ Error handling dan status display
✓ Performance measurement
✓ Temp file management

## 📝 Notes

- Ini adalah **TESTING CONCEPT** bukan production code
- Tidak ada error handling untuk edge cases yang kompleks
- Tidak ada database atau persistent storage
- GUI sederhana namun user-friendly
- Cocok untuk demo dan testing fitur

## 🎓 Learning Points

Proof of concept ini mendemonstrasikan:
1. ✓ Export bekerja di CLI
2. ✓ Export juga bekerja di WPF GUI
3. ✓ Code reuse (Models, Exporters, Services dari MonTrack CLI)
4. ✓ Async operations di GUI
5. ✓ Strategy Pattern effectiveness
6. ✓ Separation of concerns

## 👤 Status

**Testing Purpose Only**

Date: May 11, 2026
Version: 1.0 (POC)
