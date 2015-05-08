using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoViewer.Modules.Welcome.Services
{
    public interface IFileService
    {
        string Filter { get; }

        void Open(string fileName);
    }
}
