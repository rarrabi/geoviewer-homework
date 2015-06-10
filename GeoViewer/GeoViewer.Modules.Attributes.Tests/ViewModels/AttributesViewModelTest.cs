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

namespace GeoViewer.Modules.Attributes.ViewModels
{
    [TestClass]
    public class AttributesViewModelTest
    {
        [TestMethod]
        public void TestAttributes()
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

            var testSource = new FeatureSet(DotSpatial.Topology.FeatureType.Point);
            testSource.DataTable.Columns.Add(new DataColumn(testName0, testType0));
            testSource.DataTable.Columns.Add(new DataColumn(testName1, testType1));
            testSource.DataTable.Columns.Add(new DataColumn(testName2, testType2));
            var testFeature = testSource.AddFeature(new DotSpatial.Topology.Point());
            testFeature.DataRow.BeginEdit();
            testFeature.DataRow[testName0] = testValue0;
            testFeature.DataRow[testName1] = testValue1;
            testFeature.DataRow[testName2] = testValue2;
            testFeature.DataRow.EndEdit();

            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var attributesViewModel = new AttributesViewModel();
            attributesViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            attributesViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            Assert.IsNotNull(attributesViewModel.Attributes);
            Assert.AreEqual(testName0, attributesViewModel.Attributes.Table.Columns[0].ColumnName);
            Assert.AreEqual(testName1, attributesViewModel.Attributes.Table.Columns[1].ColumnName);
            Assert.AreEqual(testName2, attributesViewModel.Attributes.Table.Columns[2].ColumnName);
            Assert.AreEqual(testType0, attributesViewModel.Attributes.Table.Columns[0].DataType);
            Assert.AreEqual(testType1, attributesViewModel.Attributes.Table.Columns[1].DataType);
            Assert.AreEqual(testType2, attributesViewModel.Attributes.Table.Columns[2].DataType);
            Assert.AreEqual(testValue0, attributesViewModel.Attributes[0][0]);
            Assert.AreEqual(testValue1, attributesViewModel.Attributes[0][1]);
            Assert.AreEqual(testValue2, attributesViewModel.Attributes[0][2]);

            mockPropertyChangedEventHandler.Verify(m => m(attributesViewModel, ItIsProperty(() => attributesViewModel.Attributes)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestAttributesInvalid()
        {
            var testSource = new object();

            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var attributesViewModel = new AttributesViewModel();
            attributesViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            attributesViewModel.OnNavigatedTo(this.MockNavigationContext(testSource)); // This should throw an ArgumentOutOfRangeException.

            Assert.IsNull(attributesViewModel.Attributes);

            mockPropertyChangedEventHandler.Verify(m => m(attributesViewModel, ItIsProperty(() => attributesViewModel.Attributes)), Times.Never);
        }

        private NavigationContext MockNavigationContext(object source)
        {
            var mockRegion = new Mock<IRegion>();
            var mockRegionNavigationService = new Mock<IRegionNavigationService>();
            mockRegionNavigationService.Setup(m => m.Region).Returns(mockRegion.Object);

            return new NavigationContext(
                mockRegionNavigationService.Object,
                new Uri(Constants.Navigation.Attributes, UriKind.Relative),
                new NavigationParameters() 
                { 
                    { Constants.NavigationParameters.Attributes.Source, source } 
                });
        }

        private static PropertyChangedEventArgs ItIsProperty<T>(Expression<Func<T>> propertyExpression)
        {
            return It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertySupport.ExtractPropertyName(propertyExpression));
        }
    }
}
