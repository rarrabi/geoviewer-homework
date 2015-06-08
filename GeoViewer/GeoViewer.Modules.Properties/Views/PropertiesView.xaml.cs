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
using GeoViewer.Modules.Properties.ViewModels;

namespace GeoViewer.Modules.Properties.Views
{
    /// <summary>
    /// Interaction logic for PropertiesView.xaml.
    /// </summary>
    public partial class PropertiesView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the PropertiesView class.
        /// </summary>
        public PropertiesView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the PropertiesView class.
        /// </summary>
        /// <param name="viewModel">A view model.</param>
        public PropertiesView(PropertiesViewModel viewModel)
            : this()
        {
            this.DataContext = viewModel;
        }
    }
}
