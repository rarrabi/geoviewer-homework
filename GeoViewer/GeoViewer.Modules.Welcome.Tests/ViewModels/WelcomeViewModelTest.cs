using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GeoViewer.Common;
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
        public void TestNullRegionManager()
        {
            var mockFileService = new Mock<IFileService>();
            var mockRecentFileService = new Mock<IRecentFileService>();

            var welcomeViewModel = new WelcomeViewModel(null, mockFileService.Object, mockRecentFileService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullFileService()
        {
            var mockRegionManager = new Mock<IRegionManager>();
            var mockRecentFileService = new Mock<IRecentFileService>();

            var welcomeViewModel = new WelcomeViewModel(mockRegionManager.Object, null, mockRecentFileService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullRecentFileService()
        {
            var mockRegionManager = new Mock<IRegionManager>();
            var mockFileService = new Mock<IFileService>();

            var welcomeViewModel = new WelcomeViewModel(mockRegionManager.Object, mockFileService.Object, null);
        }

        [TestMethod]
        public void TestOpenCommand()
        {
            var testFileName = "TestFileName";
            var testFeatureSet = new DotSpatial.Data.FeatureSet();

            var mockMainRegion = new Mock<IRegion>();
            var mockLeftRegion = new Mock<IRegion>();
            var mockRightRegion = new Mock<IRegion>();
            var mockRegionCollection = new Mock<IRegionCollection>();
            mockRegionCollection.Setup(m => m.ContainsRegionWithName(It.IsAny<string>())).Returns(true);
            mockRegionCollection.Setup(m => m[Constants.Region.Main]).Returns(mockMainRegion.Object);
            mockRegionCollection.Setup(m => m[Constants.Region.Left]).Returns(mockLeftRegion.Object);
            mockRegionCollection.Setup(m => m[Constants.Region.Right]).Returns(mockRightRegion.Object);
            var mockRegionManager = new Mock<IRegionManager>();
            mockRegionManager.Setup(m => m.Regions).Returns(mockRegionCollection.Object);
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Filter).Returns("TestFilter");
            mockFileService.Setup(m => m.Open(testFileName)).Returns(testFeatureSet);
            var mockRecentFileService = new Mock<IRecentFileService>();
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var welcomeViewModel = new WelcomeViewModel(mockRegionManager.Object, mockFileService.Object, mockRecentFileService.Object);
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
            mockRegionManager.Verify(m => m.Regions);
            mockMainRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Attributes), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Attributes.Source] == testFeatureSet)));
            // TODO WelcomeViewModelTest#TestOpenCommand
            //// mockMainRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Geometry), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Geometry.Source] == testFeatureSet)));
            mockLeftRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Structure), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Structure.Source] == testFeatureSet)));
            mockRightRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Properties), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Properties.Source] == testFeatureSet)));
        }

        [TestMethod]
        public void TestOpenCommandNotConfirmed()
        {
            var mockRegionManager = new Mock<IRegionManager>();
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Filter).Returns("TestFilter");
            var mockRecentFileService = new Mock<IRecentFileService>();
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var welcomeViewModel = new WelcomeViewModel(mockRegionManager.Object, mockFileService.Object, mockRecentFileService.Object);
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
            mockRegionManager.Verify(m => m.Regions, Times.Never);
        }

        [TestMethod]
        public void TestOpenRecentCommand()
        {
            var testRecentFile = "TestRecentFile";
            var testFeatureSet = new DotSpatial.Data.FeatureSet();

            var mockMainRegion = new Mock<IRegion>();
            var mockLeftRegion = new Mock<IRegion>();
            var mockRightRegion = new Mock<IRegion>();
            var mockRegionCollection = new Mock<IRegionCollection>();
            mockRegionCollection.Setup(m => m.ContainsRegionWithName(It.IsAny<string>())).Returns(true);
            mockRegionCollection.Setup(m => m[Constants.Region.Main]).Returns(mockMainRegion.Object);
            mockRegionCollection.Setup(m => m[Constants.Region.Left]).Returns(mockLeftRegion.Object);
            mockRegionCollection.Setup(m => m[Constants.Region.Right]).Returns(mockRightRegion.Object);
            var mockRegionManager = new Mock<IRegionManager>();
            mockRegionManager.Setup(m => m.Regions).Returns(mockRegionCollection.Object);
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Open(testRecentFile)).Returns(testFeatureSet);
            var mockRecentFileService = new Mock<IRecentFileService>();
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var welcomeViewModel = new WelcomeViewModel(mockRegionManager.Object, mockFileService.Object, mockRecentFileService.Object);
            welcomeViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;

            Assert.IsTrue(welcomeViewModel.OpenRecentCommand.CanExecute(testRecentFile));
            welcomeViewModel.OpenRecentCommand.Execute(testRecentFile);

            mockFileService.Verify(m => m.Open(testRecentFile));
            mockPropertyChangedEventHandler.Verify(m => m(welcomeViewModel, ItIsProperty(() => welcomeViewModel.RecentFiles)));
            mockRegionManager.Verify(m => m.Regions);
            mockMainRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Attributes), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Attributes.Source] == testFeatureSet)));
            // TODO WelcomeViewModelTest#TestOpenRecentCommand
            //// mockMainRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Geometry), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Geometry.Source] == testFeatureSet)));
            mockLeftRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Structure), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Structure.Source] == testFeatureSet)));
            mockRightRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Properties), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Properties.Source] == testFeatureSet)));
        }

        [TestMethod]
        public void TestRecentFiles()
        {
            var testRecentFiles = new List<string>() { "TestRecentFile1", "TestRecentFile2", "TestRecentFile3" }.AsReadOnly();

            var mockRegionManager = new Mock<IRegionManager>();
            var mockFileService = new Mock<IFileService>();
            var mockRecentFileService = new Mock<IRecentFileService>();
            mockRecentFileService.Setup(m => m.RecentFiles).Returns(testRecentFiles);

            var welcomeViewModel = new WelcomeViewModel(mockRegionManager.Object, mockFileService.Object, mockRecentFileService.Object);

            Assert.IsTrue(Enumerable.SequenceEqual(testRecentFiles, welcomeViewModel.RecentFiles.Select(rfvm => rfvm.FileName)));
        }

        private static PropertyChangedEventArgs ItIsProperty<T>(Expression<Func<T>> propertyExpression)
        {
            return It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertySupport.ExtractPropertyName(propertyExpression));
        }

        private static Uri ItIsUri(string uri)
        {
            return It.Is<Uri>(u => u.ToString() == uri);
        }
    }
}
