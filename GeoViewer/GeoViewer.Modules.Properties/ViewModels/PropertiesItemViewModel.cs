using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;

namespace GeoViewer.Modules.Properties.ViewModels
{
    /// <summary>
    /// View model of a properties item.
    /// </summary>
    public class PropertiesItemViewModel : BindableBase
    {
        private readonly string name;
        private readonly string type;
        private readonly object value;

        /// <summary>
        /// Initializes a new instance of the PropertiesItemViewModel class with no children.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="type">The data type of the item.</param>
        /// <param name="value">The value of the item.</param>
        public PropertiesItemViewModel(string name, string type, object value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            this.name = name;
            this.type = type;
            this.value = value;
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
        /// Gets the value of the item.
        /// </summary>
        public object Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
