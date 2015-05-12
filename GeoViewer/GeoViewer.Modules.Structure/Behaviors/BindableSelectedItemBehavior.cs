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
    // TODO BindableSelectedItemBehaviorTest
    public class BindableSelectedItemBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(TreeViewItem), typeof(BindableSelectedItemBehavior), new FrameworkPropertyMetadata(null, SelectedItemPropertyChanged));
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(object), typeof(BindableSelectedItemBehavior), new FrameworkPropertyMetadata(null, SelectedValuePropertyChanged));

        public TreeViewItem SelectedItem
        {
            get
            {
                return (TreeViewItem)this.GetValue(SelectedItemProperty);
            }

            set
            {
                this.SetValue(SelectedItemProperty, value);
            }
        }

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

        protected override void OnAttached()
        {
            base.OnAttached();

            BindingOperations.SetBinding(this, SelectedItemProperty, new Binding(TreeView.SelectedItemProperty.Name) { Source = this.AssociatedObject });
        }

        protected override void OnDetaching()
        {
            BindingOperations.ClearBinding(this, SelectedItemProperty);

            base.OnDetaching();
        }

        private static void SelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableSelectedItemBehavior)d).SelectedItemChanged((TreeViewItem)e.OldValue, (TreeViewItem)e.NewValue);
        }

        private static void SelectedValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableSelectedItemBehavior)d).SelectedValueChanged(e.OldValue, e.NewValue);
        }

        private void SelectedItemChanged(TreeViewItem oldValue, TreeViewItem newValue)
        {
            if (oldValue != null)
            {
                // oldValue.IsSelected = false;
                BindingOperations.ClearBinding(this, SelectedValueProperty);
            }

            if (newValue != null)
            {
                // newValue.IsSelected = true;
                BindingOperations.SetBinding(this, SelectedValueProperty, new Binding(TreeViewItem.DataContextProperty.Name) { Source = newValue });
            }
        }

        private void SelectedValueChanged(object oldValue, object newValue)
        {
            this.SelectedItem = this.FindItem(newValue);
        }

        private TreeViewItem FindItem(object value)
        {
            if (value == null)
            {
                return null;
            }

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
