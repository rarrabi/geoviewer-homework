using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;

namespace GeoViewer.Modules.Welcome.Services
{
    [TestClass]
    public class FileServiceTest
    {
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
        public void TestNullRecentFileService()
        {
            var fileService = new FileService(null);
        }

        [TestMethod]
        public void TestFilter()
        {
            var mockRecentFileService = new Mock<IRecentFileService>();

            var fileService = new FileService(mockRecentFileService.Object);

            Assert.IsNotNull(fileService.Filter);

            try
            {
                var openFileDialog = new OpenFileDialog() { Filter = fileService.Filter };
            }
            catch (ArgumentException e)
            {
                Assert.Fail("Invalid filter expression: '{0}'\n{1}", fileService.Filter, e);
            }
        }

        [TestMethod]
        public void TestOpen()
        {
            var testFileName = this.testFileName;
            var mockRecentFileService = new Mock<IRecentFileService>();

            File.WriteAllText(testFileName, "Test");
            var fileService = new FileService(mockRecentFileService.Object);
            fileService.Open(testFileName);

            mockRecentFileService.Verify(m => m.Add(testFileName));
        }

        [TestMethod]
        public void TestOpenInvalidEmpty()
        {
            var testFileName = this.testFileName;
            var mockRecentFileService = new Mock<IRecentFileService>();

            var fileService = new FileService(mockRecentFileService.Object);
            fileService.Open(testFileName);

            mockRecentFileService.Verify(m => m.Remove(testFileName));
        }

        [TestMethod]
        public void TestOpenInvalidNotExists()
        {
            var testFileName = this.testFileName;
            var mockRecentFileService = new Mock<IRecentFileService>();

            File.Delete(testFileName);
            var fileService = new FileService(mockRecentFileService.Object);
            fileService.Open(testFileName);

            mockRecentFileService.Verify(m => m.Remove(testFileName));
        }
    }
}
