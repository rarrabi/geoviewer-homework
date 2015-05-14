using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;

namespace GeoViewer.Modules.Attributes.ViewModels
{
    public class AttributesViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private DataView attributes;

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

        private DataTable ToDataTable(object source)
        {
            // TODO AttributesViewModel#ToDataTable
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
            var source = navigationContext.Parameters[Constants.NavigationParameters.Attributes.Source];

            this.Attributes = this.ToDataTable(source).DefaultView;
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
