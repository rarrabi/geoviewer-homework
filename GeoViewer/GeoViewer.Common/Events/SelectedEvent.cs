using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.PubSubEvents;

namespace GeoViewer.Common.Events
{
    public class SelectedEvent : PubSubEvent<object>
    {
    }
}
