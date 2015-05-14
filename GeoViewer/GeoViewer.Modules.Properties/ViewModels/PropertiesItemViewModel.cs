using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;

namespace GeoViewer.Modules.Properties.ViewModels
{
    public class PropertiesItemViewModel : BindableBase
    {
        private readonly string name;
        private readonly string type;
        private readonly object value;

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

        public object Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
