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
using GeoViewer.Modules.Welcome.ViewModels;

namespace GeoViewer.Modules.Welcome.Views
{
    /// <summary>
    /// Interaction logic for WelcomeView.xaml.
    /// </summary>
    public partial class WelcomeView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the WelcomeView class.
        /// </summary>
        public WelcomeView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the WelcomeView class.
        /// </summary>
        /// <param name="viewModel">A view model.</param>
        public WelcomeView(WelcomeViewModel viewModel)
            : this()
        {
            this.DataContext = viewModel;
        }
    }
}
