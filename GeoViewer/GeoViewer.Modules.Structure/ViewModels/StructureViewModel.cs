using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;
using GeoViewer.Common;
using GeoViewer.Common.Events;
using GeoViewer.Common.Utils;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;

namespace GeoViewer.Modules.Structure.ViewModels
{
    /// <summary>
    /// View model for StructureView.xaml.
    /// </summary>
    public class StructureViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private readonly SelectedEvent selectedEvent;

        private IDictionary<object, StructureItemViewModel> items;
        private IDictionary<StructureItemViewModel, object> sources;

        private StructureItemViewModel root;
        private IEnumerable<StructureItemViewModel> roots;
        private StructureItemViewModel selected;

        /// <summary>
        /// Initializes a new instance of the StructureViewModel class.
        /// </summary>
        /// <param name="eventAggregator">A Microsoft.Practices.Prism.PubSubEvents.IEventAggregator.</param>
        public StructureViewModel(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            this.selectedEvent = eventAggregator.GetEvent<SelectedEvent>();
            this.selectedEvent.Subscribe(this.OnSelectedEvent);
        }

        /// <summary>
        /// Gets the root structure item.
        /// </summary>
        public StructureItemViewModel Root
        {
            get
            {
                return this.root;
            }

            private set
            {
                if (this.SetProperty(ref this.root, value))
                {
                    this.Roots = EnumerableHelper.Yield(this.root);
                }
            }
        }

        /// <summary>
        /// Gets the root structure item as an IEnumerable&lt;StructureItemViewModel&gt;.
        /// </summary>
        public IEnumerable<StructureItemViewModel> Roots
        {
            get
            {
                return this.roots;
            }

            private set
            {
                this.SetProperty(ref this.roots, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected structure item.
        /// </summary>
        public StructureItemViewModel Selected
        {
            get
            {
                return this.selected;
            }

            set
            {
                if (this.SetProperty(ref this.selected, value))
                {
                    if (this.selected != null)
                    {
                        // Publish an object selection event when the selected item is updated to a non-null item.
                        var source = (object)null;
                        if (this.sources.TryGetValue(this.selected, out source))
                        {
                            this.selectedEvent.Publish(source);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Execution logic when the SelectedEvent is published.
        /// </summary>
        /// <param name="selected">The selected object.</param>
        private void OnSelectedEvent(object selected)
        {
            if (selected == null)
            {
                throw new ArgumentNullException("selected");
            }

            var item = (StructureItemViewModel)null;
            if (this.items.TryGetValue(selected, out item))
            {
                // Avoid publishing an object selection event.
                this.selected = item;
                this.OnPropertyChanged(() => this.Selected);
            }
        }

        /// <summary>
        /// Converts an object to a structure item hierarchy.
        /// </summary>
        /// <param name="source">An object.</param>
        /// <returns>The object converted to a structure item hierarchy.</returns>
        private StructureItemViewModel ToStructureItemViewModel(object source)
        {
            var item = (StructureItemViewModel)null;

            if (source is IFeatureSet)
            {
                var featureSet = (IFeatureSet)source;
                var columnItems = featureSet.DataTable.Columns.Cast<DataColumn>().Select(this.ToStructureItemViewModel).ToList();
                item = new StructureItemViewModel("Attributes", featureSet.DataTable.GetType().Name, columnItems);
            }
            else if (source is DataColumn)
            {
                var dataColumn = (DataColumn)source;
                item = new StructureItemViewModel(dataColumn.ColumnName, dataColumn.DataType.Name);
            }

            if (item != null)
            {
                this.items[source] = item;
                this.sources[item] = source;

                return item;
            }

            // TODO StructureViewModel#ToStructureItemViewModel
            throw new ArgumentOutOfRangeException("source", source, "Invalid source");
        }

        #region INavigationAware

        /// <summary>
        /// Called to determine if this instance can handle the navigation request.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>true if this instance accepts the navigation request; otherwise, false.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// Called when the implementer is being navigated away from.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Called when the implementer has been navigated to.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var source = navigationContext.Parameters[Constants.NavigationParameters.Structure.Source];

            this.items = new Dictionary<object, StructureItemViewModel>();
            this.sources = new Dictionary<StructureItemViewModel, object>();

            this.Root = this.ToStructureItemViewModel(source);
        }

        #endregion

        #region IRegionMemberLifetime

        /// <summary>
        /// Gets a value indicating whether this instance should be kept-alive upon deactivation.
        /// </summary>
        public bool KeepAlive
        {
            get
            {
                // The view / view model should be kept alive / should not be disposed.
                return true;
            }
        }

        #endregion
    }
}
