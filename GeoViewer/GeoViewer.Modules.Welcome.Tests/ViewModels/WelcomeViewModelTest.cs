using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GeoViewer.Modules.Welcome.Interactivity;
using GeoViewer.Modules.Welcome.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GeoViewer.Modules.Welcome.ViewModels
{
    [TestClass]
    public class WelcomeViewModelTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullFileService()
        {
            var mockRecentFileService = new Mock<IRecentFileService>();

            var welcomeViewModel = new WelcomeViewModel(null, mockRecentFileService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullRecentFileService()
        {
            var mockFileService = new Mock<IFileService>();

            var welcomeViewModel = new WelcomeViewModel(mockFileService.Object, null);
        }

        [TestMethod]
        public void TestOpenCommand()
        {
            var testFileName = "TestFileName";
            var mockFileService = new Mock<IFileService>();
            mockFileService.SetupGet(m => m.Filter).Returns("TestFilter");
            var mockRecentFileService = new Mock<IRecentFileService>();
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var welcomeViewModel = new WelcomeViewModel(mockFileService.Object, mockRecentFileService.Object);
            welcomeViewModel.OpenFileInteractionRequest.Raised +=
                (s, e) =>
                {
                    var openFile = (IOpenFile)e.Context;
                    openFile.Confirmed = true;
                    openFile.FileName = testFileName;
                    e.Callback();
                };
            welcomeViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;

            Assert.IsTrue(welcomeViewModel.OpenCommand.CanExecute(null));
            welcomeViewModel.OpenCommand.Execute(null);

            mockFileService.Verify(m => m.Open(testFileName));
            mockPropertyChangedEventHandler.Verify(m => m(welcomeViewModel, ItIsProperty(() => welcomeViewModel.RecentFiles)));
        }

        [TestMethod]
        public void TestOpenCommandNotConfirmed()
        {
            var mockFileService = new Mock<IFileService>();
            mockFileService.SetupGet(m => m.Filter).Returns("TestFilter");
            var mockRecentFileService = new Mock<IRecentFileService>();
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var welcomeViewModel = new WelcomeViewModel(mockFileService.Object, mockRecentFileService.Object);
            welcomeViewModel.OpenFileInteractionRequest.Raised +=
                (s, e) =>
                {
                    var openFile = (IOpenFile)e.Context;
                    openFile.Confirmed = false;
                    e.Callback();
                };
            welcomeViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;

            Assert.IsTrue(welcomeViewModel.OpenCommand.CanExecute(null));
            welcomeViewModel.OpenCommand.Execute(null);

            mockFileService.Verify(m => m.Open(It.IsAny<string>()), Times.Never);
            mockPropertyChangedEventHandler.Verify(m => m(welcomeViewModel, ItIsProperty(() => welcomeViewModel.RecentFiles)), Times.Never);
        }

        [TestMethod]
        public void TestOpenRecentCommand()
        {
            var testRecentFile = "TestRecentFile";
            var mockFileService = new Mock<IFileService>();
            var mockRecentFileService = new Mock<IRecentFileService>();
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var welcomeViewModel = new WelcomeViewModel(mockFileService.Object, mockRecentFileService.Object);
            welcomeViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;

            Assert.IsTrue(welcomeViewModel.OpenRecentCommand.CanExecute(testRecentFile));
            welcomeViewModel.OpenRecentCommand.Execute(testRecentFile);

            mockFileService.Verify(m => m.Open(testRecentFile));
            mockPropertyChangedEventHandler.Verify(m => m(welcomeViewModel, ItIsProperty(() => welcomeViewModel.RecentFiles)));
        }

        [TestMethod]
        public void TestRecentFiles()
        {
            var testRecentFiles = new List<string>() { "TestRecentFile1", "TestRecentFile2", "TestRecentFile3" }.AsReadOnly();
            var mockFileService = new Mock<IFileService>();
            var mockRecentFileService = new Mock<IRecentFileService>();
            mockRecentFileService.SetupGet(m => m.RecentFiles).Returns(testRecentFiles);

            var welcomeViewModel = new WelcomeViewModel(mockFileService.Object, mockRecentFileService.Object);

            Assert.AreEqual(testRecentFiles.Count(), welcomeViewModel.RecentFiles.Count());
            Assert.IsTrue(Enumerable.SequenceEqual(testRecentFiles, welcomeViewModel.RecentFiles.Select(rfvm => rfvm.FileName)));
        }

        private static PropertyChangedEventArgs ItIsProperty<T>(Expression<Func<T>> propertyExpression)
        {
            return It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertySupport.ExtractPropertyName(propertyExpression));
        }
    }
}
