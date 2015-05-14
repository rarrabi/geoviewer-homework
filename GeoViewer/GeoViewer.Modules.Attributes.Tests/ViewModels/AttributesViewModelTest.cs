using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
            var testValue0 = "TestValue0";
            var testName1 = "TestName1";
            var testValue1 = "TestValue1";
            var testName2 = "TestName2";
            var testValue2 = "TestValue2";

            // TODO PropertiesViewModelTest#TestProperties
            var testSource = (object)null;

            var mockPropertyChangedEventHandler = new Mock<PropertyChangedEventHandler>();

            var attributesViewModel = new AttributesViewModel();
            attributesViewModel.PropertyChanged += mockPropertyChangedEventHandler.Object;
            attributesViewModel.OnNavigatedTo(this.MockNavigationContext(testSource));

            Assert.IsNotNull(attributesViewModel.Attributes);
            Assert.AreEqual(testName0, attributesViewModel.Attributes.Table.Columns[0].ColumnName);
            Assert.AreEqual(testValue0, attributesViewModel.Attributes[0][0]);
            Assert.AreEqual(testName1, attributesViewModel.Attributes.Table.Columns[1].ColumnName);
            Assert.AreEqual(testValue1, attributesViewModel.Attributes[0][1]);
            Assert.AreEqual(testName2, attributesViewModel.Attributes.Table.Columns[2].ColumnName);
            Assert.AreEqual(testValue2, attributesViewModel.Attributes[0][2]);

            mockPropertyChangedEventHandler.Verify(m => m(attributesViewModel, ItIsProperty(() => attributesViewModel.Attributes)));
        }

        private NavigationContext MockNavigationContext(object source)
        {
            var navigationService = new Mock<IRegionNavigationService>().Object;
            var uri = new Uri("TestUri", UriKind.Relative);
            var navigationParameters =
                new NavigationParameters() 
                { 
                    { Constants.NavigationParameters.Attributes.Source, source } 
                };

            return new NavigationContext(navigationService, uri, navigationParameters);
        }

        private static PropertyChangedEventArgs ItIsProperty<T>(Expression<Func<T>> propertyExpression)
        {
            return It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertySupport.ExtractPropertyName(propertyExpression));
        }
    }
}
