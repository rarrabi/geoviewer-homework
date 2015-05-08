using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace GeoViewer.Modules.Welcome
{
    public class WelcomeModule : IModule
    {
        private readonly ILoggerFacade logger;
        private readonly IUnityContainer container;

        public WelcomeModule(ILoggerFacade logger, IUnityContainer container)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.logger = logger;
            this.container = container;
        }

        public void Initialize()
        {
            this.logger.Log("Initializing GeoViewer.Modules.Welcome.WelcomeModule.", Category.Debug, Priority.Low);

            this.container.RegisterType<Services.IFileService, Services.FileService>();
            this.container.RegisterType<Services.IRecentFileService, Services.XmlRecentFileService>(new InjectionConstructor("GeoViewer.RecentFiles.xml", 10));

            this.container.RegisterType<object, Views.WelcomeView>(Constants.Navigation.Welcome);
            this.container.RegisterType<ViewModels.WelcomeViewModel>();
        }
    }
}
