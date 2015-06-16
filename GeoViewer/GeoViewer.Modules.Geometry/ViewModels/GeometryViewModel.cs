using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;

namespace GeoViewer.Modules.Geometry.ViewModels
{
    /// <summary>
    /// View model for GeometryView.xaml.
    /// </summary>
    public class GeometryViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        /// <summary>
        /// The feature set containing the geometries.
        /// </summary>
        private IFeatureSet featureSet;

        /// <summary>
        /// Gets the feature set containing the geometries.
        /// </summary>
        public IFeatureSet FeatureSet
        {
            get
            {
                return this.featureSet;
            }

            private set
            {
                this.SetProperty(ref this.featureSet, value);
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
            var source = navigationContext.Parameters[Constants.NavigationParameters.Geometry.Source];

            if (source is IFeatureSet)
            {
                var featureSet = (IFeatureSet)source;
                this.FeatureSet = featureSet;
            }
            else
            {
                throw new ArgumentOutOfRangeException("navigationContext", navigationContext, string.Format("Invalid navigation parameter: {0} = {1}", Constants.NavigationParameters.Geometry.Source, source));
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
