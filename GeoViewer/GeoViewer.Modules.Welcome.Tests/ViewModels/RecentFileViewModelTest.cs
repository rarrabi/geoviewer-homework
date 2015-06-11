using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoViewer.Modules.Welcome.ViewModels
{
    [TestClass]
    public class RecentFileViewModelTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullFileName()
        {
            var recentFileViewModel = new RecentFileViewModel(null);
        }

        [TestMethod]
        public void TestFileName()
        {
            var testName = "TestName";
            var testFileName = @"C:\TestDirectory\" + testName + ".testextension";

            var recentFileViewModel = new RecentFileViewModel(testFileName);

            Assert.AreEqual(testFileName, recentFileViewModel.FileName);
        }

        [TestMethod]
        public void TestName()
        {
            var testName = "TestName";
            var testFileName = @"C:\TestDirectory\" + testName + ".testextension";

            var recentFileViewModel = new RecentFileViewModel(testFileName);

            Assert.AreEqual(testName, recentFileViewModel.Name);
        }
    }
}
