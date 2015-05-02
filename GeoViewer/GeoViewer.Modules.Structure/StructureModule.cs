using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace GeoViewer.Modules.Structure
{
    public class StructureModule : IModule
    {
        private readonly ILoggerFacade logger;
        private readonly IUnityContainer container;

        public StructureModule(ILoggerFacade logger, IUnityContainer container)
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
            this.logger.Log("Initializing GeoViewer.Modules.Structure.StructureModule.", Category.Debug, Priority.Low);

            // TODO
            ////this.container.RegisterType<object, Views.StructureView>(Constants.Navigation.Structure);
            ////this.container.RegisterType<ViewModels.StructureViewModel>();
        }
    }
}
