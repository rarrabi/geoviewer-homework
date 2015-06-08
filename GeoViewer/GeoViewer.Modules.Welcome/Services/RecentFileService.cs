using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common.Utils;

namespace GeoViewer.Modules.Welcome.Services
{
    /// <summary>
    /// Service for handling the list of recent files.
    /// </summary>
    /// <see cref="GeoViewer.Modules.Welcome.Services.IRecentFileService"/>
    public abstract class RecentFileService : IRecentFileService
    {
        private readonly int size;

        private IReadOnlyList<string> recentFiles;

        /// <summary>
        /// Initializes a new instance of the RecentFileService class.
        /// </summary>
        /// <param name="size">The size of the list of recent files.</param>
        public RecentFileService(int size)
        {
            this.size = size;
        }

        /// <summary>
        /// Gets the list of recent files.
        /// </summary>
        public IReadOnlyList<string> RecentFiles
        {
            get
            {
                if (this.recentFiles == null)
                {
                    this.recentFiles = this.LoadRecentFiles();
                }

                return this.recentFiles;
            }

            private set
            {
                this.SaveRecentFiles(value);

                this.recentFiles = value;
            }
        }

        /// <summary>
        /// Adds a file to the list of recent files.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        public void Add(string fileName)
        {
            this.Update(fileName, true);
        }

        /// <summary>
        /// Removes a file from the list of recent files.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        public void Remove(string fileName)
        {
            this.Update(fileName, false);
        }

        /// <summary>
        /// Loads the list of recent files from a persistent storage.
        /// </summary>
        /// <returns>The list of recent files.</returns>
        protected abstract IReadOnlyList<string> LoadRecentFiles();

        /// <summary>
        /// Saves the list of recent files to a persistent storage.
        /// </summary>
        /// <param name="recentFiles">The list of recent files.</param>
        protected abstract void SaveRecentFiles(IReadOnlyList<string> recentFiles);

        /// <summary>
        /// Updates the list of recent files by adding or removing a file.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        /// <param name="isValid">A value indicating whether the file should be added or removed.</param>
        private void Update(string fileName, bool isValid)
        {
            var firstRecentFiles = isValid ? EnumerableHelper.Yield(fileName) : Enumerable.Empty<string>();
            var otherRecentFiles = this.RecentFiles.Where(f => f != fileName);
            var recentFiles = firstRecentFiles.Concat(otherRecentFiles).Take(this.size);
            this.RecentFiles = recentFiles.ToList().AsReadOnly();
        }
    }
}
