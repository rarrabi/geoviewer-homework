using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;

namespace GeoViewer.Modules.Structure.ViewModels
{
    public class StructureItemViewModel : BindableBase
    {
        private readonly string name;
        private readonly string type;
        private readonly IReadOnlyList<StructureItemViewModel> children;

        public StructureItemViewModel(string name, string type)
            : this(name, type, Enumerable.Empty<StructureItemViewModel>())
        {
        }

        public StructureItemViewModel(string name, string type, IEnumerable<StructureItemViewModel> children)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (children == null)
            {
                throw new ArgumentNullException("children");
            }

            this.name = name;
            this.type = type;
            this.children = children.ToList().AsReadOnly();
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string Type
        {
            get
            {
                return this.type;
            }
        }

        public IReadOnlyList<StructureItemViewModel> Children
        {
            get
            {
                return this.children;
            }
        }
    }
}
