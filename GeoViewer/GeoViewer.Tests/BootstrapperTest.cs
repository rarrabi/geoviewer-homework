using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoViewer
{
    [TestClass]
    public class BootstrapperTest
    {
        [TestMethod]
        public void TestBaseType()
        {
            var type = typeof(Bootstrapper);
            var baseType = type.BaseType; // typeof(UnityBootstrapper)

            Assert.AreEqual(typeof(UnityBootstrapper), baseType);
        }
    }
}
