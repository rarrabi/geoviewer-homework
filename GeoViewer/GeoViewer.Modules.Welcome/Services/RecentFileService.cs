using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common.Utils;

namespace GeoViewer.Modules.Welcome.Services
{
    public abstract class RecentFileService : IRecentFileService
    {
        private readonly int size;

        private IReadOnlyList<string> recentFiles;

        public RecentFileService(int size)
        {
            this.size = size;
        }

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

        public void Add(string fileName)
        {
            this.Update(fileName, true);
        }

        public void Remove(string fileName)
        {
            this.Update(fileName, false);
        }

        protected abstract IReadOnlyList<string> LoadRecentFiles();

        protected abstract void SaveRecentFiles(IReadOnlyList<string> recentFiles);

        private void Update(string fileName, bool isValid)
        {
            var firstRecentFiles = isValid ? EnumerableHelper.Yield(fileName) : Enumerable.Empty<string>();
            var otherRecentFiles = this.RecentFiles.Where(f => f != fileName);
            var recentFiles = firstRecentFiles.Concat(otherRecentFiles).Take(this.size);
            this.RecentFiles = recentFiles.ToList().AsReadOnly();
        }
    }
}
