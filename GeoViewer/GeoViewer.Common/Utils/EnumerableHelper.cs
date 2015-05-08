using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoViewer.Common.Utils
{
    public static class EnumerableHelper
    {
        public static IEnumerable<T> Yield<T>(T value)
        {
            yield return value;
        }
    }
}
