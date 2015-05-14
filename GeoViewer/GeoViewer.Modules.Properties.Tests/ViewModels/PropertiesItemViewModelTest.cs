using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoViewer.Modules.Properties.ViewModels
{
    [TestClass]
    public class PropertiesItemViewModelTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullName()
        {
            var testType = "TestType";
            var testValue = "TestValue";

            var propertiesItemViewModel = new PropertiesItemViewModel(null, testType, testValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullType()
        {
            var testName = "TestName";
            var testValue = "TestValue";

            var propertiesItemViewModel = new PropertiesItemViewModel(testName, null, testValue);
        }

        [TestMethod]
        public void TestName()
        {
            var testName = "TestName";
            var testType = "TestType";
            var testValue = "TestValue";

            var propertiesItemViewModel = new PropertiesItemViewModel(testName, testType, testValue);

            Assert.AreEqual(testName, propertiesItemViewModel.Name);
        }

        [TestMethod]
        public void TestType()
        {
            var testName = "TestName";
            var testType = "TestType";
            var testValue = "TestValue";

            var propertiesItemViewModel = new PropertiesItemViewModel(testName, testType, testValue);

            Assert.AreEqual(testType, propertiesItemViewModel.Type);
        }

        [TestMethod]
        public void TestValue()
        {
            var testName = "TestName";
            var testType = "TestType";
            var testValue = "TestValue";

            var propertiesItemViewModel = new PropertiesItemViewModel(testName, testType, testValue);

            Assert.AreEqual(testValue, propertiesItemViewModel.Value);
        }

        [TestMethod]
        public void TestValueNull()
        {
            var testName = "TestName";
            var testType = "TestType";

            var propertiesItemViewModel = new PropertiesItemViewModel(testName, testType, null);

            Assert.IsNull(propertiesItemViewModel.Value);
        }
    }
}
