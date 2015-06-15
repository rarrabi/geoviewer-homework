using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace GeoViewer.Modules.Attributes
{
    /// <summary>
    /// Initialization logic for the Attributes module. 
    /// </summary>
    public class AttributesModule : IModule
    {
        /// <summary>
        /// The Microsoft.Practices.Prism.Logging.ILoggerFacade.
        /// </summary>
        private readonly ILoggerFacade logger;

        /// <summary>
        /// The Microsoft.Practices.Unity.IUnityContainer.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the AttributesModule class.
        /// </summary>
        /// <param name="logger">A Microsoft.Practices.Prism.Logging.ILoggerFacade.</param>
        /// <param name="container">A Microsoft.Practices.Unity.IUnityContainer.</param>
        public AttributesModule(ILoggerFacade logger, IUnityContainer container)
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

        /// <summary>
        /// Initializes the Attributes module.
        /// </summary>
        public void Initialize()
        {
            this.logger.Log("Initializing GeoViewer.Modules.Attributes.AttributesModule.", Category.Debug, Priority.Low);

            this.container.RegisterType<object, Views.AttributesView>(Constants.Navigation.Attributes);
            this.container.RegisterType<ViewModels.AttributesViewModel>();
        }
    }
}
