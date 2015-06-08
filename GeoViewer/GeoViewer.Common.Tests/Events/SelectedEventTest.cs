﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoViewer.Common.Events
{
    [TestClass]
    public class SelectedEventTest
    {
        [TestMethod]
        public void TestBaseType()
        {
            var type = typeof(SelectedEvent);
            var baseType = type.BaseType; // typeof(PubSubEvent<T>)

            Assert.AreEqual(typeof(PubSubEvent<>), baseType.GetGenericTypeDefinition());
        }

        [TestMethod]
        public void TestPayloadType()
        {
            var type = typeof(SelectedEvent);
            var baseType = type.BaseType; // typeof(PubSubEvent<T>)
            var payloadType = baseType.GetGenericArguments()[0]; // typeof(T)

            Assert.AreEqual(typeof(object), payloadType);
        }
    }
}
