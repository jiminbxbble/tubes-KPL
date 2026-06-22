# 💰 MONTRACK: APLIKASI MANAJEMEN KEUANGAN

[![Framework/Tech](https://img.shields.io/badge/Tech_Stack-.NET-blue.svg)]()
[![License: MIT](https://img.shields.io/badge/License-TelkomUniversity-red.svg)](https://opensource.org/licenses/Telkom-University)

Aplikasi manajemen keuangan personal yang dirancang untuk membantu pengguna mencatat pemasukan maupun pengeluaran, melacak tagihan, dan menganalisis kesehatan finansial dengan analytic charts dan export data semua transaksi melalui csv/pdf

---

## Tampilan Antarmuka (UI)

Berikut adalah visualisasi antarmuka dan fitur utama dari aplikasi ini:

### 1. Halaman Login & Sign Up
Halaman gerbang utama yang aman untuk autentikasi pengguna baru maupun pengguna lama. Dilengkapi dengan validasi input yang intuitif.
> [!TIP]
> Jalur masuk utama menggunakan email/password ["admin1@gmail.com", "admin123"].

| Login Page | Sign Up Page |
|---|---|
| <img width="413" height="485" alt="image" src="https://github.com/user-attachments/assets/1e186b0a-de89-4e38-b774-22eb7fa3b737" /> | <img width="419" height="549" alt="image" src="https://github.com/user-attachments/assets/9b28c931-95a1-4e13-987c-fc5eaf0765ed" />

---

### 2. Halaman Pencatatan Keuangan
Fitur inti untuk mencatat arus kas (pemasukan dan pengeluaran). Pengguna dapat menambahkan nominal, memilih kategori, tanggal, serta deskripsi transaksi secara cepat.

| UI Pencatatan Keuangan |
| :---: |
| <img width="1055" height="719" alt="image" src="https://github.com/user-attachments/assets/9e4f2d88-fd60-4ee0-b8f1-b0883ba81d17" />

---

### 3. Halaman Pengingat Tagihan (Bill Reminder)
Halaman khusus untuk mengelola tagihan rutin bulanan (listrik, internet, cicilan, dll). Fitur ini membantu pengguna agar tidak terlewat membayar tagihan sebelum jatuh tempo.

| UI Pengingat Tagihan |
| :---: |
| <img width="1056" height="726" alt="image" src="https://github.com/user-attachments/assets/b50f193b-f9e1-4b61-9317-32ae27779d9b" />

---

### 4. Halaman Daftar & Riwayat Transaksi
Menampilkan seluruh daftar transaksi yang pernah dicatat oleh pengguna secara runut.
| Income | Expense|
|---|---|
| <img width="1045" height="713" alt="image" src="https://github.com/user-attachments/assets/f7fb5c48-c24e-4fd7-998e-7f96ab08fae7" /> | <img width="1050" height="719" alt="image" src="https://github.com/user-attachments/assets/e5e1fc83-c76c-452a-8397-88f71c0f4d32" />

---

### 5. Halaman Analytics & Chart
Visualisasi data keuangan dalam bentuk grafik yang interaktif dengan diagram batang. Memudahkan pengguna melakukan evaluasi finansial mingguan atau bulanan.

| UI Analytics & Chart |
| :---: |
| <img width="1053" height="718" alt="image" src="https://github.com/user-attachments/assets/075166fd-6417-4e9d-ac75-1ef0954a97e8" />

---

### 6. Halaman Export Data Transaksi
Fitur untuk mengunduh seluruh riwayat transaksi keuangan ke dalam format dokumen eksternal untuk kebutuhan pembukuan lebih lanjut.
* Support format: **PDF** dan **Excel (.pdf / .csv)**
* Dilengkapi dengan filter rentang tanggal tertentu.

| UI Export Data |
| :---: |
| <img width="1053" height="724" alt="image" src="https://github.com/user-attachments/assets/31c556db-f1d8-481c-8660-70454c02deb9" />

---

## 🚀 Fitur Utama
* **Autentikasi Aman:** Menggunakan enkripsi [sebutkan jika ada, misal: JWT/Firebase].
* **Manajemen Arus Kas:** Pencatatan pemasukan dan pengeluaran yang fleksibel.
* **Sistem Alergi Tagihan:** Notifikasi atau penanda tagihan yang mendekati tenggat waktu.
* **Riwayat & Pelacakan Transaksi:** Log riwayat yang lengkap dengan filter pencarian yang dinamis.
* **Visualisasi Data Dinamis:** Chart interaktif yang responsif.
* **Ekspor Multi-Format:** Konversi data transaksi ke PDF/Excel hanya dengan satu klik.

## 🛠️ Tech Stack
* **Modul Autentikasi (Login & Register) - Rosa Ardila :** [Automata & API ]
* **Modul Pengingat Tagihan - Raissha Najwa Maharani  :** [Automata & Table-Driven Construction ]
* **Modul Pencatatan Pemasukan & Pengeluaran - Rafiqah Nailaturrahmah :** [Parametrization/Generics & Table-Driven Construction ]
* **Modul Daftar & Riwayat Transaksi - Daniartha Wikus Nugroho :** [Parameterization/Generics, Runtime Configuration ]
* **Modul Ekspor Data Transaksi - Thoriq Abdurrahman Taqy :** [Runtime configuration dan API ]

---

## 💻 Cara Menjalankan Project

Pastikan kamu sudah menginstal [.NET SDK 9.0 atau 10.0](https://dotnet.microsoft.com/download) di komputermu sebelum menjalankan langkah-langkah di bawah ini.

1. **Clone Repositori**
   ```bash
   git clone https://github.com/jiminbxbble/tubes-KPL/
2. **Masuk ke Direktori Project**
     ```bash
   cd tubes-kpl
    cd Montrack.WinForms
3. **Restore Dependencies**
     ```bash
   dotnet restore
4. **Build Project**
     ```bash
   dotnet build
5. **Jalankan Aplikasi**
     ```bash
   dotnet run
