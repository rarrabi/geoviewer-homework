﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;

namespace GeoViewer.Modules.Properties.ViewModels
{
    /// <summary>
    /// View model for PropertiesView.xaml.
    /// </summary>
    public class PropertiesViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private IReadOnlyList<PropertiesItemViewModel> properties;

        /// <summary>
        /// Gets the list of properties items.
        /// </summary>
        public IReadOnlyList<PropertiesItemViewModel> Properties
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
        /// Converts an object to a list of properties items.
        /// </summary>
        /// <param name="source">An object.</param>
        /// <returns>The object converted to a list of properties items.</returns>
        private IEnumerable<PropertiesItemViewModel> ToPropertiesItemViewModels(object source)
        {
            // TODO PropertiesViewModel#ToPropertiesItemViewModels
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts an object to a properties item.
        /// </summary>
        /// <param name="source">An object.</param>
        /// <returns>The object converted to a properties item.</returns>
        private PropertiesItemViewModel ToPropertiesItemViewModel(object source)
        {
            // TODO PropertiesViewModel#ToPropertiesItemViewModel
            throw new NotImplementedException();
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

            this.Properties = this.ToPropertiesItemViewModels(source).ToList().AsReadOnly();
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
