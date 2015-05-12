using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoViewer.Common
{
    public static class Constants
    {
        public static class Region
        {
            public const string Left = "LeftRegion";
            public const string Main = "MainRegion";
            public const string Right = "RightRegion";
        }

        public static class Navigation
        {
            public const string Welcome = "GeoViewer.Modules.Welcome.Views.WelcomeView"; // typeof(GeoViewer.Modules.Welcome.Views.WelcomeView).FullName;
            public const string Properties = "GeoViewer.Modules.Properties.Views.PropertiesView"; // typeof(GeoViewer.Modules.Properties.Views.PropertiesView).FullName
            public const string Structure = "GeoViewer.Modules.Structure.Views.StructureView"; // typeof(GeoViewer.Modules.Structure.Views.StructureView).FullName
            public const string Attributes = "GeoViewer.Modules.Attributes.Views.AttributesView"; // typeof(GeoViewer.Modules.Attributes.Views.AttributesView).FullName
            public const string Geometry = "GeoViewer.Modules.Geometry.Views.GeometryView"; // typeof(GeoViewer.Modules.Geometry.Views.GeometryView).FullName
        }

        public static class NavigationParameters
        {
            public static class Welcome
            {
            }

            public static class Properties
            {
            }

            public static class Structure
            {
                public const string Source = "Source"; // type: object
            }

            public static class Attributes
            {
            }

            public static class Geometry
            {
            }
        }
    }
}
