using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GeoViewer.Modules.Properties.ViewModels
{
    [TestClass]
    public class PropertiesViewModelTest
    {
        [TestMethod]
        public void TestProperties()
        {
            var testName0 = "TestName0";
            var testType0 = "TestType0";
            var testValue0 = "TestValue0";
            var testName1 = "TestName1";
            var testType1 = "TestType1";
            var testValue1 = "TestValue1";
            var testName2 = "TestName2";
            var testType2 = "TestType2";
            var testValue2 = "TestValue2";

            // TODO PropertiesViewModelTest#TestProperties
            var testSource = (object)null;

            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var propertiesViewModel = new PropertiesViewModel();
            propertiesViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            propertiesViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            Assert.IsNotNull(propertiesViewModel.Properties);
            Assert.AreEqual(testName0, propertiesViewModel.Properties[0].Name);
            Assert.AreEqual(testType0, propertiesViewModel.Properties[0].Type);
            Assert.AreEqual(testValue0, propertiesViewModel.Properties[0].Value);
            Assert.AreEqual(testName1, propertiesViewModel.Properties[1].Name);
            Assert.AreEqual(testType1, propertiesViewModel.Properties[1].Type);
            Assert.AreEqual(testValue1, propertiesViewModel.Properties[1].Value);
            Assert.AreEqual(testName2, propertiesViewModel.Properties[2].Name);
            Assert.AreEqual(testType2, propertiesViewModel.Properties[2].Type);
            Assert.AreEqual(testValue2, propertiesViewModel.Properties[2].Value);

            mockPropertyChangedEventHandler.Verify(m => m(propertiesViewModel, ItIsProperty(() => propertiesViewModel.Properties)));
        }

        private NavigationContext MockNavigationContext(object source)
        {
            var navigationService = new Mock<IRegionNavigationService>().Object;
            var uri = new Uri("TestUri", UriKind.Relative);
            var navigationParameters =
                new NavigationParameters() 
                { 
                    { Constants.NavigationParameters.Properties.Source, source } 
                };

            return new NavigationContext(navigationService, uri, navigationParameters);
        }

        private static PropertyChangedEventArgs ItIsProperty<T>(Expression<Func<T>> propertyExpression)
        {
            return It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertySupport.ExtractPropertyName(propertyExpression));
        }
    }
}
