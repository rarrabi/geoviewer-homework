using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace GeoViewer.Modules.Geometry
{
    public class GeometryModule : IModule
    {
        private readonly ILoggerFacade logger;
        private readonly IUnityContainer container;

        public GeometryModule(ILoggerFacade logger, IUnityContainer container)
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
            this.logger.Log("Initializing GeoViewer.Modules.Geometry.GeometryModule.", Category.Debug, Priority.Low);

            // TODO
            ////this.container.RegisterType<object, Views.GeometryView>(Constants.Navigation.Geometry);
            ////this.container.RegisterType<ViewModels.GeometryViewModel>();
        }
    }
}
