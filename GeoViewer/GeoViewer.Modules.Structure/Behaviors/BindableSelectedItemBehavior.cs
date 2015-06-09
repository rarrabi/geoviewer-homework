using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace GeoViewer.Modules.Structure.Behaviors
{
    /// <summary>
    /// Provides databindable selection state information for a TreeView.
    /// </summary>
    //// TODO BindableSelectedItemBehavior
    //// TODO BindableSelectedItemBehaviorTest
    public class BindableSelectedItemBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(BindableSelectedItemBehavior), new FrameworkPropertyMetadata(null, SelectedItemPropertyChanged));
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(object), typeof(BindableSelectedItemBehavior), new FrameworkPropertyMetadata(null, SelectedValuePropertyChanged));

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return (object)this.GetValue(SelectedItemProperty);
            }

            set
            {
                this.SetValue(SelectedItemProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected value.
        /// </summary>
        public object SelectedValue
        {
            get
            {
                return (TreeViewItem)this.GetValue(SelectedValueProperty);
            }

            set
            {
                this.SetValue(SelectedValueProperty, value);
            }
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            BindingOperations.SetBinding(this, SelectedItemProperty, new Binding(TreeView.SelectedItemProperty.Name) { Source = this.AssociatedObject });
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            BindingOperations.ClearBinding(this, SelectedItemProperty);

            base.OnDetaching();
        }

        /// <summary>
        /// The callback that is invoked when the effective property value of the SelectedItemProperty dependency property changes.
        /// </summary>
        /// <param name="d">The System.Windows.DependencyObject on which the property has changed value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this property.</param>
        private static void SelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableSelectedItemBehavior)d).SelectedItemChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// The callback that is invoked when the effective property value of the SelectedValueProperty dependency property changes.
        /// </summary>
        /// <param name="d">The System.Windows.DependencyObject on which the property has changed value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this property.</param>
        private static void SelectedValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableSelectedItemBehavior)d).SelectedValueChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// The callback that is invoked when the effective property value of the SelectedItemProperty dependency property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the SelectedItemProperty dependency property.</param>
        /// <param name="newValue">The new value of the SelectedItemProperty dependency property.</param>
        private void SelectedItemChanged(object oldValue, object newValue)
        {
            // Clear the binding from the old selected item to the selected value.
            if (oldValue != null)
            {
                // oldValue.IsSelected = false;
                BindingOperations.ClearBinding(this, SelectedValueProperty);
            }

            // Set the binding from the new selected item to the selected value.
            if (newValue != null)
            {
                // newValue.IsSelected = true;
                BindingOperations.SetBinding(this, SelectedValueProperty, new Binding(TreeViewItem.DataContextProperty.Name) { Source = newValue });
            }
        }

        /// <summary>
        /// The callback that is invoked when the effective property value of the SelectedValueProperty dependency property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the SelectedValueProperty dependency property.</param>
        /// <param name="newValue">The new value of the SelectedValueProperty dependency property.</param>
        private void SelectedValueChanged(object oldValue, object newValue)
        {
            // Update the selected item based on the new selected value.
            this.SelectedItem = this.FindItem(newValue);
        }

        /// <summary>
        /// Searches the items (TreeViewItem) of the associated object (TreeView) for an item with a value (DataContext).
        /// </summary>
        /// <param name="value">A value to search for.</param>
        /// <returns>The item with the value.</returns>
        private TreeViewItem FindItem(object value)
        {
            if (value == null)
            {
                // Coalesce nulls: all items should have non-null values.
                return null;
            }

            // Breadth-first search.
            var queue = new Queue<TreeViewItem>(this.AssociatedObject.Items.Cast<TreeViewItem>());
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (item.DataContext == value)
                {
                    return item;
                }

                foreach (var child in item.Items.Cast<TreeViewItem>())
                {
                    queue.Enqueue(child);
                }
            }

            return null;
        }
    }
}
