using comicReader.NET;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class NumberComparerTest
    {
        [TestMethod]
        public void NumberCompareTest1()
        {
            NumberComparer cmp = new NumberComparer();
            Assert.IsTrue(cmp.Compare("test01", "test02") < 0);
        }
    }
}
