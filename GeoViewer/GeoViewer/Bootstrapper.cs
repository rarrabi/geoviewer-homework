using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace GeoViewer
{
    /// <summary>
    /// Provides a bootstrapping sequence for the application.
    /// </summary>
    /// <see cref="GeoViewer.App"/>
    public class Bootstrapper : UnityBootstrapper
    {
        /// <summary>
        /// Creates the Microsoft.Practices.Prism.Logging.ILoggerFacade used by the bootstrapper.
        /// </summary>
        /// <returns>A new Microsoft.Practices.Prism.Logging.TraceLogger.</returns>
        protected override ILoggerFacade CreateLogger()
        {
            return new TraceLogger();
        }

        /// <summary>
        /// Configures the Microsoft.Practices.Prism.Modularity.IModuleCatalog used by Prism.
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var welcomeModule = typeof(Modules.Welcome.WelcomeModule);
            this.ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = welcomeModule.Name, ModuleType = welcomeModule.AssemblyQualifiedName });

            var propertiesModule = typeof(Modules.Properties.PropertiesModule);
            this.ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = propertiesModule.Name, ModuleType = propertiesModule.AssemblyQualifiedName });

            var structureModule = typeof(Modules.Structure.StructureModule);
            this.ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = structureModule.Name, ModuleType = structureModule.AssemblyQualifiedName });

            var attributesModule = typeof(Modules.Attributes.AttributesModule);
            this.ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = attributesModule.Name, ModuleType = attributesModule.AssemblyQualifiedName });

            var geometryModule = typeof(Modules.Geometry.GeometryModule);
            this.ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = geometryModule.Name, ModuleType = geometryModule.AssemblyQualifiedName });
        }

        /// <summary>
        /// Configures the Microsoft.Practices.Unity.IUnityContainer.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.Container.RegisterType<IEventAggregator, EventAggregator>();

            this.Container.RegisterType<Views.Shell>();
            this.Container.RegisterType<ViewModels.ShellViewModel>();
        }

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Views.Shell>();
        }

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        /// <summary>
        /// Initializes the modules.
        /// </summary>
        protected override void InitializeModules()
        {
            base.InitializeModules();

            // Navigate the Main region to the Welcome view.
            var regionManager = this.Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate(Constants.Region.Main, Constants.Navigation.Welcome);
        }
    }
}
