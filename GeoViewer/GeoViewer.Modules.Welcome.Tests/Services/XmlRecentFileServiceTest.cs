using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoViewer.Modules.Welcome.Services
{
    [TestClass]
    public class XmlRecentFileServiceTest
    {
        /// <summary>
        /// The string containing the full path of the test file.
        /// </summary>
        private string testFileName;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testFileName = Path.GetTempFileName();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (this.testFileName != null && File.Exists(this.testFileName))
            {
                File.Delete(this.testFileName);
                this.testFileName = null;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullFileName()
        {
            var recentFileViewModel = new XmlRecentFileService(null, byte.MaxValue);
        }

        [TestMethod]
        public void TestLoad()
        {
            var testFileName = this.testFileName;
            var testRecentFiles = new List<string>() { "TestRecentFile1", "TestRecentFile2", "TestRecentFile3" }.AsReadOnly();
            var testDocument = new XDocument(new XElement("RecentFiles", testRecentFiles.Select(rf => new XElement("RecentFile", new XAttribute("fileName", rf))).ToArray()));

            testDocument.Save(testFileName);

            var xmlRecentFileService = new XmlRecentFileService(testFileName, byte.MaxValue);

            Assert.IsTrue(Enumerable.SequenceEqual(testRecentFiles, xmlRecentFileService.RecentFiles));
        }

        [TestMethod]
        public void TestLoadInvalidEmpty()
        {
            var testFileName = this.testFileName;
            var testRecentFiles = new List<string>().AsReadOnly();

            var xmlRecentFileService = new XmlRecentFileService(testFileName, byte.MaxValue);

            Assert.IsTrue(Enumerable.SequenceEqual(testRecentFiles, xmlRecentFileService.RecentFiles));
        }

        [TestMethod]
        public void TestLoadInvalidNotExists()
        {
            var testFileName = this.testFileName;
            var testRecentFiles = new List<string>().AsReadOnly();

            File.Delete(testFileName);

            var xmlRecentFileService = new XmlRecentFileService(testFileName, byte.MaxValue);

            Assert.IsTrue(Enumerable.SequenceEqual(testRecentFiles, xmlRecentFileService.RecentFiles));
        }

        [TestMethod]
        public void TestSave()
        {
            var testFileName = this.testFileName;
            var testRecentFiles = new List<string>() { "TestRecentFile1", "TestRecentFile2", "TestRecentFile3" }.AsReadOnly();
            var testDocument = new XDocument(new XElement("RecentFiles", testRecentFiles.Select(rf => new XElement("RecentFile", new XAttribute("fileName", rf))).ToArray()));

            var xmlRecentFileService = new XmlRecentFileService(testFileName, byte.MaxValue);
            foreach (var testRecentFile in testRecentFiles.Reverse())
            {
                xmlRecentFileService.Add(testRecentFile);
            }

            var document = XDocument.Load(testFileName);
            Assert.IsTrue(XNode.DeepEquals(testDocument, document));
        }
    }
}
