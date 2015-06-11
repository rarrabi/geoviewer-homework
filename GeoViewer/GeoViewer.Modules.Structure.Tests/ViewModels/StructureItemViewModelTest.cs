using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoViewer.Modules.Structure.ViewModels
{
    [TestClass]
    public class StructureItemViewModelTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullName()
        {
            var testType = "TestType";
            var testChildren = Enumerable.Empty<StructureItemViewModel>();

            var structureItemViewModel = new StructureItemViewModel(null, testType, testChildren);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullNameDefaultChildren()
        {
            var testType = "TestType";

            var structureItemViewModel = new StructureItemViewModel(null, testType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullType()
        {
            var testName = "TestName";
            var testChildren = Enumerable.Empty<StructureItemViewModel>();

            var structureItemViewModel = new StructureItemViewModel(testName, null, testChildren);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullTypeDefaultChildren()
        {
            var testName = "TestName";

            var structureItemViewModel = new StructureItemViewModel(testName, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullChildren()
        {
            var testName = "TestName";
            var testType = "TestType";

            var structureItemViewModel = new StructureItemViewModel(testName, testType, null);
        }

        [TestMethod]
        public void TestDefaultChildren()
        {
            var testName = "TestName";
            var testType = "TestType";
            var testChildren = Enumerable.Empty<StructureItemViewModel>();

            var structureItemViewModel = new StructureItemViewModel(testName, testType);

            Assert.IsTrue(Enumerable.SequenceEqual(testChildren, structureItemViewModel.Children));
        }

        [TestMethod]
        public void TestName()
        {
            var testName = "TestName";
            var testType = "TestType";
            var testChildren =
                new List<StructureItemViewModel>() 
                { 
                    new StructureItemViewModel("TestName1", "TestType1"), 
                    new StructureItemViewModel("TestName2", "TestType2"), 
                    new StructureItemViewModel("TestName3", "TestType3") 
                }.AsReadOnly();

            var structureItemViewModel = new StructureItemViewModel(testName, testType, testChildren);

            Assert.AreEqual(testName, structureItemViewModel.Name);
        }

        [TestMethod]
        public void TestType()
        {
            var testName = "TestName";
            var testType = "TestType";
            var testChildren =
                new List<StructureItemViewModel>()
                { 
                    new StructureItemViewModel("TestName1", "TestType1"),
                    new StructureItemViewModel("TestName2", "TestType2"),
                    new StructureItemViewModel("TestName3", "TestType3") 
                }.AsReadOnly();

            var structureItemViewModel = new StructureItemViewModel(testName, testType, testChildren);

            Assert.AreEqual(testType, structureItemViewModel.Type);
        }

        [TestMethod]
        public void TestChildren()
        {
            var testName = "TestName";
            var testType = "TestType";
            var testChildren =
                new List<StructureItemViewModel>() 
                {
                    new StructureItemViewModel("TestName1", "TestType1"), 
                    new StructureItemViewModel("TestName2", "TestType2"),
                    new StructureItemViewModel("TestName3", "TestType3") 
                }.AsReadOnly();

            var structureItemViewModel = new StructureItemViewModel(testName, testType, testChildren);

            Assert.IsTrue(Enumerable.SequenceEqual(testChildren, structureItemViewModel.Children));
        }
    }
}
