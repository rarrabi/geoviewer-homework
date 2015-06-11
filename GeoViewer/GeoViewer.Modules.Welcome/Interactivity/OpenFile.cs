using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace GeoViewer.Modules.Welcome.Interactivity
{
    /// <summary>
    /// Represents an interaction request used for browsing files.
    /// </summary>
    /// <see cref="GeoViewer.Modules.Welcome.Interactivity.IOpenFile"/>
    public class OpenFile : Confirmation, IOpenFile
    {
        private readonly string filter;

        /// <summary>
        /// Initializes a new instance of the OpenFile class with an empty filter string.
        /// </summary>
        public OpenFile()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the OpenFile class.
        /// </summary>
        /// <param name="filter">A filter string that determines what types of files are displayed.</param>
        public OpenFile(string filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            this.filter = filter;
        }

        /// <summary>
        /// Gets the filter string that determines what types of files are displayed.
        /// See: https://msdn.microsoft.com/en-us/library/microsoft.win32.filedialog.filter(v=vs.110).aspx
        /// </summary>
        public string Filter
        {
            get
            {
                return this.filter;
            }
        }

        /// <summary>
        /// Gets or sets a string containing the full path of the file selected.
        /// </summary>
        public string FileName { get; set; }
    }
}
