using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;
using GeoViewer.Common.Utils;

namespace GeoViewer.Modules.Structure.Behaviors
{
    /// <summary>
    /// Behavior that makes the <see cref="TreeView.SelectedItem" /> dependency property databindable.
    /// </summary>
    /// <remarks>
    /// The <see cref="TreeView.SelectedItem" /> dependency property is read-only.
    /// See: http://stackoverflow.com/questions/11065995/binding-selecteditem-in-a-hierarchicaldatatemplate-applied-wpf-treeview/18700099
    /// </remarks>
    //// TODO BindableSelectedItemBehaviorTest
    public class BindableSelectedItemBehavior : Behavior<TreeView>
    {
        /// <summary>
        /// Identifies the <see cref="SelectedItem" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(BindableSelectedItemBehavior), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedItemPropertyChanged));

        /// <summary>
        /// Gets or sets the selected item of the <see cref="TreeView" /> that this behavior is attached to.
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return this.GetValue(SelectedItemProperty);
            }

            set
            {
                this.SetValue(SelectedItemProperty, value);
            }
        }

        /// <summary>
        /// Called after the behavior is attached to an <see cref="AssociatedObject" />.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the <see cref="AssociatedObject" />.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.SelectedItemChanged += this.TreeViewSelectedItemChanged;
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="AssociatedObject" />, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the <see cref="AssociatedObject" />.
        /// </remarks>
        protected override void OnDetaching()
        {
            this.AssociatedObject.SelectedItemChanged -= this.TreeViewSelectedItemChanged;

            base.OnDetaching();
        }

        /// <summary>
        /// Recursively searches for an element of a certain type in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of element to find.</typeparam>
        /// <param name="parent">The parent element.</param>
        /// <returns>The element of the specified type</returns>
        private static T FindVisualDescendant<T>(Visual parent)
            where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(parent, i) as Visual;
                if (child != null)
                {
                    T element = child as T;
                    if (element != null)
                    {
                        return element;
                    }

                    T descendantElement = FindVisualDescendant<T>(child);
                    if (descendantElement != null)
                    {
                        return descendantElement;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Recursively searches the <see cref="TreeViewItem" />s of a <see cref="TreeView" /> or <see cref="TreeViewItem" /> for an <see cref="TreeViewItem" /> containing an item.
        /// </summary>
        /// <param name="container">A <see cref="TreeView" /> or a <see cref="TreeViewItem" />.</param>
        /// <param name="item">The item to search for.</param>
        /// <returns>The <see cref="TreeViewItem" /> that contains the item.</returns>
        private static TreeViewItem GetTreeViewItem(ItemsControl container, object item)
        {
            var treeViewItem = container as TreeViewItem;
            if (treeViewItem != null && treeViewItem.DataContext == item)
            {
                return treeViewItem;
            }

            // Try to generate the ItemsPresenter and the ItemsPanel by calling ApplyTemplate.
            container.ApplyTemplate();

            var itemsPresenter = container.Template.FindName("ItemsHost", container) as ItemsPresenter;
            if (itemsPresenter == null)
            {
                // The template has not named the ItemsPresenter as "ItemsHost".
                itemsPresenter = FindVisualDescendant<ItemsPresenter>(container);
                if (itemsPresenter == null)
                {
                    container.UpdateLayout();
                    itemsPresenter = FindVisualDescendant<ItemsPresenter>(container);
                }
            }

            // Try to generate the ItemsHost Panel and the ItemsPanel by calling ApplyTemplate.
            itemsPresenter.ApplyTemplate();

            var itemsHostPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as Panel;

#pragma warning disable 168
            // Ensure that the generator for this panel has been created.
            var children = itemsHostPanel.Children;
#pragma warning restore 168

            foreach (var subItem in container.Items)
            {
                var subContainer = container.ItemContainerGenerator.ContainerFromItem(subItem) as TreeViewItem;
                if (subContainer != null)
                {
                    // Search the next level for the object.
                    var descendantTreeViewItem = GetTreeViewItem(subContainer, item);
                    if (descendantTreeViewItem != null)
                    {
                        return descendantTreeViewItem;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// The callback that is invoked when the effective property value of the <see cref="SelectedItem" /> dependency property changes.
        /// </summary>
        /// <param name="d">The <see cref="System.Windows.DependencyObject" /> on which the property has changed value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this property.</param>
        private static void SelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableSelectedItemBehavior)d).SelectedItemChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// The callback that is invoked when the effective property value of the <see cref="SelectedItem" /> dependency property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the <see cref="SelectedItem" /> dependency property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedItem" /> dependency property.</param>
        private void SelectedItemChanged(object oldValue, object newValue)
        {
            var treeViewItem = newValue as TreeViewItem;
            if (treeViewItem == null)
            {
                treeViewItem = GetTreeViewItem(this.AssociatedObject, newValue);
            }

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                treeViewItem.BringIntoView();
            }
        }

        /// <summary>
        /// Occurs when the selected item of the <see cref="TreeView" /> that this behavior is attached to changes.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void TreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.SelectedItem = e.NewValue;
        }
    }
}
