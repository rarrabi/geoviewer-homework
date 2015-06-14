using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoViewer.Common
{
    [TestClass]
    public class ConstantsTest
    {
        [TestMethod]
        public void TestRegion()
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(Constants.Region.Left));
            Assert.IsFalse(string.IsNullOrWhiteSpace(Constants.Region.Main));
            Assert.IsFalse(string.IsNullOrWhiteSpace(Constants.Region.Right));
        }

        [TestMethod]
        public void TestNavigation()
        {
            Assert.AreEqual(Constants.Navigation.Welcome, typeof(Modules.Welcome.Views.WelcomeView).FullName);
            Assert.AreEqual(Constants.Navigation.Structure, typeof(Modules.Structure.Views.StructureView).FullName);
            Assert.AreEqual(Constants.Navigation.Properties, typeof(Modules.Properties.Views.PropertiesView).FullName);
            Assert.AreEqual(Constants.Navigation.Attributes, typeof(Modules.Attributes.Views.AttributesView).FullName);
            Assert.AreEqual(Constants.Navigation.Geometry, typeof(Modules.Geometry.Views.GeometryView).FullName);
        }

        [TestMethod]
        public void TestNavigationParameters()
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(Constants.NavigationParameters.Structure.Source));
            Assert.IsFalse(string.IsNullOrWhiteSpace(Constants.NavigationParameters.Properties.Source));
            Assert.IsFalse(string.IsNullOrWhiteSpace(Constants.NavigationParameters.Attributes.Source));
        }
    }
}
