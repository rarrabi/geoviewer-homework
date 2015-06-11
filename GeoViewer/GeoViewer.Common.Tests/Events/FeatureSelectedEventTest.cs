using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoViewer.Common.Events
{
    [TestClass]
    public class FeatureSelectedEventTest
    {
        [TestMethod]
        public void TestBaseType()
        {
            var type = typeof(FeatureSelectedEvent);
            var baseType = type.BaseType; // typeof(SelectedEvent<IFeature>)

            Assert.AreEqual(typeof(SelectedEvent<>), baseType.GetGenericTypeDefinition());
        }

        [TestMethod]
        public void TestPayloadType()
        {
            var type = typeof(FeatureSelectedEvent);
            var baseType = type.BaseType; // typeof(SelectedEvent<IFeature>)
            var payloadType = baseType.GetGenericArguments()[0]; // typeof(IFeature)

            Assert.AreEqual(typeof(IFeature), payloadType);
        }
    }
}
