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
    /// <summary>
    /// View model for WelcomeView.xaml.
    /// </summary>
    public class WelcomeViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private readonly IRegionManager regionManager;
        private readonly IFileService fileService;
        private readonly IRecentFileService recentFileService;
        private readonly DelegateCommand openCommand;
        private readonly DelegateCommand<string> openRecentCommand;
        private readonly InteractionRequest<IOpenFile> openFileInteractionRequest;

        /// <summary>
        /// Initializes a new instance of the WelcomeViewModel class.
        /// </summary>
        /// <param name="regionManager">A Microsoft.Practices.Prism.Regions.IRegionManager.</param>
        /// <param name="fileService">A GeoViewer.Modules.Welcome.Services.IFileService.</param>
        /// <param name="recentFileService">A GeoViewer.Modules.Welcome.Services.IRecentFileService.</param>
        public WelcomeViewModel(IRegionManager regionManager, IFileService fileService, IRecentFileService recentFileService)
        {
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            if (fileService == null)
            {
                throw new ArgumentNullException("fileService");
            }

            if (recentFileService == null)
            {
                throw new ArgumentNullException("recentFileService");
            }

            this.regionManager = regionManager;
            this.fileService = fileService;
            this.recentFileService = recentFileService;

            this.openCommand = new DelegateCommand(this.OpenCommandExecute);
            this.openRecentCommand = new DelegateCommand<string>(this.OpenRecentCommandExecute);
            this.openFileInteractionRequest = new InteractionRequest<IOpenFile>();
        }

        /// <summary>
        /// Gets a command for opening files.
        /// </summary>
        public ICommand OpenCommand
        {
            get
            {
                return this.openCommand;
            }
        }

        /// <summary>
        /// Gets a command for opening recent files.
        /// </summary>
        public ICommand OpenRecentCommand
        {
            get
            {
                return this.openRecentCommand;
            }
        }

        /// <summary>
        /// Gets an interaction request for browsing / opening files.
        /// </summary>
        public IInteractionRequest OpenFileInteractionRequest
        {
            get
            {
                return this.openFileInteractionRequest;
            }
        }

        /// <summary>
        /// Gets the list of recent files.
        /// </summary>
        public IReadOnlyList<RecentFileViewModel> RecentFiles
        {
            get
            {
                return this.ToRecentFileViewModels(this.recentFileService.RecentFiles).ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// Execution logic when the OpenCommand is invoked.
        /// </summary>
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

        /// <summary>
        /// Execution logic when the OpenRecentCommand is invoked.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        private void OpenRecentCommandExecute(string fileName)
        {
            this.Open(fileName);
        }

        /// <summary>
        /// Opens a file.
        /// </summary>
        /// <param name="fileName">A string containing the full path of the file.</param>
        private void Open(string fileName)
        {
            var featureSet = this.fileService.Open(fileName);

            this.OnPropertyChanged(() => this.RecentFiles);

            // Navigate the Main region to the Attributes view.
            this.regionManager.RequestNavigate(
                Constants.Region.Main,
                Constants.Navigation.Attributes,
                new NavigationParameters()
                {
                    { Constants.NavigationParameters.Attributes.Source, featureSet }
                });

            // TODO WelcomeViewModel#Open
            // Navigate the Main region to the Geometry view.
            //// this.regionManager.RequestNavigate(
            ////     Constants.Region.Main,
            ////     Constants.Navigation.Geometry,
            ////     new NavigationParameters()
            ////     {
            ////         { Constants.NavigationParameters.Geometry.Source, featureSet }
            ////     });

            // Navigate the Left region to the Structure view.
            this.regionManager.RequestNavigate(
                Constants.Region.Left,
                Constants.Navigation.Structure,
                new NavigationParameters()
                 {
                     { Constants.NavigationParameters.Structure.Source, featureSet }
                 });

            // Navigate the Right region to the Properties view.
            this.regionManager.RequestNavigate(
                Constants.Region.Right,
                Constants.Navigation.Properties,
                new NavigationParameters()
                 {
                     { Constants.NavigationParameters.Properties.Source, featureSet }
                 });
        }

        private IEnumerable<RecentFileViewModel> ToRecentFileViewModels(IEnumerable<string> recentFiles)
        {
            return recentFiles.Select(f => new RecentFileViewModel(f));
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
            // Nothing to do.
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
                // The view / view model should not be kept alive / should be disposed.
                return false;
            }
        }

        #endregion
    }
}
