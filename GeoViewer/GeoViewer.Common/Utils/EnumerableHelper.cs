using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoViewer.Common.Utils
{
    /// <summary>
    /// Contains helper methods for IEnumerable.
    /// </summary>
    public static class EnumerableHelper
    {
        #region Yield
        // See: http://stackoverflow.com/questions/1577822/passing-a-single-item-as-ienumerablet

        /// <summary>
        /// Wraps an object instance into an IEnumerable&lt;T&gt; consisting of a single item.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="value">The instance that will be wrapped.</param>
        /// <returns>An IEnumerable&lt;T&gt; consisting of a single item.</returns>
        public static IEnumerable<T> Yield<T>(T value)
        {
            yield return value;
        }

        #endregion
    }
}
