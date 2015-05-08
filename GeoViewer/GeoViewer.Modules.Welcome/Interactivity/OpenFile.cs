using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace GeoViewer.Modules.Welcome.Interactivity
{
    public class OpenFile : Confirmation, IOpenFile
    {
        private readonly string filter;

        public OpenFile()
            : this(string.Empty)
        {
        }

        public OpenFile(string filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            this.filter = filter;
        }

        public string FileName { get; set; }

        public string Filter
        {
            get
            {
                return this.filter;
            }
        }
    }
}
