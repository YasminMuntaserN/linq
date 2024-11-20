using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ.Exercises.data
{
    public static class ExtensionFunctional
    {
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> list, int numberOfItems)
            => list.OrderBy(_ => Guid.NewGuid()).Take(numberOfItems);

    }

}
