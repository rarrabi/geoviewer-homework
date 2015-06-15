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
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SCG = System.Collections.Generic;

namespace GeoViewer.Modules.Properties.ViewModels
{
    [TestClass]
    public class PropertiesViewModelTest
    {
        [TestMethod]
        public void TestPropertiesFeatureSet()
        {
            var testSource = new FeatureSet(DotSpatial.Topology.FeatureType.Point);
            testSource.Name = "TestName";
            testSource.Projection = DotSpatial.Projections.KnownCoordinateSystems.Projected.NationalGrids.HD1972EgysegesOrszagosVetuleti;
            testSource.AddFeature(new DotSpatial.Topology.Point(0.0, 0.0));
            testSource.AddFeature(new DotSpatial.Topology.Point(1.0, 1.0));
            testSource.AddFeature(new DotSpatial.Topology.Point(2.0, 0.0));
            testSource.UpdateExtent();

            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var propertiesViewModel = new PropertiesViewModel();
            propertiesViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            propertiesViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            Assert.IsNotNull(propertiesViewModel.Properties);
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel("Name", testSource.Name)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel("Feature Type", testSource.FeatureType)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel("Feature Count", testSource.Features.Count)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel("Projection", testSource.Projection)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel("Width", testSource.Extent.Width)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel("Height", testSource.Extent.Height)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel("MinX", testSource.Extent.MinX)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel("MaxX", testSource.Extent.MaxX)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel("MinY", testSource.Extent.MinY)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel("MaxY", testSource.Extent.MaxY)));

            mockPropertyChangedEventHandler.Verify(m => m(propertiesViewModel, ItIsProperty(() => propertiesViewModel.Properties)));
        }

        [TestMethod]
        public void TestPropertiesFeature()
        {
            var testName0 = "TestName0";
            var testName1 = "TestName1";
            var testName2 = "TestName2";
            var testType0 = typeof(string);
            var testType1 = typeof(string);
            var testType2 = typeof(string);
            var testValue0 = "TestValue0";
            var testValue1 = "TestValue1";
            var testValue2 = "TestValue2";

            var testFeatureSet = new FeatureSet(DotSpatial.Topology.FeatureType.Point);
            testFeatureSet.DataTable.Columns.Add(new DataColumn(testName0, testType0));
            testFeatureSet.DataTable.Columns.Add(new DataColumn(testName1, testType1));
            testFeatureSet.DataTable.Columns.Add(new DataColumn(testName2, testType2));
            var testSource = testFeatureSet.AddFeature(new DotSpatial.Topology.Point(0.0, 0.0));
            testSource.DataRow.BeginEdit();
            testSource.DataRow[testName0] = testValue0;
            testSource.DataRow[testName1] = testValue1;
            testSource.DataRow[testName2] = testValue2;
            testSource.DataRow.EndEdit();

            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var propertiesViewModel = new PropertiesViewModel();
            propertiesViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            propertiesViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            Assert.IsNotNull(propertiesViewModel.Properties);
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel("ID", "ID", testSource.Fid)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel(testName0, testType0.Name, testValue0)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel(testName1, testType1.Name, testValue1)));
            Assert.IsTrue(propertiesViewModel.Properties.Any(IsPropertiesItemViewModel(testName2, testType2.Name, testValue2)));

            mockPropertyChangedEventHandler.Verify(m => m(propertiesViewModel, ItIsProperty(() => propertiesViewModel.Properties)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPropertiesInvalid()
        {
            var testSource = new object();

            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var propertiesViewModel = new PropertiesViewModel();
            propertiesViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            propertiesViewModel.OnNavigatedTo(this.MockNavigationContext(testSource)); // This should throw an ArgumentOutOfRangeException.

            Assert.IsNull(propertiesViewModel.Properties);

            mockPropertyChangedEventHandler.Verify(m => m(propertiesViewModel, ItIsProperty(() => propertiesViewModel.Properties)), Times.Never);
        }

        private static Func<PropertiesItemViewModel, bool> IsPropertiesItemViewModel<T>(string name, T value)
        {
            return pivm => pivm.Name == name && object.Equals(pivm.Value, value);
        }

        private static Func<PropertiesItemViewModel, bool> IsPropertiesItemViewModel<T>(string name, string type, T value)
        {
            return pivm => pivm.Name == name && pivm.Type == type && object.Equals(pivm.Value, value);
        }

        private static PropertyChangedEventArgs ItIsProperty<T>(Expression<Func<T>> propertyExpression)
        {
            return It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertySupport.ExtractPropertyName(propertyExpression));
        }

        private NavigationContext MockNavigationContext(object source)
        {
            var mockRegion = new Mock<IRegion>();
            var mockRegionNavigationService = new Mock<IRegionNavigationService>();
            mockRegionNavigationService.Setup(m => m.Region).Returns(mockRegion.Object);

            return new NavigationContext(
                mockRegionNavigationService.Object,
                new Uri(Constants.Navigation.Properties, UriKind.Relative),
                new NavigationParameters() 
                { 
                    { Constants.NavigationParameters.Properties.Source, source } 
                });
        }
    }
}
