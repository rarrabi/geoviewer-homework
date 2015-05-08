using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GeoViewer.Common;
using GeoViewer.Modules.Welcome.Interactivity;
using GeoViewer.Modules.Welcome.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;

namespace GeoViewer.Modules.Welcome.ViewModels
{
    public class WelcomeViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private readonly IFileService fileService;
        private readonly IRecentFileService recentFileService;
        private readonly DelegateCommand openCommand;
        private readonly DelegateCommand<string> openRecentCommand;
        private readonly InteractionRequest<IOpenFile> openFileInteractionRequest;

        public WelcomeViewModel(IFileService fileService, IRecentFileService recentFileService)
        {
            if (fileService == null)
            {
                throw new ArgumentNullException("fileService");
            }

            if (recentFileService == null)
            {
                throw new ArgumentNullException("recentFileService");
            }

            this.fileService = fileService;
            this.recentFileService = recentFileService;

            this.openCommand = new DelegateCommand(this.OpenCommandExecute);
            this.openRecentCommand = new DelegateCommand<string>(this.OpenRecentCommandExecute);
            this.openFileInteractionRequest = new InteractionRequest<IOpenFile>();
        }

        public ICommand OpenCommand
        {
            get
            {
                return this.openCommand;
            }
        }

        public ICommand OpenRecentCommand
        {
            get
            {
                return this.openRecentCommand;
            }
        }

        public IInteractionRequest OpenFileInteractionRequest
        {
            get
            {
                return this.openFileInteractionRequest;
            }
        }

        public IReadOnlyList<RecentFileViewModel> RecentFiles
        {
            get
            {
                var recentFiles = this.recentFileService.RecentFiles.Select(f => new RecentFileViewModel(f));
                return recentFiles.ToList().AsReadOnly();
            }
        }

        private void OpenCommandExecute()
        {
            this.openFileInteractionRequest.Raise(
                new OpenFile(this.fileService.Filter),
                of =>
                {
                    if (of.Confirmed)
                    {
                        this.Open(of.FileName);
                    }
                });
        }

        private void OpenRecentCommandExecute(string fileName)
        {
            this.Open(fileName);
        }

        private void Open(string fileName)
        {
            this.fileService.Open(fileName);

            this.OnPropertyChanged(() => this.RecentFiles);
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
            // Nothing to do.
        }

        #endregion

        #region IRegionMemberLifetime

        public bool KeepAlive
        {
            get
            {
                // The View and ViewModel should not be kept alive / should be disposed.
                return false;
            }
        }

        #endregion
    }
}
