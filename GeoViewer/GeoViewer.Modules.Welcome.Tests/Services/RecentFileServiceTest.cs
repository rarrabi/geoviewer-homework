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

        /// <summary>
        /// A test service for handling the list of recent files.
        /// </summary>
        private class TestRecentFileService : RecentFileService
        {
            /// <summary>
            /// Initializes a new instance of the TestRecentFileService class.
            /// </summary>
            /// <param name="size">The size of the list of recent files.</param>
            public TestRecentFileService(int size)
                : base(size)
            {
            }

            /// <summary>
            /// Gets the list of test recent files.
            /// </summary>
            public IReadOnlyList<string> TestRecentFiles { get; set; }

            /// <summary>
            /// Loads the list of recent files from a persistent storage (the test recent files).
            /// </summary>
            /// <returns>The list of recent files.</returns>
            protected override IReadOnlyList<string> LoadRecentFiles()
            {
                return this.TestRecentFiles;
            }

            /// <summary>
            /// Saves the list of recent files to a persistent storage (the test recent files).
            /// </summary>
            /// <param name="recentFiles">The list of recent files.</param>
            protected override void SaveRecentFiles(IReadOnlyList<string> recentFiles)
            {
                this.TestRecentFiles = recentFiles;
            }
        }
    }
}
