using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoViewer.Modules.Welcome.Services
{
    /// <summary>
    /// Service for handling the list of recent files.
    /// </summary>
    public interface IRecentFileService
    {
        /// <summary>
        /// Gets the list of recent files.
        /// </summary>
        IReadOnlyList<string> RecentFiles { get; }

        /// <summary>
        /// Adds a file to the list of recent files.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        void Add(string fileName);

        /// <summary>
        /// Removes a file from the list of recent files.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        void Remove(string fileName);
    }
}
