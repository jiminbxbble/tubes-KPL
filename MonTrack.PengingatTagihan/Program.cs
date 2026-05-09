using System.Diagnostics;
using MonTrack_PengingatTagihan;

var watch = Stopwatch.StartNew();

// Simulasi proses 100 tagihan
for (int i = 0; i < 100; i++)
{
    var penagih = new PengingatTagihan("Tagihan ke-" + i, 10000);
    penagih.Bayar("Air", 12500);
}

watch.Stop();
Console.WriteLine($"\n--- Hasil Performa ---");
Console.WriteLine($"Waktu Eksekusi: {watch.Elapsed.TotalMilliseconds} ms");