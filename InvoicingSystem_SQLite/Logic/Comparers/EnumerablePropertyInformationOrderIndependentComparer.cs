using InvoicingSystem_SQLite.DataAccess.SQL;
using System.Collections.Generic;
using System.Linq;

namespace InvoicingSystem_SQLite.Logic.Comparers
{
    public class EnumerablePropertyInformationOrderIndependentComparer : IComparer<IEnumerable<PropertyInformation>>
    {
        private readonly PropertyInformationComparer comparer = new PropertyInformationComparer();

        public int Compare(IEnumerable<PropertyInformation> x, IEnumerable<PropertyInformation> y)
        {
            if (x is null && y is null)
                return 0;
            if (x is null ^ y is null)
                return -1;

            var xList = x.ToList();
            var yList = y.ToList();

            if (xList.Count != yList.Count)
                return -1;

            foreach (var xItem in xList)
            {
                var yItem = yList.SingleOrDefault(p => p.ColumnName == xItem.ColumnName);

                if (yItem is null)
                    return -1;

                var result = comparer.Compare(xItem, yItem);

                if (result != 0)
                    return -1;
            }

            return 0;
        }
    }
}
