using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoViewer.Common
{
    /// <summary>
    /// Contains constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Contains region names.
        /// </summary>
        public static class Region
        {
            public const string Left = "LeftRegion";
            public const string Main = "MainRegion";
            public const string Right = "RightRegion";
        }

        /// <summary>
        /// Contains navigation URIs.
        /// </summary>
        public static class Navigation
        {
            public const string Welcome = "GeoViewer.Modules.Welcome.Views.WelcomeView"; // typeof(GeoViewer.Modules.Welcome.Views.WelcomeView).FullName;
            public const string Properties = "GeoViewer.Modules.Properties.Views.PropertiesView"; // typeof(GeoViewer.Modules.Properties.Views.PropertiesView).FullName
            public const string Structure = "GeoViewer.Modules.Structure.Views.StructureView"; // typeof(GeoViewer.Modules.Structure.Views.StructureView).FullName
            public const string Attributes = "GeoViewer.Modules.Attributes.Views.AttributesView"; // typeof(GeoViewer.Modules.Attributes.Views.AttributesView).FullName
            public const string Geometry = "GeoViewer.Modules.Geometry.Views.GeometryView"; // typeof(GeoViewer.Modules.Geometry.Views.GeometryView).FullName
        }

        /// <summary>
        /// Contains navigation parameters.
        /// </summary>
        public static class NavigationParameters
        {
            /// <summary>
            /// Contains navigation parameters for the Welcome view / view model.
            /// </summary>
            public static class Welcome
            {
            }

            /// <summary>
            /// Contains navigation parameters for the Properties view / view model.
            /// </summary>
            public static class Properties
            {
                public const string Source = "Source"; // type: object
            }

            /// <summary>
            /// Contains navigation parameters for the Structure view / view model.
            /// </summary>
            public static class Structure
            {
                public const string Source = "Source"; // type: object
            }

            /// <summary>
            /// Contains navigation parameters for the Attributes view / view model.
            /// </summary>
            public static class Attributes
            {
                public const string Source = "Source"; // type: DotSpatial.Data.IFeatureSet
            }

            /// <summary>
            /// Contains navigation parameters for the Geometry view / view model.
            /// </summary>
            public static class Geometry
            {
            }
        }
    }
}
