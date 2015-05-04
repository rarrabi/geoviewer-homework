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
    public partial class StructureView : UserControl
    {
        public StructureView()
        {
            this.InitializeComponent();
        }

        public StructureView(StructureViewModel viewModel)
            : this()
        {
            this.DataContext = viewModel;
        }
    }
}
