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
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == "Name" && (string)pvm.Value == testSource.Name));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == "Feature Type" && (DotSpatial.Topology.FeatureType)pvm.Value == testSource.FeatureType));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == "Feature Count" && (int)pvm.Value == testSource.Features.Count));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == "Projection" && pvm.Value == testSource.Projection));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == "Width" && (double)pvm.Value == testSource.Extent.Width));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == "Height" && (double)pvm.Value == testSource.Extent.Height));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == "MinX" && (double)pvm.Value == testSource.Extent.MinX));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == "MaxX" && (double)pvm.Value == testSource.Extent.MaxX));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == "MinY" && (double)pvm.Value == testSource.Extent.MinY));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == "MaxY" && (double)pvm.Value == testSource.Extent.MaxY));

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
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == "ID" && pvm.Type == "ID" && (int)pvm.Value == testSource.Fid));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == testName0 && pvm.Type == testType0.Name && (string)pvm.Value == testValue0));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == testName1 && pvm.Type == testType1.Name && (string)pvm.Value == testValue1));
            Assert.IsTrue(propertiesViewModel.Properties.Any(pvm => pvm.Name == testName2 && pvm.Type == testType2.Name && (string)pvm.Value == testValue2));

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

        private static PropertyChangedEventArgs ItIsProperty<T>(Expression<Func<T>> propertyExpression)
        {
            return It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertySupport.ExtractPropertyName(propertyExpression));
        }
    }
}
