using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonTrack_PengingatTagihan;
using System;
using System.Linq;

namespace Unit_Testing
{
    [TestClass]
    public class TagihanTests
    {
        [TestMethod]
        public void TestStatusAwalHarusTersedia()
        {
            DateTime tglDibuat = DateTime.Now;
            DateTime tglJatuhTempo = DateTime.Now.AddDays(15);

            var tagihan = new PengingatTagihan(1, "PLN Mei", "Utilitas", 50000, tglDibuat, tglJatuhTempo);

            Assert.AreEqual(TagihanState.Tersedia, tagihan.StatusSaatIni);
        }

        [TestMethod]
        public void TestStatusHarusTerlambatJikaDeadlineLewat()
        {
            DateTime tanggalDibuat = DateTime.Now.AddDays(-10);
            DateTime tanggalJatuhTempo = DateTime.Now.AddDays(-5);

            var tagihan = new PengingatTagihan(2, "Tagihan Kost", "Utilitas", 1000000, tanggalDibuat, tanggalJatuhTempo);

            tagihan.CekWaktuJatuhTempo();

            Assert.AreEqual(TagihanState.Terlambat, tagihan.StatusSaatIni);
        }

        [TestMethod]
        public void TestTandaiLunasBerhasilMengubahState()
        {
            var tagihan = new PengingatTagihan(3, "Indihome", "Layanan digital", 300000, DateTime.Now, DateTime.Now.AddDays(10));

            tagihan.TandaiLunas();

            Assert.AreEqual(TagihanState.Lunas, tagihan.StatusSaatIni);
        }
    }

    // --- Unit test fitur CRUD ---
    [TestClass]
    public class TagihanManagerTests
    {
        [TestMethod]
        public void TestTambahTagihanMenambahJumlahData()
        {
            var manager = new TagihanManager();

            manager.CreateTagihan("Spotify", "Layanan digital", 50000, DateTime.Now, DateTime.Now.AddDays(30));
            var daftarTagihan = manager.GetSemuaTagihan();

            Assert.AreEqual(1, daftarTagihan.Count);
            Assert.AreEqual("Spotify", daftarTagihan[0].Nama);
        }

        [TestMethod]
        public void TestUpdateTagihanMengubahDetailData()
        {
            var manager = new TagihanManager();
            manager.CreateTagihan("Token Listrik", "Utilitas", 100000, DateTime.Now, DateTime.Now.AddDays(20));
            var tagihan = manager.GetSemuaTagihan().First();

            DateTime tglBaru = DateTime.Now;
            DateTime tenggatBaru = DateTime.Now.AddDays(25);
            manager.UpdateTagihan(tagihan.Id, "Token Listrik Rumah", "Utilitas", 150000, tglBaru, tenggatBaru);
            
            Assert.AreEqual("Token Listrik Rumah", tagihan.Nama);
            Assert.AreEqual(150000, tagihan.Nominal);
            Assert.AreEqual(tenggatBaru, tagihan.TanggalJatuhTempo);
        }

        [TestMethod]
        public void TestDeleteTagihanMengurangiJumlahData()
        {
            var manager = new TagihanManager();
            manager.CreateTagihan("PDAM", "Utilitas", 80000, DateTime.Now, DateTime.Now.AddDays(15));
            var tagihan = manager.GetSemuaTagihan().First();

            manager.DeleteTagihan(tagihan.Id);

            Assert.AreEqual(0, manager.GetSemuaTagihan().Count);
        }
    }
}