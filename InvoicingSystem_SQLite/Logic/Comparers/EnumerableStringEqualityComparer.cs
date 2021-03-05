using System.Collections.Generic;
using System.Linq;

namespace InvoicingSystem_SQLite.Logic.Comparers
{
    public class EnumerableStringEqualityComparer : IEqualityComparer<IEnumerable<string>>
    {
        public bool Equals(IEnumerable<string> x, IEnumerable<string> y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null ^ y is null)
                return false;

            var xList = x.ToList();
            var yList = y.ToList();

            if (xList.Count != yList.Count)
                return false;

            for (var i = 0; i < xList.Count; i++)
            {
                var xItem = xList[i];
                var yItem = yList[i];

                if (xItem != yItem)
                    return false;
            }

            return true;
        }

        public int GetHashCode(IEnumerable<string> obj)
        {
            return obj.GetHashCode();
        }
    }
}
