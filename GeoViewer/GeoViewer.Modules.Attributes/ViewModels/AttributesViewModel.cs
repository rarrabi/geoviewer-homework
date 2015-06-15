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

namespace GeoViewer.Modules.Attributes.ViewModels
{
    /// <summary>
    /// View model for AttributesView.xaml.
    /// </summary>
    public class AttributesViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        /// <summary>
        /// The data view containing the attributes.
        /// </summary>
        private DataView attributes;

        /// <summary>
        /// Gets the data view containing the attributes.
        /// </summary>
        public DataView Attributes
        {
            get
            {
                return this.attributes;
            }

            private set
            {
                this.SetProperty(ref this.attributes, value);
            }
        }

        /// <summary>
        /// Converts a feature set to a data view.
        /// </summary>
        /// <param name="featureSet">A feature set.</param>
        /// <returns>The feature set converted to a data view.</returns>
        private DataView ToDataView(IFeatureSet featureSet)
        {
            return featureSet.DataTable.DefaultView;
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
            var source = navigationContext.Parameters[Constants.NavigationParameters.Attributes.Source];

            if (source is IFeatureSet)
            {
                var featureSet = (IFeatureSet)source;
                this.Attributes = this.ToDataView(featureSet);
            }
            else
            {
                throw new ArgumentOutOfRangeException("navigationContext", navigationContext, string.Format("Invalid navigation parameter: {0} = {1}", Constants.NavigationParameters.Attributes.Source, source));
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
