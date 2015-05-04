using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace GeoViewer.Modules.Properties
{
    public class PropertiesModule : IModule
    {
        private readonly ILoggerFacade logger;
        private readonly IUnityContainer container;

        public PropertiesModule(ILoggerFacade logger, IUnityContainer container)
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
            this.logger.Log("Initializing GeoViewer.Modules.Properties.PropertiesModule.", Category.Debug, Priority.Low);

            this.container.RegisterType<object, Views.PropertiesView>(Constants.Navigation.Properties);
            this.container.RegisterType<ViewModels.PropertiesViewModel>();
        }
    }
}
