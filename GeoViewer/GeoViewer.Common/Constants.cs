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
            /// <summary>
            /// The name of the left region.
            /// </summary>
            /// <remarks>
            /// Views: Structure view.
            /// </remarks>
            public const string Left = "LeftRegion";

            /// <summary>
            /// The name of the main region.
            /// </summary>
            /// <remarks>
            /// Views: Welcome view, Attributes view, Geometry view.
            /// </remarks>
            public const string Main = "MainRegion";

            /// <summary>
            /// The name of the right region.
            /// </summary>
            /// <remarks>
            /// Views: Properties view.
            /// </remarks>
            public const string Right = "RightRegion";
        }

        /// <summary>
        /// Contains navigation URIs.
        /// </summary>
        public static class Navigation
        {
            /// <summary>
            /// The URI of the Welcome view.
            /// </summary>
            /// <remarks>
            /// Value: typeof(GeoViewer.Modules.Welcome.Views.WelcomeView).FullName.
            /// </remarks>
            public const string Welcome = "GeoViewer.Modules.Welcome.Views.WelcomeView";

            /// <summary>
            /// The URI of the Properties view.
            /// </summary>
            /// <remarks>
            /// Value: typeof(GeoViewer.Modules.Properties.Views.PropertiesView).FullName.
            /// </remarks>
            public const string Properties = "GeoViewer.Modules.Properties.Views.PropertiesView";

            /// <summary>
            /// The URI of the Structure view.
            /// </summary>
            /// <remarks>
            /// Value: typeof(GeoViewer.Modules.Structure.Views.StructureView).FullName.
            /// </remarks>
            public const string Structure = "GeoViewer.Modules.Structure.Views.StructureView";

            /// <summary>
            /// The URI of the Attributes view.
            /// </summary>
            /// <remarks>
            /// Value: typeof(GeoViewer.Modules.Attributes.Views.AttributesView).FullName.
            /// </remarks>
            public const string Attributes = "GeoViewer.Modules.Attributes.Views.AttributesView";

            /// <summary>
            /// The URI of the Geometry view.
            /// </summary>
            /// <remarks>
            /// Value: typeof(GeoViewer.Modules.Geometry.Views.GeometryView).FullName.
            /// </remarks>
            public const string Geometry = "GeoViewer.Modules.Geometry.Views.GeometryView";
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
                // This should be empty.
            }

            /// <summary>
            /// Contains navigation parameters for the Properties view / view model.
            /// </summary>
            public static class Properties
            {
                /// <summary>
                /// The source of the properties.
                /// </summary>
                /// <remarks>
                /// Type: DotSpatial.Data.IFeatureSet or DotSpatial.Data.IFeature.
                /// </remarks>
                public const string Source = "Source";
            }

            /// <summary>
            /// Contains navigation parameters for the Structure view / view model.
            /// </summary>
            public static class Structure
            {
                /// <summary>
                /// The source of the structure.
                /// </summary>
                /// <remarks>
                /// Type: DotSpatial.Data.IFeatureSet.
                /// </remarks>
                public const string Source = "Source";
            }

            /// <summary>
            /// Contains navigation parameters for the Attributes view / view model.
            /// </summary>
            public static class Attributes
            {
                /// <summary>
                /// The source of the attributes.
                /// </summary>
                /// <remarks>
                /// Type: DotSpatial.Data.IFeatureSet.
                /// </remarks>
                public const string Source = "Source";
            }

            /// <summary>
            /// Contains navigation parameters for the Geometry view / view model.
            /// </summary>
            public static class Geometry
            {
                // This should be empty.
            }
        }
    }
}
