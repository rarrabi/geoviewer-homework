using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace GeoViewer.Modules.Welcome.Interactivity
{
    public interface IOpenFile : IConfirmation
    {
        string Filter { get; }

        string FileName { get; set; }
    }
}
