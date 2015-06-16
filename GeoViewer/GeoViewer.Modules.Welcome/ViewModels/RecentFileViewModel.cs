using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;

namespace GeoViewer.Modules.Welcome.ViewModels
{
    /// <summary>
    /// View model of a recent file.
    /// </summary>
    public class RecentFileViewModel : BindableBase
    {
        /// <summary>
        /// The string containing the full path of the file.
        /// </summary>
        private readonly string fileName;

        /// <summary>
        /// Initializes a new instance of the RecentFileViewModel class.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        public RecentFileViewModel(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            this.fileName = fileName;
        }

        /// <summary>
        /// Gets a string containing the full path of the file.
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        /// <summary>
        /// Gets a string containing the name of the file to be display on the UI.
        /// </summary>
        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.fileName);
            }
        }
    }
}
