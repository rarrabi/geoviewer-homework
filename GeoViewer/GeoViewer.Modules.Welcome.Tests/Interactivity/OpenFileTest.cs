using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoViewer.Modules.Welcome.Interactivity
{
    [TestClass]
    public class OpenFileTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullFilter()
        {
            var openFile = new OpenFile(null);
        }

        [TestMethod]
        public void TestDefaultFilter()
        {
            var openFile = new OpenFile();

            Assert.AreEqual(string.Empty, openFile.Filter);
        }

        [TestMethod]
        public void TestFilter()
        {
            var testFilter = "TestFiler";

            var openFile = new OpenFile(testFilter);

            Assert.AreEqual(testFilter, openFile.Filter);
        }
    }
}
