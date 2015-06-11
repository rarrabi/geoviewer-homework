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
    /// <typeparam name="T">The type of the payload.</typeparam>
    public abstract class SelectedEvent<T> : PubSubEvent<T>
    {
        // This should be empty.
    }
}
