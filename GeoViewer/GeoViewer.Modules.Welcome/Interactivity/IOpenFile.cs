using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace GeoViewer.Modules.Welcome.Interactivity
{
    /// <summary>
    /// Represents an interaction request used for browsing / opening files.
    /// </summary>
    public interface IOpenFile : IConfirmation
    {
        /// <summary>
        /// Gets the filter string that determines what types of files are displayed.
        /// See: https://msdn.microsoft.com/en-us/library/microsoft.win32.filedialog.filter(v=vs.110).aspx
        /// </summary>
        string Filter { get; }

        /// <summary>
        /// Gets or sets a string containing the full path of the file selected.
        /// </summary>
        string FileName { get; set; }
    }
}
