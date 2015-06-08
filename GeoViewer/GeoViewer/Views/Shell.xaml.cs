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
using MahApps.Metro.Controls;

namespace GeoViewer.Views
{
    /// <summary>
    /// Interaction logic for Shell.xaml.
    /// </summary>
    public partial class Shell : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the Shell class.
        /// </summary>
        public Shell()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the Shell class.
        /// </summary>
        /// <param name="viewModel">A view model.</param>
        public Shell(ViewModels.ShellViewModel viewModel)
            : this()
        {
            this.DataContext = viewModel;
        }
    }
}
