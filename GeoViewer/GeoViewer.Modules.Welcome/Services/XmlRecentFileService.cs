using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeoViewer.Modules.Welcome.Services
{
    public class XmlRecentFileService : RecentFileService
    {
        private readonly string fileName;

        public XmlRecentFileService(string fileName, int size)
            : base(size)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            this.fileName = fileName;
        }

        protected override IReadOnlyList<string> LoadRecentFiles()
        {
            // Validate the file name.
            if (!this.IsValid(this.fileName))
            {
                // Return an empty list when the file is invalid.
                return new List<string>().AsReadOnly();
            }

            var document = XDocument.Load(this.fileName);
            var recentFilesElement = document.Root;
            var recentFiles = this.RecentFilesFromXml(recentFilesElement);
            return recentFiles.ToList().AsReadOnly();
        }

        protected override void SaveRecentFiles(IReadOnlyList<string> recentFiles)
        {
            var recentFileElements = this.RecentFilesToXml(recentFiles);
            var recentFilesElement = new XElement("RecentFiles", recentFileElements.ToArray());
            var document = new XDocument(recentFilesElement);
            document.Save(this.fileName);
        }

        private bool IsValid(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            return fileInfo.Exists && fileInfo.Length > 0;
        }

        private IEnumerable<string> RecentFilesFromXml(XElement recentFilesElement)
        {
            foreach (var recentFileElement in recentFilesElement.Elements("RecentFile"))
            {
                var fileName = recentFileElement.Attributes("fileName").Single().Value;
                yield return fileName;
            }
        }

        private IEnumerable<XElement> RecentFilesToXml(IEnumerable<string> recentFiles)
        {
            foreach (var fileName in recentFiles)
            {
                yield return new XElement("RecentFile", new XAttribute("fileName", fileName));
            }
        }
    }
}
