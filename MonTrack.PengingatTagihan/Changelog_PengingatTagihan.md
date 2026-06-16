# Catatan Update File - Raissha

Tanggal: 14 Juni 2026
Fitur: Refactor Modul Pengingat Tagihan

Semua perubahan yang signifikan pada proyek `MonTrack_PengingatTagihan` akan didokumentasikan dalam file ini.

## 📝 Ringkasan Perubahan

Proses *refactoring* berfokus pada peningkatan kualitas kode (*code quality*), keandalan sistem (*reliability*), tata kelola state yang lebih formal, serta penyediaan fungsionalitas manajemen data yang lebih utuh. Berikut adalah poin-poin perubahan arsitektural yang signifikan:

* **Penerapan Pola Desain *Table-Driven Construction*:** Mengganti struktur pemetaan data kategori tagihan yang sebelumnya berbasis instansiasi manual/acak menjadi tabel berbasis `Dictionary` yang terorganisasi berdasarkan grup kategori utama (seperti *Utilitas*, *Layanan digital*, *Pendidikan*, dll.).
* **Implementasi Mesin Status Formal (*Finite State Automata*):** Logika pembaruan status tagihan (`TagihanState`) yang sebelumnya menggunakan pemeriksaan kondisi konvensional (`if-else`) kini digantikan oleh mekanisme *State Transition Table* yang digerakkan oleh pemicu (*Trigger*) struktural.
* **Penyediaan Fitur CRUD Lengkap Melalui Kelas Kontroler Baru:** Memperkenalkan kelas `TagihanManager` untuk memisahkan logika bisnis manajemen koleksi data (Penyimpanan, Pembacaan, Pembaruan, Penghapusan) dari kelas representasi data dasar (`PengingatTagihan`).
* **Perubahan Paradigma Antarmuka Pengguna (CLI):** Mengubah kelas `Program` utama dari yang awalnya berupa skrip eksekusi statis dan pengujian performa temporer menjadi aplikasi *Command Line Interface* (CLI) interaktif berbasis menu (*menu-driven loop*).
* **Penguatan Aspek Keamanan Kode (*Defensive Programming* & *Design by Contract*):** * Penggunaan `Debug.Assert` secara ketat untuk memvalidasi prasyarat (*pre-conditions*) pembuatan objek.
    * Enkapsulasi yang lebih ketat dengan membatasi akses pengubah properti menjadi `private set`.
    * Validasi masukan dinamis pengguna (seperti parse format tanggal kustom) untuk menghindari kegagalan sistem saat berjalan runtime.
* **Peluasan Cakupan Pengujian (*Unit Testing*):** Menambahkan skenario pengujian komprehensif untuk memastikan validitas fitur manajemen data baru (CRUD).

---

## 📁 Rincian File yang Diubah / Ditambah

### A. Berkas: `PengingatTagihan.cs` (Modifikasi Struktur & Penambahan Kelas)

Berkas ini mengalami restrukturisasi besar-besaran untuk menerapkan pola desain *State Automata* dan pemisahan tanggung jawab kode (*Separation of Concerns*).

#### 1. Kelas `KonfigurasiTagihan` (Dimodifikasi)
* **Perubahan:** Menyederhanakan struktur model data konfigurasi.
* **Sebelum:** Memiliki properti `Kelompok` (string) dan `HariTenggatWaktu` (int).
* **Sesudah:** Hanya menyimpan properti `TenggatWaktuDefault` (int) karena pengelompokan sudah dipetakan langsung sebagai kunci (*key*) utama di dalam *dictionary*.

#### 2. Penambahan Komponen Automata Baru (Ditambahkan)
* **`enum Trigger`:** Menambahkan enumerasi baru untuk mendefinisikan pemicu perubahan status tagihan:
    * `WaktuJatuhTempo`: Dipicu saat melewati batas waktu bayar.
    * `BayarSekarang`: Dipicu ketika pengguna menyelesaikan pembayaran.
* **`class TransisiState`:** Membuat kelas model pendukung untuk memetakan transisi status secara deklaratif (`StateAwal`, `Trigger`, `StateAkhir`).

