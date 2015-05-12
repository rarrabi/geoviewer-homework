using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;

namespace GeoViewer.Modules.Welcome.ViewModels
{
    public class RecentFileViewModel : BindableBase
    {
        private readonly string fileName;

        public RecentFileViewModel(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            this.fileName = fileName;
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.fileName);
            }
        }
    }
}
