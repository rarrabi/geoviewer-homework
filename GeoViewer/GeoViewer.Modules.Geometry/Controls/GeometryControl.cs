using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DotSpatial.Data;

namespace GeoViewer.Modules.Geometry.Controls
{
    /// <summary>
    /// Displays geometries contained in a feature set.
    /// </summary>
    public class GeometryControl : FrameworkElement
    {
        /// <summary>
        /// Identifies the <see cref="FeatureSet" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FeatureSetProperty = DependencyProperty.Register("FeatureSet", typeof(IFeatureSet), typeof(GeometryControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Gets or sets the feature set containing the geometries.
        /// </summary>
        public IFeatureSet FeatureSet
        {
            get
            {
                return (IFeatureSet)this.GetValue(FeatureSetProperty);
            }

            set
            {
                this.SetValue(FeatureSetProperty, value);
            }
        }

        /// <summary>
        /// When overridden in a derived class, participates in rendering operations that are directed by the layout system.
        /// The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            // Brushes
            var geometryBrush = (Brush)this.TryFindResource("AccentColorBrush4");
            var geometryBorderBrush = (Brush)this.TryFindResource("AccentColorBrush");

            // Pens
            var geometryBorderPen = new Pen(geometryBorderBrush, 1.0);

            // Features / Geometries
            if (this.FeatureSet != null)
            {
                var geometry = this.ToGeometry(this.FeatureSet, this.GetTransformFromProjectionToScreen());
                drawingContext.DrawGeometry(geometryBrush, geometryBorderPen, geometry);
            }
        }

        /// <summary>
        /// Gets the transform which projects the feature set to screen space.
        /// </summary>
        /// <returns>The transform which projects the feature set to screen space.</returns>
        private Transform GetTransformFromProjectionToScreen()
        {
            var transform = new TransformGroup();

            // Move and scale into the viewport based on the extent of the feature set and the render size of the screen.
            // The scaling should be proportional.
            transform.Children.Add(new TranslateTransform(-this.FeatureSet.Extent.MinX, -this.FeatureSet.Extent.MinY));
            var scaleX = this.RenderSize.Width / this.FeatureSet.Extent.Width;
            var scaleY = this.RenderSize.Height / this.FeatureSet.Extent.Height;
            var scale = Math.Min(scaleX, scaleY);
            transform.Children.Add(new ScaleTransform(scale, scale));

            // Mirror vertically based on the differences between the coordinate systems of WPF and DotSpatial.
            // The mirroring should be consistent with the previous proportional scaling.
            transform.Children.Add(new ScaleTransform(1.0, -1.0));
            transform.Children.Add(new TranslateTransform(0.0, this.FeatureSet.Extent.Height * scale));

            // Move to center based on the previous proportional scaling.
            var translateX = (this.RenderSize.Width - (this.FeatureSet.Extent.Width * scale)) / 2;
            var translateY = (this.RenderSize.Height - (this.FeatureSet.Extent.Height * scale)) / 2;
            transform.Children.Add(new TranslateTransform(translateX, translateY));

            return transform;
        }

        /// <summary>
        /// Converts a feature set to a System.Windows.Media.Geometry while transforming the coordinates on the fly.
        /// </summary>
        /// <param name="featureSet">A feature set.</param>
        /// <param name="transform">A transform.</param>
        /// <returns>The feature set converted to a System.Windows.Media.Geometry with the coordinates transformed.</returns>
        private System.Windows.Media.Geometry ToGeometry(IFeatureSet featureSet, Transform transform)
        {
            var geometryGroup = new GeometryGroup();
            foreach (var feature in featureSet.Features)
            {
                var geometry = this.ToGeometry(feature, transform);
                geometryGroup.Children.Add(geometry);
            }

            return geometryGroup;
        }

