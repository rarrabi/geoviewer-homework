using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoViewer.Modules.Welcome.Services
{
    public class FileService : IFileService
    {
        private readonly IRecentFileService recentFileService;

        public FileService(IRecentFileService recentFileService)
        {
            if (recentFileService == null)
            {
                throw new ArgumentNullException("recentFileService");
            }

            this.recentFileService = recentFileService;
        }

        public string Filter
        {
            get
            {
                return "Shapefiles (*.shp)|*.shp";
            }
        }

        public void Open(string fileName)
        {
            // Convert the file name to the absolute / fully qualified file name.
            fileName = Path.GetFullPath(fileName);

            // Validate the file name.
            if (!this.IsValid(fileName))
            {
                // Remove invalid files from the list of recent files.
                this.recentFileService.Remove(fileName);
                return;
            }

            // TODO FileService#Open
            throw new NotImplementedException();
            var isSuccess = true;

            // Update the list of recent files.
            if (isSuccess)
            {
                // Add valid files to the list of recent files.
                this.recentFileService.Add(fileName);
            }
            else
            {
                // Remove invalid files from the list of recent files.
                this.recentFileService.Remove(fileName);
            }
        }

        private bool IsValid(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            return fileInfo.Exists && fileInfo.Length > 0;
        }
    }
}
