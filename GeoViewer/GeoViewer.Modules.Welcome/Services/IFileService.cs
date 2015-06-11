using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;

namespace GeoViewer.Modules.Welcome.Services
{
    /// <summary>
    /// Service for handling files.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Gets the filter string that determines what types of files are displayed.
        /// See: https://msdn.microsoft.com/en-us/library/microsoft.win32.filedialog.filter(v=vs.110).aspx
        /// </summary>
        string Filter { get; }

        /// <summary>
        /// Opens a file as a feature set.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        /// <returns>The file as a feature set.</returns>
        IFeatureSet Open(string fileName);
    }
}
