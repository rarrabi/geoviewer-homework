using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoViewer.Modules.Welcome.Services
{
    public interface IRecentFileService
    {
        IReadOnlyList<string> RecentFiles { get; }

        void Add(string fileName);

        void Remove(string fileName);
    }
}
