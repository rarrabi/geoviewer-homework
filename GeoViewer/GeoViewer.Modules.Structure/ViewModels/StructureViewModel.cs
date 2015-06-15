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
        /// <summary>
        /// The Microsoft.Practices.Prism.Regions.IRegionManager.
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// The event used for publishing feature selection based on the selected structure item.
        /// </summary>
        private readonly FeatureSelectedEvent featureSelectedEvent;

        /// <summary>
        /// The source of the structure item hierarchy.
        /// </summary>
        private IFeatureSet source;

        /// <summary>
        /// The mapping between features and structure items.
        /// </summary>
        private IDictionary<IFeature, StructureItemViewModel> featureItems;

        /// <summary>
        /// The mapping between  structure items and features.
        /// </summary>
        private IDictionary<StructureItemViewModel, IFeature> featureSources;

        /// <summary>
        /// The root of the structure item hierarchy.
        /// </summary>
        private StructureItemViewModel root;

        /// <summary>
        /// The selected structure item.
        /// </summary>
        private StructureItemViewModel selected;

        /// <summary>
        /// Initializes a new instance of the StructureViewModel class.
        /// </summary>
        /// <param name="regionManager">A Microsoft.Practices.Prism.Regions.IRegionManager.</param>
        /// <param name="eventAggregator">A Microsoft.Practices.Prism.PubSubEvents.IEventAggregator.</param>
        public StructureViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            this.regionManager = regionManager;

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
                if (this.SetSelected(value))
                {
                    // Publish a feature selection event when the selected item is changed.
                    this.PublishSelected();
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
            if (this.featureItems.TryGetValue(feature, out item))
            {
                this.SetSelected(item);

                // Avoid publishing a feature selection event recursively.
                //// this.PublishSelected();
            }
        }

        /// <summary>
        /// Publishes a feature selection event based on the selected structure item.
        /// </summary>
        /// <remarks>
        /// Only publishes a feature selection event when the selected structure item is a feature.
        /// </remarks>
        private void PublishSelected()
        {
            if (this.selected == null)
            {
                // Avoid publishing a feature selection event when nothing is selected.
                return;
            }

            var feature = (IFeature)null;
            if (this.featureSources.TryGetValue(this.selected, out feature))
            {
                this.featureSelectedEvent.Publish(feature);
            }
        }

        /// <summary>
        /// Sets the the selected structure item.
        /// </summary>
        /// <param name="value">A structure item.</param>
        /// <returns>A value indicating whether the value was changed.</returns>
        private bool SetSelected(StructureItemViewModel value)
        {
            var changed = this.SetProperty(ref this.selected, value, PropertySupport.ExtractPropertyName(() => this.Selected));
            if (changed)
            {
                var source = (object)this.source;

                // Check whether the source of the selected structure item is a feature.
                if (this.selected != null)
                {
                    var feature = (IFeature)null;
                    if (this.featureSources.TryGetValue(this.selected, out feature))
                    {
                        source = feature;
                    }
                }

                if (source != null)
                {
                    // Navigate the Right region to the Properties view.
                    this.regionManager.RequestNavigate(
                        Constants.Region.Right,
                        Constants.Navigation.Properties,
                        new NavigationParameters()
                        {
                            { Constants.NavigationParameters.Properties.Source, source }
                        });
                }
            }

            return changed;
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

            // Store the mapping betwen the feature and the structure item.
            this.featureItems[feature] = item;
            this.featureSources[item] = feature;

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
                this.source = featureSet;
                this.featureItems = new Dictionary<IFeature, StructureItemViewModel>();
                this.featureSources = new Dictionary<StructureItemViewModel, IFeature>();
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
