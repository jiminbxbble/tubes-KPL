using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonTrack_PengingatTagihan;
using System;

namespace Unit_Testing
{
    [TestClass]
    public class TagihanTests
    {
        [TestMethod]
        public void TestStatusAwalHarusTersedia()
        {
            var tagihan = new PengingatTagihan("PLN Mei", KategoriTagihan.Utilitas, 50000, DateTime.Now);
            Assert.AreEqual(PengingatTagihan.TagihanState.Tersedia, tagihan.StatusSaatIni);
        }

        [TestMethod]
        public void TestStatusHarusTerlambatJikaDeadlineLewat()
        {
            var tagihan = new PengingatTagihan("Tagihan Kost", KategoriTagihan.Utilitas, 1000000, DateTime.Now.AddDays(-40));
            Assert.AreEqual(PengingatTagihan.TagihanState.Terlambat, tagihan.StatusSaatIni);
        }

        [TestMethod]
        public void TestTandaiLunasBerhasilMengubahState()
        {
            var tagihan = new PengingatTagihan("Indihome", KategoriTagihan.LayananDigital, 300000, DateTime.Now);
            tagihan.TandaiLunas();

            Assert.AreEqual(PengingatTagihan.TagihanState.Lunas, tagihan.StatusSaatIni);
        }

        [TestMethod]
        public void TestCustomDeadlineDanRepetisi()
        {
            var customDeadline = DateTime.Now.AddDays(45);
            var tagihan = new PengingatTagihan("PLN Custom", KategoriTagihan.Utilitas, 50000, DateTime.Now, customDeadline, "Bulanan");
            Assert.AreEqual(customDeadline, tagihan.Deadline);
            Assert.AreEqual("Bulanan", tagihan.Repetisi);
        }
    }
}