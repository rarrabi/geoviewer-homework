using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Win32;

namespace GeoViewer.Modules.Welcome.Interactivity
{
    /// <summary>
    /// Displays a Microsoft.Win32.OpenFileDialog when invoked. 
    /// </summary>
    //// TODO OpenFileActionTest
    public class OpenFileAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// Invokes the action. 
        /// </summary>
        /// <param name="parameter">The parameter to the action.</param>
        protected override void Invoke(object parameter)
        {
            var e = parameter as InteractionRequestedEventArgs;
            if (e == null)
            {
                return;
            }

            var openFile = e.Context as IOpenFile;
            if (openFile == null)
            {
                return;
            }

            var openFileDialog = new OpenFileDialog() { CheckFileExists = true, CheckPathExists = true, Filter = openFile.Filter, ValidateNames = true };
            if (openFileDialog.ShowDialog(this.FindParent<Window>()) == true)
            {
                openFile.Confirmed = true;
                openFile.FileName = openFileDialog.FileName;
            }
            else
            {
                openFile.Confirmed = false;
                openFile.FileName = null;
            }

            e.Callback();
        }

        /// <summary>
        /// Searches the logical and visual tree for a parent of the associated object.
        /// </summary>
        /// <typeparam name="T">The type of the parent to search for.</typeparam>
        /// <returns>The parent of the associated object.</returns>
        private T FindParent<T>()
            where T : class
        {
            var parent = this.AssociatedObject as T;

            if (parent == null)
            {
                var logicalParent = LogicalTreeHelper.GetParent(this.AssociatedObject);
                while (parent == null && logicalParent != null)
                {
                    parent = logicalParent as T;
                    logicalParent = LogicalTreeHelper.GetParent(logicalParent);
                }
            }

            if (parent == null)
            {
                var visualParent = VisualTreeHelper.GetParent(this.AssociatedObject);
                while (parent == null && visualParent != null)
                {
                    parent = visualParent as T;
                    visualParent = VisualTreeHelper.GetParent(visualParent);
                }
            }

            return parent;
        }
    }
}
