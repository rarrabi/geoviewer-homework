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
        public void TestNullEventAggregator()
        {
            var structureViewModel = new StructureViewModel(null);
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

            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            Assert.IsNotNull(structureViewModel.Root);
            Assert.AreEqual(structureViewModel.Root, structureViewModel.Roots.Single());
            Assert.AreEqual(testSource.Name, structureViewModel.Root.Name);
            Assert.AreEqual(testSource.GetType().Name, structureViewModel.Root.Type);
            Assert.IsTrue(structureViewModel.Root.Children.Any(
                sivm => sivm.Name == "Attributes" && sivm.Type == testSource.DataTable.GetType().Name &&
                        sivm.Children.Any(csivm => csivm.Name == testName0 && csivm.Type == testType0.Name) &&
                        sivm.Children.Any(csivm => csivm.Name == testName1 && csivm.Type == testType1.Name) &&
                        sivm.Children.Any(csivm => csivm.Name == testName1 && csivm.Type == testType2.Name)));
            Assert.IsTrue(structureViewModel.Root.Children.Any(
                sivm => sivm.Name == "Features" && sivm.Type == string.Format("[{0}]", testSource.FeatureType) &&
                        sivm.Children.Any(csivm => csivm.Name == testFeature0.Fid.ToString() && csivm.Type == testFeature0.FeatureType.ToString()) &&
                        sivm.Children.Any(csivm => csivm.Name == testFeature1.Fid.ToString() && csivm.Type == testFeature1.FeatureType.ToString()) &&
                        sivm.Children.Any(csivm => csivm.Name == testFeature2.Fid.ToString() && csivm.Type == testFeature2.FeatureType.ToString())));

            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Root)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestRootInvalid()
        {
            var testSource = new object();

            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
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

            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            structureViewModel.Selected = structureViewModel.Root.Children.Where(sivm => sivm.Name == "Features").Single().Children.First();
            structureViewModel.Selected = structureViewModel.Root.Children.Where(sivm => sivm.Name == "Features").Single().Children.Last();

            // Moq doesn't support sequences / verifying the invocation order. (See: Java - Mockito - InOrder).
            mockFeatureSelectedEvent.Verify(m => m.Publish(testFeature0));
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

            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            structureViewModel.Selected = null;

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

            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockFeatureSelectedEventCallback = (Action<IFeature>)null;
            mockFeatureSelectedEvent
                .Setup(m => m.Subscribe(It.IsAny<Action<IFeature>>(), It.IsAny<ThreadOption>(), It.IsAny<bool>(), It.IsAny<Predicate<IFeature>>()))
                .Callback<Action<IFeature>, ThreadOption, bool, Predicate<IFeature>>((a, t, k, f) => mockFeatureSelectedEventCallback = a);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            mockFeatureSelectedEventCallback(testFeature0);
            Assert.AreEqual(structureViewModel.Root.Children.Where(sivm => sivm.Name == "Features").Single().Children.First(), structureViewModel.Selected);
            mockFeatureSelectedEventCallback(testFeature2);
            Assert.AreEqual(structureViewModel.Root.Children.Where(sivm => sivm.Name == "Features").Single().Children.Last(), structureViewModel.Selected);

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

            var mockFeatureSelectedEvent = new Mock<FeatureSelectedEvent>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(m => m.GetEvent<FeatureSelectedEvent>()).Returns(mockFeatureSelectedEvent.Object);
            var mockFeatureSelectedEventCallback = (Action<IFeature>)null;
            mockFeatureSelectedEvent
                .Setup(m => m.Subscribe(It.IsAny<Action<IFeature>>(), It.IsAny<ThreadOption>(), It.IsAny<bool>(), It.IsAny<Predicate<IFeature>>()))
                .Callback<Action<IFeature>, ThreadOption, bool, Predicate<IFeature>>((a, t, k, f) => mockFeatureSelectedEventCallback = a);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            mockFeatureSelectedEventCallback(null); // This should throw an ArgumentNullException.
            Assert.IsNull(structureViewModel.Selected);

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

        private static PropertyChangedEventArgs ItIsProperty<T>(Expression<Func<T>> propertyExpression)
        {
            return It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertySupport.ExtractPropertyName(propertyExpression));
        }
    }
}
