using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.PubSubEvents;

namespace GeoViewer.Common.Events
{
    /// <summary>
    /// Defines a class that manages publication and subscription to object selection events.  
    /// </summary>
    public class SelectedEvent : PubSubEvent<object>
    {
        // This should be empty.
    }
}
