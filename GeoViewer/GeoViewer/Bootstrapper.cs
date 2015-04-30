using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GeoViewer.Common;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace GeoViewer
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override ILoggerFacade CreateLogger()
        {
            return new TraceLogger();
        }

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

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.Container.RegisterType<Views.Shell>();
            this.Container.RegisterType<ViewModels.ShellViewModel>();
        }

        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Views.Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            var regionManager = this.Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate(Constants.Region.Main, Constants.Navigation.Welcome);
        }
    }
}
