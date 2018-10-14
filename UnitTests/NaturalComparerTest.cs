using comicReader.NET;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    /// <summary>
    ///This is a test class for NaturalComparerTest and is intended
    ///to contain all NaturalComparerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NaturalComparerTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod()]
        public void CompareTest01()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare("ciao01test", "ciao02aol") < 0);
        }

        [TestMethod()]
        public void CompareTest02()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare("shit2", "shit12") < 0);
        }

        [TestMethod()]
        public void CompareTest03()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare("shit2lol134", "shit2lol54") > 0);
        }

        [TestMethod()]
        public void CompareTest04()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare("Aku no Hana ch017 [C-S].rar", "Aku no Hana ch017.5 [C-S].rar") < 0);
        }

        [TestMethod()]
        public void CompareTest05()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare("Short-Program-v03c11-12_[Peebs].zip", "Short_Program_v01.zip") > 0);
        }

        [TestMethod()]
        public void CompareTest06()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare(@"b\altan_Pagina_018.jpg", @"b\altan_Pagina_018b.jpg") < 0);
        }

        [TestMethod()]
        public void CompareTest07()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare(@"Page 54-55.png", @"Page 56.png") < 0);
        }

        [TestMethod()]
        public void CompareTest08()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare(@"Page  1.png", @"Page 2.png") < 0);
        }

        [TestMethod()]
        public void CompareTest09()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare(@"Series [groupName] - Chapter 1.zip", @"Series (groupName) - Chapter 2.rar") < 0);
        }


        [TestMethod()]
        public void CompareTest10()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare(@"1409698850351.jpg", @"1409699120855.jpg") < 0);
        }


        [TestMethod()]
        public void CompareTest11()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare(@"Chapter_022.zip", @"Chapter_022_B.zip") < 0);
        }

        //[TestMethod()]
        //public void CompareTest12()
        //{
        //    //I added this test, but still haven't bothered with fixing it
        //    NaturalComparer target = new NaturalComparer();
        //    Assert.IsTrue(target.Compare(@"Ryuushika_Ryuushika_v01\Ryuushika_Ryuushika_v01_c02-5_035.jpeg", @"Ryuushika_Ryuushika_v01\Ryuushika_Ryuushika_v01_c02_028.jpeg") > 0);
        //}

        [TestMethod()]
        public void CompareTest13()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare(@"Shin Chimoguri Ringo to Kingyobachi Otoko c002 [Habanero Scans].zip", @"Shin Chimoguri Ringo to Kingyobachi Otoko c002b [Habanero Scans].zip") < 0);
        }



        [TestMethod()]
        public void CompareTest14()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare(@"Shin Chimoguri Ringo to Kingyobachi Otoko c002.zip", @"Shin Chimoguri Ringo to Kingyobachi Otoko c002b.zip") < 0);
        }



        [TestMethod()]
        public void CompareTest15()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare(@"Chimoguri_Ringo_to_Kingyobachi_Otoko_v01_c01_[Maigo]\Chimoguri_Ringo_c01_17.png", 
                                            @"Chimoguri_Ringo_to_Kingyobachi_Otoko_v01_c01_[Maigo]\Chimoguri_Ringo_c01_29.png") < 0);
        }

        [TestMethod()]
        public void CompareTest16()
        {
            NaturalComparer target = new NaturalComparer();
            Assert.IsTrue(target.Compare(@"[瀬戸口みづき] ローカル女子の遠吠え【電子限定版】　２巻_00080.jpg",
                                            @"[瀬戸口みづき] ローカル女子の遠吠え【電子限定版】　２巻_00001.jpg") < 0);
        }
    }
}