#### 3. Kelas `PengingatTagihan` (Dimodifikasi)
* **Enkapsulasi Data:** Mengubah tingkat akses modifier *setter* properti utama (`Nama`, `Kategori`, `Nominal`, `TanggalDibuat`) dari publik menjadi `private set`. Properti hanya bisa dimodifikasi dari dalam objek melalui metode yang sah.
* **Identitas Objek:** Menambahkan properti `Id` (int) sebagai pengenal unik mutlak yang sangat penting untuk operasi basis data/manajemen koleksi.
* **Tabel Konfigurasi Baru:** `TabelKonfigurasi` diubah konfigurasinya menjadi berbasis grup besar (Utilitas, Layanan digital, Pendidikan, Finansial & Cicilan, Asuransi & Kesehatan) dengan nilai tenggat waktu standar (30 hari).
* **Mekanisme Transisi Status:**
    * Menambahkan properti statis read-only `TabelTransisiAutomata` yang mendeklarasikan aturan perpindahan status tagihan secara formal.
    * Menambahkan metode privat `UbahState(Trigger trigger)` yang mengecek keabsahan perpindahan status berdasarkan tabel transisi sebelum mengubah nilai `StatusSaatIni`.
* **Alur Validasi Baru:**
    * Memisahkan logika inisialisasi ke metode internal `SetupDetail`.
    * Menambahkan metode `UpdateDetail` untuk mendukung proses penyuntingan data oleh pengguna.
    * Mengganti metode `UpdateStatusBerdasarkanWaktu()` menjadi `CekWaktuJatuhTempo()` yang memanfaatkan pemicu `Trigger.WaktuJatuhTempo`.

#### 4. Kelas `TagihanManager` (Baru Ditambahkan)
Sebuah kelas manajer baru yang berfungsi sebagai repositori lokal untuk mengelola operasi data (*CRUD*):
* `daftarTagihan`: Koleksi bertipe `List<PengingatTagihan>` untuk menyimpan memori data runtime.
* `CreateTagihan(...)`: Membuat instansiasi baru, otomatis memberikan ID urut berkelanjutan (`_nextId++`), menangani kesalahan pembuatan secara defensif.
* `GetSemuaTagihan()`: Mengembalikan seluruh data tagihan sekaligus melakukan sinkronisasi status keterlambatan secara otomatis sebelum data dikembalikan.
* `UpdateTagihan(...)`: Mencari data berdasarkan ID tertentu, melakukan pembaruan field secara aman melalui metode internal objek.
* `DeleteTagihan(...)`: Menghapus data dari daftar koleksi berdasarkan ID spesifik. Dilengkapi validasi defensif throwing `ArgumentException` jika ID tidak terdaftar.

---

### B. Berkas: `TagihanTests.cs` (Pembaruan Unit Testing)

Pengujian unit diperluas untuk memastikan fungsionalitas baru tidak merusak logika yang sudah ada (*regression prevention*) serta memastikan keandalan fungsi manajer.

#### 1. Kelas `TagihanTests` (Dimodifikasi)
* **Pembaruan Parameter Konstruktor:** Mengubah inisialisasi objek uji `new PengingatTagihan(...)` agar sesuai dengan tanda tangan (*signature*) konstruktor baru (menyertakan ID dan objek tanggal jatuh tempo eksplisit/nullable).
* **Penyesuaian Logika:** Metode pengujian terlambat sekarang memanggil fungsi `tagihan.CekWaktuJatuhTempo()` secara eksplisit sebelum melakukan asersi status.

#### 2. Kelas `TagihanManagerTests` (Baru Ditambahkan)
Kelas pengujian baru khusus ditujukan untuk memvalidasi siklus hidup data pada objek kontoler `TagihanManager`:
* `TestTambahTagihanMenambahJumlahData`: Memastikan bahwa metode `CreateTagihan` berhasil memasukkan objek ke koleksi list dan menambah total ukuran data.
* `TestUpdateTagihanMengubahDetailData`: Memastikan metode `UpdateTagihan` secara tepat mengubah isi konten data (Nama, Nominal, dan Tanggal) berdasarkan target ID yang sesuai.
* `TestDeleteTagihanMengurangiJumlahData`: Memastikan bahwa metode `DeleteTagihan` berhasil menghapus objek dari list dan menyisakan ukuran koleksi yang tepat.

---

### C. Berkas: `Program.cs` (Peralihan Total Fungsionalitas)

Berkas utama aplikasi ditulis ulang sepenuhnya untuk mengubah fungsi aplikasi dari kebutuhan internal developer (*benchmark/testing*) menjadi modul aplikasi akhir (*production-ready system*).

