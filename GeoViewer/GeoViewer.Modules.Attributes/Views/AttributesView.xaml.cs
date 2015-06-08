using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GeoViewer.Modules.Attributes.ViewModels;

namespace GeoViewer.Modules.Attributes.Views
{
    /// <summary>
    /// Interaction logic for AttributesView.xaml.
    /// </summary>
    public partial class AttributesView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the AttributesView class.
        /// </summary>
        public AttributesView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the AttributesView class.
        /// </summary>
        /// <param name="viewModel">A view model.</param>
        public AttributesView(AttributesViewModel viewModel)
            : this()
        {
            this.DataContext = viewModel;
        }
    }
}
