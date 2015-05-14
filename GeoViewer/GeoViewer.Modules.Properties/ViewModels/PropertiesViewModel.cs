using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;

namespace GeoViewer.Modules.Properties.ViewModels
{
    public class PropertiesViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private IReadOnlyList<PropertiesItemViewModel> properties;

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

        private IEnumerable<PropertiesItemViewModel> ToPropertiesItemViewModels(object source)
        {
            // TODO PropertiesViewModel#ToPropertiesItemViewModels
            throw new NotImplementedException();
        }

        private PropertiesItemViewModel ToPropertiesItemViewModel(object source)
        {
            // TODO PropertiesViewModel#ToPropertiesItemViewModel
            throw new NotImplementedException();
        }

        #region INavigationAware

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // The View and ViewModel can handle / accepts all navigation requests.
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Nothing to do.
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var source = navigationContext.Parameters[Constants.NavigationParameters.Properties.Source];

            this.Properties = this.ToPropertiesItemViewModels(source).ToList().AsReadOnly();
        }

        #endregion

        #region IRegionMemberLifetime

        public bool KeepAlive
        {
            get
            {
                // The View and ViewModel should be kept alive / should not be disposed.
                return true;
            }
        }

        #endregion
    }
}
