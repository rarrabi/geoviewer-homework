using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using SCG = System.Collections.Generic;

namespace GeoViewer.Modules.Properties.ViewModels
{
    /// <summary>
    /// View model for PropertiesView.xaml.
    /// </summary>
    public class PropertiesViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        /// <summary>
        /// The list of properties items.
        /// </summary>
        private SCG.IReadOnlyList<PropertiesItemViewModel> properties;

        /// <summary>
        /// Gets the list of properties items.
        /// </summary>
        public SCG.IReadOnlyList<PropertiesItemViewModel> Properties
        {
            get
            {
                return this.properties;
            }

            private set
            {
                this.SetProperty(ref this.properties, value);
            }
        }

        /// <summary>
        /// Converts a feature set to a list of properties items.
        /// </summary>
        /// <param name="featureSet">A feature set.</param>
        /// <returns>The feature set converted to a list of properties items.</returns>
        private IEnumerable<PropertiesItemViewModel> ToPropertiesItemViewModels(IFeatureSet featureSet)
        {
            yield return new PropertiesItemViewModel("Name", featureSet.Name.GetType().Name, featureSet.Name);
            yield return new PropertiesItemViewModel("Feature Type", featureSet.FeatureType.GetType().Name, featureSet.FeatureType);
            yield return new PropertiesItemViewModel("Feature Count", featureSet.Features.Count.GetType().Name, featureSet.Features.Count);
            yield return new PropertiesItemViewModel("Projection", featureSet.Projection.GetType().Name, featureSet.Projection);
            yield return new PropertiesItemViewModel("Width", featureSet.Extent.Width.GetType().Name, featureSet.Extent.Width);
            yield return new PropertiesItemViewModel("Height", featureSet.Extent.Height.GetType().Name, featureSet.Extent.Height);
            yield return new PropertiesItemViewModel("MinX", featureSet.Extent.MinX.GetType().Name, featureSet.Extent.MinX);
            yield return new PropertiesItemViewModel("MaxX", featureSet.Extent.MaxX.GetType().Name, featureSet.Extent.MaxX);
            yield return new PropertiesItemViewModel("MinY", featureSet.Extent.MinY.GetType().Name, featureSet.Extent.MinY);
            yield return new PropertiesItemViewModel("MaxY", featureSet.Extent.MaxY.GetType().Name, featureSet.Extent.MaxY);
        }

        /// <summary>
        /// Converts a feature to a list of properties items.
        /// </summary>
        /// <param name="feature">A feature.</param>
        /// <returns>The feature converted to a list of properties items.</returns>
        private IEnumerable<PropertiesItemViewModel> ToPropertiesItemViewModels(IFeature feature)
        {
            yield return new PropertiesItemViewModel("ID", "ID", feature.Fid);

            foreach (var dataColumn in feature.DataRow.Table.Columns.Cast<DataColumn>())
            {
                yield return new PropertiesItemViewModel(dataColumn.ColumnName, dataColumn.DataType.Name, feature.DataRow[dataColumn]);
            }
        }

        #region INavigationAware

        /// <summary>
        /// Called to determine if this instance can handle the navigation request.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>true if this instance accepts the navigation request; otherwise, false.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // The view / view model can handle / accepts all navigation requests.
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
            var source = navigationContext.Parameters[Constants.NavigationParameters.Properties.Source];

            if (source is IFeatureSet)
            {
                var featureSet = (IFeatureSet)source;
                this.Properties = this.ToPropertiesItemViewModels(featureSet).ToList().AsReadOnly();
            }
            else if (source is IFeature)
            {
                var feature = (IFeature)source;
                this.Properties = this.ToPropertiesItemViewModels(feature).ToList().AsReadOnly();
            }
            else
            {
                throw new ArgumentOutOfRangeException("navigationContext", navigationContext, string.Format("Invalid navigation parameter: {0} = {1}", Constants.NavigationParameters.Properties.Source, source));
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
