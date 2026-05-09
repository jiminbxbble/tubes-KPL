using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonTrack_PengingatTagihan;

namespace MonTrack_Tests
{
    [TestClass]
    public class TagihanTests
    {
        [TestMethod]
        public void TestStatusAwalHarusTersedia()
        {
            var tagihan = new PengingatTagihan("PLN Mei", 50000);
            Assert.AreEqual(PengingatTagihan.TagihanState.Tersedia, tagihan.StatusSaatIni);
        }

        [TestMethod]
        public void TestBayarBerhasilMengubahStateKeLunas()
        {
            var tagihan = new PengingatTagihan("Indihome", 300000);
            tagihan.Bayar("Internet", 310000); // 300rb + 10rb admin
            Assert.AreEqual(PengingatTagihan.TagihanState.Lunas, tagihan.StatusSaatIni);
        }
    }
}