using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;
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
                File.Delete(this.testFileName + ".shp");
                File.Delete(this.testFileName + ".shx");
                File.Delete(this.testFileName + ".dbf");
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
            var testFileName = this.testFileName + ".shp";
            var testFeatureSet = new FeatureSet(DotSpatial.Topology.FeatureType.Point);

            var mockRecentFileService = new Mock<IRecentFileService>();

            testFeatureSet.SaveAs(testFileName, true);

            var fileService = new FileService(mockRecentFileService.Object);
            var featureSet = fileService.Open(testFileName);

            Assert.IsNotNull(featureSet);

            mockRecentFileService.Verify(m => m.Add(testFileName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestOpenFailed()
        {
            var testFileName = this.testFileName;

            var mockRecentFileService = new Mock<IRecentFileService>();

            File.WriteAllText(testFileName, "Test");

            var fileService = new FileService(mockRecentFileService.Object);
            var featureSet = fileService.Open(testFileName); // This should throw an ArgumentException.

            Assert.IsNull(featureSet);

            mockRecentFileService.Verify(m => m.Remove(testFileName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestOpenInvalidEmpty()
        {
            var testFileName = this.testFileName;

            var mockRecentFileService = new Mock<IRecentFileService>();

            var fileService = new FileService(mockRecentFileService.Object);
            var featureSet = fileService.Open(testFileName); // This should throw an ArgumentException.

            Assert.IsNull(featureSet);

            mockRecentFileService.Verify(m => m.Remove(testFileName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestOpenInvalidNotExists()
        {
            var testFileName = this.testFileName;

            var mockRecentFileService = new Mock<IRecentFileService>();

            File.Delete(testFileName);

            var fileService = new FileService(mockRecentFileService.Object);
            var featureSet = fileService.Open(testFileName); // This should throw an ArgumentException.

            Assert.IsNull(featureSet);

            mockRecentFileService.Verify(m => m.Remove(testFileName));
        }
    }
}
