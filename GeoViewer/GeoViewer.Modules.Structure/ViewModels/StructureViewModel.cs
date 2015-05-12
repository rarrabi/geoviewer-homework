using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common;
using GeoViewer.Common.Events;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;

namespace GeoViewer.Modules.Structure.ViewModels
{
    public class StructureViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private readonly SelectedEvent selectedEvent;

        private IReadOnlyDictionary<object, StructureItemViewModel> items;
        private IReadOnlyDictionary<StructureItemViewModel, object> sources;

        private StructureItemViewModel root;
        private StructureItemViewModel selected;

        public StructureViewModel(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            this.selectedEvent = eventAggregator.GetEvent<SelectedEvent>();
            this.selectedEvent.Subscribe(this.OnSelectedEvent);
        }

        public StructureItemViewModel Root
        {
            get
            {
                return this.root;
            }

            private set
            {
                this.SetProperty(ref this.root, value);
            }
        }

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
                        var source = (object)null;
                        if (this.sources.TryGetValue(this.selected, out source))
                        {
                            this.selectedEvent.Publish(source);
                        }
                    }
                }
            }
        }

        private void OnSelectedEvent(object selected)
        {
            if (selected == null)
            {
                throw new ArgumentNullException("selected");
            }

            var item = (StructureItemViewModel)null;
            if (this.items.TryGetValue(selected, out item))
            {
                this.selected = item;
                this.OnPropertyChanged(() => this.Selected);
            }
        }

        private StructureItemViewModel ToStructureItemViewModel(object source)
        {
            // TODO StructureViewModel#ToStructureItemViewModel
            throw new NotImplementedException();
        }

        #region INavigationAware

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var source = navigationContext.Parameters[Constants.NavigationParameters.Structure.Source];

            return this.root == null || this.sources[this.root] == source;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Nothing to do.
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var source = navigationContext.Parameters[Constants.NavigationParameters.Structure.Source];

            this.Root = this.ToStructureItemViewModel(source);
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
