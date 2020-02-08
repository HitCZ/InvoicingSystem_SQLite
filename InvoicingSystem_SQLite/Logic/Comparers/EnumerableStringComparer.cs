using System.Collections.Generic;
using System.Linq;

namespace InvoicingSystem_SQLite.Logic.Comparers
{
    public  class EnumerableStringComparer : IComparer<IEnumerable<string>>
    {
        public int Compare(IEnumerable<string> x, IEnumerable<string> y)
        {
            if (x is null && y is null)
                return 0;
            if (x is null ^ y is null)
                return -1;

            var xList = x.ToList();
            var yList = y.ToList();

            if (xList.Count != yList.Count)
                return -1;

            for (var i = 0; i < xList.Count; i++)
            {
                var xItem = xList[i];
                var yItem = yList[i];

                if (xItem != yItem)
                    return -1;
            }

            return 0;
        }
    }
}
