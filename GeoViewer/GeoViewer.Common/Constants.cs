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
            public const string Welcome = "WelcomeView"; // typeof(WelcomeView).FullName
            public const string Properties = "PropertiesView"; // typeof(PropertiesView).FullName
            public const string Structure = "StructureView"; // typeof(StructureView).FullName
            public const string Attributes = "AttributesView"; // typeof(AttributesView).FullName
            public const string Geometry = "GeometryView"; // typeof(GeometryView).FullName
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