* **Penghapusan Simulasi Performa:** Menghapus seluruh blok kode pengujian beban tinggi (`Stopwatch`, pengulangan 1000 iterasi data acak, dan pencetakan waktu eksekusi milidetik).
* **Implementasi CLI Menu Loop:** Menambahkan sistem navigasi interaktif berbasis teks menggunakan kombinasi perulangan `while (aplikasiJalan)` dan percabangan `switch (pilihan)` dari menu input `0` sampai `5`.
* **Fungsi Pembantu Visual Dinamis:** Menambahkan metode statis `TampilkanSemuaTagihan(TagihanManager manager)` untuk mencetak ringkasan seluruh data dalam bentuk format tabel konsol yang rapi lengkap dengan alignment kolom teks.
* **Metode Validasi Input `InputTanggal` (Defensive Programming):**
    * Menambahkan metode penanganan input tanggal kustom secara interaktif.
    * Mendukung multi-format input teks: `dd-MM-yyyy`, `dd/MM/yyyy`, atau `dd MM yyyy`.
    * Menggunakan `DateTime.TryParseExact` di dalam perulangan tak terbatas (`while (true)`) untuk memaksa pengguna memasukkan format tanggal yang legal, mencegah aplikasi *crash* akibat kesalahan input teks manual dari user.


# Catatan Update File - Raissha

Tanggal: 16 Juni 2026
Fitur: Refactor Modul Pengingat Tagihan - Bagian 2 (Penerapan clean code)

## 📝 Ringkasan Perubahan
Pembaruan ini berfokus pada perbaikan struktur kode dengan menerapkan prinsip *Clean Code* tanpa mengubah arsitektur inti (*Table-Driven*, *Automata*, *Design by Contract*, dan *Defensive Programming*). Pembaruan utama meliputi:
* **Penghapusan Magic Strings:** Menggunakan konstanta terpusat untuk data string yang berulang (kategori tagihan).
* **Peningkatan Enkapsulasi:** Menutup akses *state* dan *rules* internal agar tidak bisa dimodifikasi secara sembarangan dari luar kelas.
* **Penerapan Single Responsibility Principle (SRP):** Memisahkan tanggung jawab logika bisnis (Manager) dengan UI/Console (menghapus *print* dan *try-catch* di level logika).
* **Refactoring Extract Method:** Memecah fungsi yang terlalu kompleks menjadi fungsi yang lebih kecil dan fokus pada satu tugas.

---

## 🚀 Tambahan (Added)
* **Komponen Baru `KategoriTagihan` (Class Statis):** Ditambahkan untuk menyimpan konstanta string kategori tagihan (`Utilitas`, `LayananDigital`, dll) guna mencegah *typo* dan mempermudah pemeliharaan (*DRY Principle*).
* **Fungsi Baru `HitungTanggalJatuhTempo` di `PengingatTagihan`:** Di-ekstrak dari fungsi `SetupDetail` untuk menangani khusus logika kalkulasi tanggal jatuh tempo berdasarkan tabel konfigurasi.

## 🛠️ Perubahan (Changed)
* **Komponen `PengingatTagihan`:**
    * Akses variabel `TabelKonfigurasi` diubah dari `public static` menjadi `private static` agar pengaturannya eksklusif hanya untuk kelas ini.
    * Nilai *key* pada `TabelKonfigurasi` kini merujuk pada konstanta di kelas `KategoriTagihan` (bukan *hardcoded string*).
    * Fungsi `SetupDetail` sekarang menjadi lebih ringkas karena logika tanggal dipindahkan ke `HitungTanggalJatuhTempo`.
* **Komponen `TagihanManager`:**
    * Penamaan *private fields* diperbarui mengikuti standar C# (`daftarTagihan` menjadi `_daftarTagihan`).
    * Fungsi `CreateTagihan` dan `UpdateTagihan` tidak lagi menggunakan `try-catch` dan `Console.WriteLine()`. *Exception handling* diserahkan kepada layer yang memanggil (UI Layer).
    * Fungsi `GetSemuaTagihan` kini mengembalikan tipe `IReadOnlyList<PengingatTagihan>` menggunakan `.AsReadOnly()` sehingga koleksi tidak dapat dimanipulasi (ditambah/dihapus) secara langsung dari luar manajer.
    * Fungsi `UpdateTagihan` ditambahkan *Defensive Programming* berupa pengecekan nilai `null` dan langsung melempar `ArgumentException` jika ID tidak ditemukan (Prinsip *Fail-Fast*, konsisten dengan `DeleteTagihan`).

## 🗑️ Dihapus (Removed)
* **Log Console dan Handling Internal:** Dihapus dari `TagihanManager` karena menyalahi prinsip *Single Responsibility*. Logika inti tidak seharusnya mengatur urusan *input/output* antarmuka (*Console*).