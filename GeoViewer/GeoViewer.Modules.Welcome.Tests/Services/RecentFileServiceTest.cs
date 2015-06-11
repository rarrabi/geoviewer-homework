using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoViewer.Modules.Welcome.Services
{
    [TestClass]
    public class RecentFileServiceTest
    {
        [TestMethod]
        public void TestRecentFiles()
        {
            var testRecentFiles = new List<string>() { "TestRecentFile1", "TestRecentFile2", "TestRecentFile3" }.AsReadOnly();

            var recentFileService = new TestRecentFileService(byte.MaxValue) { TestRecentFiles = testRecentFiles };

            Assert.AreEqual(testRecentFiles.Count(), recentFileService.RecentFiles.Count());
            Assert.IsTrue(Enumerable.SequenceEqual(testRecentFiles, recentFileService.RecentFiles));
        }

        [TestMethod]
        public void TestAdd()
        {
            var testRecentFile = "TestRecentFile";
            var testRecentFiles = new List<string>() { "TestRecentFile1", "TestRecentFile2", "TestRecentFile3" }.AsReadOnly();

            var recentFileService = new TestRecentFileService(byte.MaxValue) { TestRecentFiles = testRecentFiles };
            recentFileService.Add(testRecentFile);

            Assert.AreEqual(testRecentFiles.Count() + 1, recentFileService.RecentFiles.Count());
            Assert.AreEqual(testRecentFile, recentFileService.RecentFiles.First());
            Assert.IsTrue(Enumerable.SequenceEqual(testRecentFiles, recentFileService.RecentFiles.Skip(1)));
        }

        [TestMethod]
        public void TestRemove()
        {
            var testRecentFile = "TestRecentFile";
            var testRecentFiles = new List<string>() { testRecentFile, "TestRecentFile1", "TestRecentFile2", "TestRecentFile3" }.AsReadOnly();

            var recentFileService = new TestRecentFileService(byte.MaxValue) { TestRecentFiles = testRecentFiles };
            recentFileService.Remove(testRecentFile);

            Assert.AreEqual(testRecentFiles.Count() - 1, recentFileService.RecentFiles.Count());
            Assert.IsFalse(recentFileService.RecentFiles.Contains(testRecentFile));
            Assert.IsTrue(Enumerable.SequenceEqual(testRecentFiles.Skip(1), recentFileService.RecentFiles));
        }

        public void TestSize()
        {
            var testRecentFile = "TestRecentFile";
            var testRecentFiles = new List<string>() { "TestRecentFile1", "TestRecentFile2", "TestRecentFile3" }.AsReadOnly();
            var testSize = testRecentFiles.Count();

            var recentFileService = new TestRecentFileService(testSize) { TestRecentFiles = testRecentFiles };
            recentFileService.Add(testRecentFile);

            Assert.AreEqual(testSize, recentFileService.RecentFiles.Count());
            Assert.AreEqual(testRecentFile, recentFileService.RecentFiles.First());
            Assert.IsTrue(Enumerable.SequenceEqual(testRecentFiles.Take(testRecentFiles.Count() - 1), recentFileService.RecentFiles.Skip(1)));
        }

        private class TestRecentFileService : RecentFileService
        {
            public TestRecentFileService(int size)
                : base(size)
            {
            }

            public IReadOnlyList<string> TestRecentFiles { get; set; }

            protected override IReadOnlyList<string> LoadRecentFiles()
            {
                return this.TestRecentFiles;
            }

            protected override void SaveRecentFiles(IReadOnlyList<string> recentFiles)
            {
                this.TestRecentFiles = recentFiles;
            }
        }
    }
}
