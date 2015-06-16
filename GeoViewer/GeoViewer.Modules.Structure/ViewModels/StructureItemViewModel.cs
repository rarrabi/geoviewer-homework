using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;

namespace GeoViewer.Modules.Structure.ViewModels
{
    /// <summary>
    /// View model of a structure item.
    /// </summary>
    public class StructureItemViewModel : BindableBase
    {
        /// <summary>
        /// The name of the item.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The data type of the item.
        /// </summary>
        private readonly string type;

        /// <summary>
        /// The children of the item.
        /// </summary>
        private readonly IReadOnlyList<StructureItemViewModel> children;

        /// <summary>
        /// Initializes a new instance of the StructureItemViewModel class with no children.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="type">The data type of the item.</param>
        public StructureItemViewModel(string name, string type)
            : this(name, type, Enumerable.Empty<StructureItemViewModel>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the StructureItemViewModel class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="type">The data type of the item.</param>
        /// <param name="children">The children of the item.</param>
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

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets the data type of the item.
        /// </summary>
        public string Type
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        /// Gets the children of the item.
        /// </summary>
        public IReadOnlyList<StructureItemViewModel> Children
        {
            get
            {
                return this.children;
            }
        }
    }
}
