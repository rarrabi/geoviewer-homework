using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoViewer.Common.Utils
{
    [TestClass]
    public class EnumerableHelperTest
    {
        [TestMethod]
        public void TestYield()
        {
            var testValue = new object();

            var enumerable = EnumerableHelper.Yield(testValue);

            Assert.AreEqual(1, enumerable.Count());
            Assert.AreEqual(testValue, enumerable.Single());
        }
    }
}
