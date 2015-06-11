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
        private readonly FeatureSelectedEvent featureSelectedEvent;

        private IDictionary<IFeature, StructureItemViewModel> items;
        private IDictionary<StructureItemViewModel, IFeature> sources;

        private StructureItemViewModel root;
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

            this.featureSelectedEvent = eventAggregator.GetEvent<FeatureSelectedEvent>();
            this.featureSelectedEvent.Subscribe(this.OnFeatureSelectedEvent);
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
                    this.OnPropertyChanged(() => this.Roots);
                }
            }
        }

        /// <summary>
        /// Gets the root structure item as an IEnumerable&lt;StructureItemViewModel&gt;.
        /// </summary>
        /// <remarks>
        /// Used for data binding to an ItemsControl.
        /// </remarks>
        public IEnumerable<StructureItemViewModel> Roots
        {
            get
            {
                return EnumerableHelper.Yield(this.root);
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
                        // Publish a feature selection event when the selected item is updated to a non-null item.
                        var source = (IFeature)null;
                        if (this.sources.TryGetValue(this.selected, out source))
                        {
                            this.featureSelectedEvent.Publish(source);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Execution logic when the FeatureSelectedEvent is published.
        /// </summary>
        /// <param name="feature">A feature.</param>
        private void OnFeatureSelectedEvent(IFeature feature)
        {
            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            var item = (StructureItemViewModel)null;
            if (this.items.TryGetValue(feature, out item))
            {
                // Avoid publishing a feature selection event recursively.
                this.SetProperty(ref this.selected, item, PropertySupport.ExtractPropertyName(() => this.Selected));
            }
        }

        /// <summary>
        /// Converts a feature set to a structure item hierarchy.
        /// </summary>
        /// <param name="featureSet">A feature set.</param>
        /// <returns>The feature set converted to a structure item hierarchy.</returns>
        private StructureItemViewModel ToStructureItemViewModel(IFeatureSet featureSet)
        {
            var columnItems = featureSet.DataTable.Columns.Cast<DataColumn>().Select(this.ToStructureItemViewModel).ToList();
            var attributesItem = new StructureItemViewModel("Attributes", featureSet.DataTable.GetType().Name, columnItems);

            var featureItems = featureSet.Features.Select(this.ToStructureItemViewModel).ToList();
            var featuresItem = new StructureItemViewModel("Features", string.Format("[{0}]", featureSet.FeatureType), featureItems);

            var item = new StructureItemViewModel(featureSet.Name, featureSet.GetType().Name, new List<StructureItemViewModel>() { attributesItem, featuresItem });

            return item;
        }

        /// <summary>
        /// Converts a data column to a structure item.
        /// </summary>
        /// <param name="dataColumn">A data column.</param>
        /// <returns>The data column converted to a structure item.</returns>
        private StructureItemViewModel ToStructureItemViewModel(DataColumn dataColumn)
        {
            return new StructureItemViewModel(dataColumn.ColumnName, dataColumn.DataType.Name);
        }

        /// <summary>
        /// Converts a feature to a structure item.
        /// </summary>
        /// <param name="feature">A feature.</param>
        /// <returns>The feature converted to a structure item.</returns>
        private StructureItemViewModel ToStructureItemViewModel(IFeature feature)
        {
            var item = new StructureItemViewModel(feature.Fid.ToString(), feature.FeatureType.ToString());

            this.items[feature] = item;
            this.sources[item] = feature;

            return item;
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

            if (source is IFeatureSet)
            {
                var featureSet = (IFeatureSet)source;
                this.items = new Dictionary<IFeature, StructureItemViewModel>();
                this.sources = new Dictionary<StructureItemViewModel, IFeature>();
                this.Root = this.ToStructureItemViewModel(featureSet);
            }
            else
            {
                throw new ArgumentOutOfRangeException("navigationContext", navigationContext, string.Format("Invalid navigation parameter: {0} = {1}", Constants.NavigationParameters.Structure.Source, source));
            }
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
