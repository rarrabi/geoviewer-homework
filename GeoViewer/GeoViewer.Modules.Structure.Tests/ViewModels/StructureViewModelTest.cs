using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;
using GeoViewer.Common;
using GeoViewer.Common.Events;
using GeoViewer.Common.Utils;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GeoViewer.Modules.Structure.ViewModels
{
    [TestClass]
    public class StructureViewModelTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullRegionManager()
        {
            var mockEventAggregator = new Mock<IEventAggregator>();

            var structureViewModel = new StructureViewModel(null, mockEventAggregator.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullEventAggregator()
        {
            var mockRegionManager = new Mock<IRegionManager>();

            var structureViewModel = new StructureViewModel(mockRegionManager.Object, null);
        }

        [TestMethod]
        public void TestRoot()
        {
            var testName0 = "TestName0";
            var testName1 = "TestName1";
            var testName2 = "TestName2";
            var testType0 = typeof(string);
            var testType1 = typeof(string);
            var testType2 = typeof(string);

            var testSource = new FeatureSet(DotSpatial.Topology.FeatureType.Point);
            testSource.Name = "TestName";
            testSource.DataTable.Columns.Add(new DataColumn(testName0, testType0));
            testSource.DataTable.Columns.Add(new DataColumn(testName1, testType1));
            testSource.DataTable.Columns.Add(new DataColumn(testName2, testType2));
            var testFeature0 = testSource.AddFeature(new DotSpatial.Topology.Point(0.0, 0.0));
            var testFeature1 = testSource.AddFeature(new DotSpatial.Topology.Point(1.0, 1.0));
            var testFeature2 = testSource.AddFeature(new DotSpatial.Topology.Point(2.0, 0.0));

            var mockRegionManager = new Mock<IRegionManager>();
            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockRegionManager.Object, mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            Assert.IsNotNull(structureViewModel.Root);
            Assert.AreEqual(structureViewModel.Root, structureViewModel.Roots.Single());
            Assert.IsTrue(IsStructureItemViewModel(testSource.Name, testSource.GetType().Name)(structureViewModel.Root));
            Assert.IsTrue(structureViewModel.Root.Children.Any(
                sivm => IsStructureItemViewModel("Attributes", testSource.DataTable.GetType().Name)(sivm) &&
                        sivm.Children.Any(IsStructureItemViewModel(testName0, testType0.Name)) &&
                        sivm.Children.Any(IsStructureItemViewModel(testName1, testType1.Name)) &&
                        sivm.Children.Any(IsStructureItemViewModel(testName2, testType2.Name))));
            Assert.IsTrue(structureViewModel.Root.Children.Any(
                sivm => IsStructureItemViewModel("Features", string.Format("[{0}]", testSource.FeatureType))(sivm) &&
                        sivm.Children.Any(IsStructureItemViewModel(testFeature0.Fid.ToString(), testFeature0.FeatureType.ToString())) &&
                        sivm.Children.Any(IsStructureItemViewModel(testFeature1.Fid.ToString(), testFeature1.FeatureType.ToString())) &&
                        sivm.Children.Any(IsStructureItemViewModel(testFeature2.Fid.ToString(), testFeature2.FeatureType.ToString()))));

            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Root)));
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Roots)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestRootInvalid()
        {
            var testSource = new object();

            var mockRegionManager = new Mock<IRegionManager>();
            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockRegionManager.Object, mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource)); // This should throw an ArgumentOutOfRangeException.

            Assert.IsNull(structureViewModel.Root);
            Assert.AreEqual(structureViewModel.Root, structureViewModel.Roots.Single());

            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Root)), Times.Never);
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Roots)), Times.Never);
        }

        [TestMethod]
        public void TestSelected()
        {
            var testSource = new FeatureSet(DotSpatial.Topology.FeatureType.Point);
            testSource.Name = "TestName";
            var testFeature0 = testSource.AddFeature(new DotSpatial.Topology.Point(0.0, 0.0));
            var testFeature1 = testSource.AddFeature(new DotSpatial.Topology.Point(1.0, 1.0));
            var testFeature2 = testSource.AddFeature(new DotSpatial.Topology.Point(2.0, 0.0));

            var mockRightRegion = new Mock<IRegion>();
            var mockRegionCollection = new Mock<IRegionCollection>();
            mockRegionCollection.Setup(m => m.ContainsRegionWithName(It.IsAny<string>())).Returns(true);
            mockRegionCollection.Setup(m => m[Constants.Region.Right]).Returns(mockRightRegion.Object);
            var mockRegionManager = new Mock<IRegionManager>();
            mockRegionManager.Setup(m => m.Regions).Returns(mockRegionCollection.Object);
            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockRegionManager.Object, mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            structureViewModel.Selected = structureViewModel.Root.Children.Single(IsStructureItemViewModel("Features")).Children.First();
            structureViewModel.Selected = structureViewModel.Root.Children.Single(IsStructureItemViewModel("Features")).Children.Last();

            // Moq doesn't support sequences / verifying the invocation order. See: Java - Mockito - InOrder
            mockRightRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Properties), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Properties.Source] == testFeature0)));
            mockFeatureSelectedEvent.Verify(m => m.Publish(testFeature0));
            mockRightRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Properties), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Properties.Source] == testFeature2)));
            mockFeatureSelectedEvent.Verify(m => m.Publish(testFeature2));
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Selected)), Times.Exactly(2));
        }

        [TestMethod]
        public void TestSelectedNull()
        {
            var testSource = new FeatureSet(DotSpatial.Topology.FeatureType.Point);
            testSource.Name = "TestName";
            var testFeature0 = testSource.AddFeature(new DotSpatial.Topology.Point(0.0, 0.0));
            var testFeature1 = testSource.AddFeature(new DotSpatial.Topology.Point(1.0, 1.0));
            var testFeature2 = testSource.AddFeature(new DotSpatial.Topology.Point(2.0, 0.0));

            var mockRegionManager = new Mock<IRegionManager>();
            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockRegionManager.Object, mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            structureViewModel.Selected = null;

            mockRegionManager.Verify(m => m.Regions, Times.Never);
            mockFeatureSelectedEvent.Verify(m => m.Publish(It.IsAny<IFeature>()), Times.Never);
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Selected)), Times.Never);
        }

        [TestMethod]
        public void TestSelectedEvent()
        {
            var testSource = new FeatureSet(DotSpatial.Topology.FeatureType.Point);
            testSource.Name = "TestName";
            var testFeature0 = testSource.AddFeature(new DotSpatial.Topology.Point(0.0, 0.0));
            var testFeature1 = testSource.AddFeature(new DotSpatial.Topology.Point(1.0, 1.0));
            var testFeature2 = testSource.AddFeature(new DotSpatial.Topology.Point(2.0, 0.0));

            var mockRightRegion = new Mock<IRegion>();
            var mockRegionCollection = new Mock<IRegionCollection>();
            mockRegionCollection.Setup(m => m.ContainsRegionWithName(It.IsAny<string>())).Returns(true);
            mockRegionCollection.Setup(m => m[Constants.Region.Right]).Returns(mockRightRegion.Object);
            var mockRegionManager = new Mock<IRegionManager>();
            mockRegionManager.Setup(m => m.Regions).Returns(mockRegionCollection.Object);
            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockFeatureSelectedEventCallback = (Action<IFeature>)null;
            mockFeatureSelectedEvent
                .Setup(m => m.Subscribe(It.IsAny<Action<IFeature>>(), It.IsAny<ThreadOption>(), It.IsAny<bool>(), It.IsAny<Predicate<IFeature>>()))
                .Callback<Action<IFeature>, ThreadOption, bool, Predicate<IFeature>>((a, t, k, f) => mockFeatureSelectedEventCallback = a);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockRegionManager.Object, mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            mockFeatureSelectedEventCallback(testFeature0);
            Assert.AreEqual(structureViewModel.Root.Children.Single(IsStructureItemViewModel("Features")).Children.First(), structureViewModel.Selected);
            mockFeatureSelectedEventCallback(testFeature2);
            Assert.AreEqual(structureViewModel.Root.Children.Single(IsStructureItemViewModel("Features")).Children.Last(), structureViewModel.Selected);

            // Moq doesn't support sequences / verifying the invocation order. See: Java - Mockito - InOrder
            mockRightRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Properties), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Properties.Source] == testFeature0)));
            mockRightRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Properties), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Properties.Source] == testFeature2)));
            mockFeatureSelectedEvent.Verify(m => m.Publish(It.IsAny<IFeature>()), Times.Never);
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Selected)), Times.Exactly(2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSelectedEventNull()
        {
            var testSource = new FeatureSet(DotSpatial.Topology.FeatureType.Point);
            testSource.Name = "TestName";
            var testFeature0 = testSource.AddFeature(new DotSpatial.Topology.Point(0.0, 0.0));
            var testFeature1 = testSource.AddFeature(new DotSpatial.Topology.Point(1.0, 1.0));
            var testFeature2 = testSource.AddFeature(new DotSpatial.Topology.Point(2.0, 0.0));

            var mockRightRegion = new Mock<IRegion>();
            var mockRegionCollection = new Mock<IRegionCollection>();
            mockRegionCollection.Setup(m => m.ContainsRegionWithName(It.IsAny<string>())).Returns(true);
            mockRegionCollection.Setup(m => m[Constants.Region.Right]).Returns(mockRightRegion.Object);
            var mockRegionManager = new Mock<IRegionManager>();
            mockRegionManager.Setup(m => m.Regions).Returns(mockRegionCollection.Object);
            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockFeatureSelectedEventCallback = (Action<IFeature>)null;
            mockFeatureSelectedEvent
                .Setup(m => m.Subscribe(It.IsAny<Action<IFeature>>(), It.IsAny<ThreadOption>(), It.IsAny<bool>(), It.IsAny<Predicate<IFeature>>()))
                .Callback<Action<IFeature>, ThreadOption, bool, Predicate<IFeature>>((a, t, k, f) => mockFeatureSelectedEventCallback = a);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockRegionManager.Object, mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            mockFeatureSelectedEventCallback(null); // This should throw an ArgumentNullException.
            Assert.IsNull(structureViewModel.Selected);

            mockRightRegion.Verify(m => m.RequestNavigate(ItIsUri(Constants.Navigation.Properties), It.IsAny<Action<NavigationResult>>(), It.Is<NavigationParameters>(np => np[Constants.NavigationParameters.Properties.Source] == testSource)));
            mockFeatureSelectedEvent.Verify(m => m.Publish(It.IsAny<IFeature>()), Times.Never);
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Selected)), Times.Never);
        }

        private NavigationContext MockNavigationContext(object source)
        {
            var mockRegion = new Mock<IRegion>();
            var mockRegionNavigationService = new Mock<IRegionNavigationService>();
            mockRegionNavigationService.Setup(m => m.Region).Returns(mockRegion.Object);

            return new NavigationContext(
                mockRegionNavigationService.Object,
                new Uri(Constants.Navigation.Structure, UriKind.Relative),
                new NavigationParameters() 
                { 
                    { Constants.NavigationParameters.Structure.Source, source } 
                });
        }

        private static Func<StructureItemViewModel, bool> IsStructureItemViewModel(string name)
        {
            return sivm => sivm.Name == name;
        }

        private static Func<StructureItemViewModel, bool> IsStructureItemViewModel(string name, string type)
        {
            return sivm => sivm.Name == name && sivm.Type == type;
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
