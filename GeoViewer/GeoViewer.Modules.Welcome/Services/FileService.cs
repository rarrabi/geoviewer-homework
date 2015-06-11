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
    /// <see cref="GeoViewer.Modules.Welcome.Services.IFileService"/>
    public class FileService : IFileService
    {
        private readonly IRecentFileService recentFileService;

        /// <summary>
        /// Initializes a new instance of the FileService class.
        /// </summary>
        /// <param name="recentFileService">A GeoViewer.Modules.Welcome.Services.IRecentFileService.</param>
        public FileService(IRecentFileService recentFileService)
        {
            if (recentFileService == null)
            {
                throw new ArgumentNullException("recentFileService");
            }

            this.recentFileService = recentFileService;
        }

        /// <summary>
        /// Gets the filter string that determines what types of files are displayed.
        /// See: https://msdn.microsoft.com/en-us/library/microsoft.win32.filedialog.filter(v=vs.110).aspx
        /// </summary>
        public string Filter
        {
            get
            {
                return "Shapefiles (*.shp)|*.shp";
            }
        }

        /// <summary>
        /// Opens a file as a feature set.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        /// <returns>The file as a feature set.</returns>
        public IFeatureSet Open(string fileName)
        {
            // Convert the file name to an absolute / fully qualified file name.
            fileName = Path.GetFullPath(fileName);

            if (!this.IsValid(fileName))
            {
                // Remove invalid files from the list of recent files.
                this.recentFileService.Remove(fileName);

                throw new ArgumentException(string.Format("Invalid file name: {0}", fileName), "fileName");
            }

            try
            {
                var featureSet = this.DoOpen(fileName);

                // Add valid files to the list of recent files.
                this.recentFileService.Add(fileName);

                return featureSet;
            }
            catch (Exception e)
            {
                // Remove invalid files from the list of recent files.
                this.recentFileService.Remove(fileName);

                throw new ArgumentException(string.Format("Failed to open file: {0}", fileName), "fileName", e);
            }
        }

        /// <summary>
        /// Opens a file as a feature set.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        /// <returns>The file as a feature set.</returns>
        private IFeatureSet DoOpen(string fileName)
        {
            var featureSet = FeatureSet.OpenFile(fileName);
            featureSet.FillAttributes();
            return featureSet;
        }

        /// <summary>
        /// Determines whether a file name is valid.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        /// <returns>A value indicating whether the file name is valid.</returns>
        private bool IsValid(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            return fileInfo.Exists && fileInfo.Length > 0;
        }
    }
}
