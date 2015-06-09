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
            var testName = "TestName";
            var testType = "TestType";
            var testChildren = Enumerable.Empty<StructureItemViewModel>();

            // TODO StructureViewModelTest#TestRoot
            var testSource = (object)null;

            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockSelectedEvent = new Mock<SelectedEvent>();
            mockEventAggregator.Setup(m => m.GetEvent<SelectedEvent>()).Returns(mockSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            Assert.IsNotNull(structureViewModel.Root);
            Assert.AreEqual(testName, structureViewModel.Root.Name);
            Assert.AreEqual(testType, structureViewModel.Root.Type);
            Assert.IsTrue(Enumerable.SequenceEqual(testChildren, structureViewModel.Root.Children));

            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Root)));
        }

        public void TestRootHierarchy()
        {
            var testName = "TestName";
            var testType = "TestType";
            var testName0 = "TestName0";
            var testType0 = "TestType0";
            var testName1 = "TestName1";
            var testType1 = "TestType1";
            var testName2 = "TestName2";
            var testType2 = "TestType2";

            // TODO StructureViewModelTest#TestRootHierarchy
            var testSource = (object)null;

            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockSelectedEvent = new Mock<SelectedEvent>();
            mockEventAggregator.Setup(m => m.GetEvent<SelectedEvent>()).Returns(mockSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            Assert.IsNotNull(structureViewModel.Root);
            Assert.AreEqual(testName, structureViewModel.Root.Name);
            Assert.AreEqual(testType, structureViewModel.Root.Type);
            Assert.AreEqual(testName0, structureViewModel.Root.Children[0].Name);
            Assert.AreEqual(testType0, structureViewModel.Root.Children[0].Type);
            Assert.AreEqual(testName1, structureViewModel.Root.Children[1].Name);
            Assert.AreEqual(testType1, structureViewModel.Root.Children[1].Type);
            Assert.AreEqual(testName2, structureViewModel.Root.Children[2].Name);
            Assert.AreEqual(testType2, structureViewModel.Root.Children[2].Type);

            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Root)));
        }

        [TestMethod]
        public void TestSelectedNull()
        {
            // TODO StructureViewModelTest#TestSelectedNull
            var testSource = (object)null;

            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockSelectedEvent = new Mock<SelectedEvent>();
            mockEventAggregator.Setup(m => m.GetEvent<SelectedEvent>()).Returns(mockSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            structureViewModel.Selected = null;
            Assert.IsNull(structureViewModel.Selected);

            mockSelectedEvent.Verify(m => m.Publish(It.IsAny<object>()), Times.Never);
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Selected)), Times.Never);
        }

        [TestMethod]
        public void TestSelectedRoot()
        {
            // TODO StructureViewModelTest#TestSelectedRoot
            var testSource = (object)null;

            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockSelectedEvent = new Mock<SelectedEvent>();
            mockEventAggregator.Setup(m => m.GetEvent<SelectedEvent>()).Returns(mockSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            structureViewModel.Selected = structureViewModel.Root;
            Assert.AreEqual(structureViewModel.Root, structureViewModel.Selected);

            mockSelectedEvent.Verify(m => m.Publish(testSource));
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Selected)));
        }

        [TestMethod]
        public void TestSelectedHierarchy()
        {
            // TODO StructureViewModelTest#TestSelectedHierarchy
            var testSource = (object)null;

            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockSelectedEvent = new Mock<SelectedEvent>();
            mockEventAggregator.Setup(m => m.GetEvent<SelectedEvent>()).Returns(mockSelectedEvent.Object);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            structureViewModel.Selected = structureViewModel.Root.Children[1];
            Assert.AreEqual(structureViewModel.Root.Children[1], structureViewModel.Selected);

            // TODO StructureViewModelTest#TestSelectedHierarchy
            mockSelectedEvent.Verify(m => m.Publish(null));
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Selected)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSelectedEventNull()
        {
            // TODO StructureViewModelTest#TestSelectedEventNull
            var testSource = (object)null;

            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockSelectedEvent = new Mock<SelectedEvent>();
            mockEventAggregator.Setup(m => m.GetEvent<SelectedEvent>()).Returns(mockSelectedEvent.Object);
            var mockSelectedEventCallback = (Action<object>)null;
            mockSelectedEvent
                .Setup(m => m.Subscribe(It.IsAny<Action<object>>(), It.IsAny<ThreadOption>(), It.IsAny<bool>(), It.IsAny<Predicate<object>>()))
                .Callback<Action<object>, ThreadOption, bool, Predicate<object>>((a, t, k, f) => mockSelectedEventCallback = a);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            mockSelectedEventCallback(null); // This should throw an ArgumentNullException.
            Assert.IsNull(structureViewModel.Selected);

            mockSelectedEvent.Verify(m => m.Publish(It.IsAny<object>()), Times.Never);
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Selected)), Times.Never);
        }

        [TestMethod]
        public void TestSelectedEventRoot()
        {
            // TODO StructureViewModelTest#TestSelectedEventRoot
            var testSource = (object)null;

            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockSelectedEvent = new Mock<SelectedEvent>();
            mockEventAggregator.Setup(m => m.GetEvent<SelectedEvent>()).Returns(mockSelectedEvent.Object);
            var mockSelectedEventCallback = (Action<object>)null;
            mockSelectedEvent
                .Setup(m => m.Subscribe(It.IsAny<Action<object>>(), It.IsAny<ThreadOption>(), It.IsAny<bool>(), It.IsAny<Predicate<object>>()))
                .Callback<Action<object>, ThreadOption, bool, Predicate<object>>((a, t, k, f) => mockSelectedEventCallback = a);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            mockSelectedEventCallback(testSource);
            Assert.AreEqual(structureViewModel.Root, structureViewModel.Selected);

            mockSelectedEvent.Verify(m => m.Publish(It.IsAny<object>()), Times.Never);
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Selected)));
        }

        [TestMethod]
        public void TestSelectedEventHierachy()
        {
            // TODO StructureViewModelTest#TestSelectedEventHierachy
            var testSource = (object)null;

            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockSelectedEvent = new Mock<SelectedEvent>();
            mockEventAggregator.Setup(m => m.GetEvent<SelectedEvent>()).Returns(mockSelectedEvent.Object);
            var mockSelectedEventCallback = (Action<object>)null;
            mockSelectedEvent
                .Setup(m => m.Subscribe(It.IsAny<Action<object>>(), It.IsAny<ThreadOption>(), It.IsAny<bool>(), It.IsAny<Predicate<object>>()))
                .Callback<Action<object>, ThreadOption, bool, Predicate<object>>((a, t, k, f) => mockSelectedEventCallback = a);
            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var structureViewModel = new StructureViewModel(mockEventAggregator.Object);
            structureViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            structureViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            // TODO StructureViewModelTest#TestSelectedEventHierachy
            mockSelectedEventCallback(null);
            Assert.AreEqual(structureViewModel.Root.Children[1], structureViewModel.Selected);

            mockSelectedEvent.Verify(m => m.Publish(It.IsAny<object>()), Times.Never);
            mockPropertyChangedEventHandler.Verify(m => m(structureViewModel, ItIsProperty(() => structureViewModel.Selected)));
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
