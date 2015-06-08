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
using GeoViewer.Modules.Structure.ViewModels;

namespace GeoViewer.Modules.Structure.Views
{
    /// <summary>
    /// Interaction logic for StructureView.xaml.
    /// </summary>
    public partial class StructureView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the StructureView class.
        /// </summary>
        public StructureView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the StructureView class.
        /// </summary>
        /// <param name="viewModel">A view model.</param>
        public StructureView(StructureViewModel viewModel)
            : this()
        {
            this.DataContext = viewModel;
        }
    }
}