        /// <summary>
        /// Converts a feature to a System.Windows.Media.Geometry while transforming the coordinates on the fly.
        /// </summary>
        /// <param name="feature">A feature.</param>
        /// <param name="transform">A transform.</param>
        /// <returns>The feature converted to a System.Windows.Media.Geometry with the coordinates transformed.</returns>
        private System.Windows.Media.Geometry ToGeometry(IFeature feature, Transform transform)
        {
            if (feature.NumGeometries == 0)
            {
                return System.Windows.Media.Geometry.Empty;
            }
            else if (feature.NumGeometries == 1)
            {
                var geometry = feature.BasicGeometry;
                var geometryGeometry = this.ToGeometry(geometry, transform);
                return geometryGeometry;
            }
            else
            {
                var geometryGroup = new GeometryGroup();
                for (int geometryIndex = 0; geometryIndex < feature.NumGeometries; geometryIndex++)
                {
                    var geometry = feature.GetBasicGeometryN(geometryIndex);
                    var geometryGeometry = this.ToGeometry(geometry, transform);
                    geometryGroup.Children.Add(geometryGeometry);
                }

                return geometryGroup;
            }
        }

        /// <summary>
        /// Converts a DotSpatial.Topology.IBasicGeometry to a System.Windows.Media.Geometry while transforming the coordinates on the fly.
        /// </summary>
        /// <param name="geometry">A DotSpatial.Topology.IBasicGeometry.</param>
        /// <param name="transform">A transform.</param>
        /// <returns>The DotSpatial.Topology.IBasicGeometry converted to a System.Windows.Media.Geometry with the coordinates transformed.</returns>
        private System.Windows.Media.Geometry ToGeometry(DotSpatial.Topology.IBasicGeometry geometry, Transform transform)
        {
            const double PointEllipseSize = 2.5;

            if (geometry.FeatureType == DotSpatial.Topology.FeatureType.Point)
            {
                var point = transform.Transform(new Point(geometry.Coordinates[0].X, geometry.Coordinates[0].Y));
                var ellipseGeometry = new EllipseGeometry(point, PointEllipseSize, PointEllipseSize);
                return ellipseGeometry;
            }
            else if (geometry.FeatureType == DotSpatial.Topology.FeatureType.Line)
            {
                var streamGeometry = new StreamGeometry();
                using (var streamGeometryContext = streamGeometry.Open())
                {
                    var firstPoint = transform.Transform(new Point(geometry.Coordinates[0].X, geometry.Coordinates[0].Y));
                    streamGeometryContext.BeginFigure(firstPoint, false, false);
                    for (int pointIndex = 1; pointIndex < geometry.NumPoints; pointIndex++)
                    {
                        var point = transform.Transform(new Point(geometry.Coordinates[pointIndex].X, geometry.Coordinates[pointIndex].Y));
                        streamGeometryContext.LineTo(point, true, true);
                    }
                }

                return streamGeometry;
            }
            else if (geometry.FeatureType == DotSpatial.Topology.FeatureType.Polygon)
            {
                var streamGeometry = new StreamGeometry();
                using (var streamGeometryContext = streamGeometry.Open())
                {
                    var firstPoint = transform.Transform(new Point(geometry.Coordinates[0].X, geometry.Coordinates[0].Y));
                    streamGeometryContext.BeginFigure(firstPoint, true, true);
                    for (int pointIndex = 1; pointIndex < geometry.NumPoints; pointIndex++)
                    {
                        var point = transform.Transform(new Point(geometry.Coordinates[pointIndex].X, geometry.Coordinates[pointIndex].Y));
                        streamGeometryContext.LineTo(point, true, true);
                    }
                }

                return streamGeometry;
            }
            else if (geometry.FeatureType == DotSpatial.Topology.FeatureType.MultiPoint)
            {
                var geometryGroup = new GeometryGroup();
                for (int pointIndex = 0; pointIndex < geometry.NumPoints; pointIndex++)
                {
                    var point = transform.Transform(new Point(geometry.Coordinates[pointIndex].X, geometry.Coordinates[pointIndex].Y));
                    var ellipseGeometry = new EllipseGeometry(point, PointEllipseSize, PointEllipseSize);
                    geometryGroup.Children.Add(ellipseGeometry);
                }

                return geometryGroup;
            }
            else
            {
                throw new ArgumentOutOfRangeException(string.Format("Unsupported feature type: '{0}'\n{1}", geometry.FeatureType, geometry));
            }
        }
    }
}
