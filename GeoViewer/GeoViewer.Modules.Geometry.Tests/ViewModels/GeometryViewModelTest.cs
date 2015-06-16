using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GeoViewer.Modules.Geometry.ViewModels
{
    [TestClass]
    public class GeometryViewModelTest
    {
        [TestMethod]
        public void TestFeatureSet()
        {
            var testSource = new FeatureSet();

            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var geometryViewModel = new GeometryViewModel();
            geometryViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            geometryViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            Assert.AreEqual(testSource, geometryViewModel.FeatureSet);

            mockPropertyChangedEventHandler.Verify(m => m(geometryViewModel, ItIsProperty(() => geometryViewModel.FeatureSet)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestFeatureSetInvalid()
        {
            var testSource = new object();

            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var geometryViewModel = new GeometryViewModel();
            geometryViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            geometryViewModel.OnNavigatedTo(this.MockNavigationContext(testSource)); // This should throw an ArgumentOutOfRangeException.

            Assert.IsNull(geometryViewModel.FeatureSet);

            mockPropertyChangedEventHandler.Verify(m => m(geometryViewModel, ItIsProperty(() => geometryViewModel.FeatureSet)), Times.Never);
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
                new Uri(Constants.Navigation.Geometry, UriKind.Relative),
                new NavigationParameters() 
                { 
                    { Constants.NavigationParameters.Geometry.Source, source } 
                });
        }
    }
}
