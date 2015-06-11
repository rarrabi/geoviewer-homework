using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeoViewer.Modules.Welcome.Services
{
    /// <summary>
    /// Service for handling the list of recent files using an XML file as persistent storage.
    /// </summary>
    public class XmlRecentFileService : RecentFileService
    {
        private readonly string fileName;

        /// <summary>
        /// Initializes a new instance of the XmlRecentFileService class.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the XML file.</param>
        /// <param name="size">The size of the list of recent files.</param>
        public XmlRecentFileService(string fileName, int size)
            : base(size)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            this.fileName = fileName;
        }

        /// <summary>
        /// Loads the list of recent files from a persistent storage (XML file).
        /// </summary>
        /// <returns>The list of recent files.</returns>
        protected override IReadOnlyList<string> LoadRecentFiles()
        {
            if (!this.IsValid(this.fileName))
            {
                // Return an empty list when the file name is invalid.
                return new List<string>().AsReadOnly();
            }

            var document = XDocument.Load(this.fileName);
            var recentFilesElement = document.Root;
            var recentFiles = this.RecentFilesFromXml(recentFilesElement);
            return recentFiles.ToList().AsReadOnly();
        }

        /// <summary>
        /// Saves the list of recent files to a persistent storage (XML file).
        /// </summary>
        /// <param name="recentFiles">The list of recent files.</param>
        protected override void SaveRecentFiles(IReadOnlyList<string> recentFiles)
        {
            var recentFileElements = this.RecentFilesToXml(recentFiles);
            var recentFilesElement = new XElement("RecentFiles", recentFileElements.ToArray());
            var document = new XDocument(recentFilesElement);
            document.Save(this.fileName);
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

        /// <summary>
        /// Deserializes the list of recent files from XML.
        /// </summary>
        /// <param name="recentFilesElement">The list of recent files as XML.</param>
        /// <returns>The list of recent files.</returns>
        private IEnumerable<string> RecentFilesFromXml(XElement recentFilesElement)
        {
            foreach (var recentFileElement in recentFilesElement.Elements("RecentFile"))
            {
                var fileName = recentFileElement.Attributes("fileName").Single().Value;
                yield return fileName;
            }
        }

        /// <summary>
        /// Serializes the list of recent files to XML.
        /// </summary>
        /// <param name="recentFiles">The list of recent files.</param>
        /// <returns>The list of recent files as XML.</returns>
        private IEnumerable<XElement> RecentFilesToXml(IEnumerable<string> recentFiles)
        {
            foreach (var fileName in recentFiles)
            {
                yield return new XElement("RecentFile", new XAttribute("fileName", fileName));
            }
        }
    }
}
