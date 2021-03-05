using InvoicingSystem_SQLite.DataAccess.SQL;
using System.Collections.Generic;
using System.Linq;

namespace InvoicingSystem_SQLite.Logic.Comparers
{
    public class EnumerablePropertyInformationEqualityComparer : IEqualityComparer<IEnumerable<PropertyInformation>>
    {
        private readonly PropertyInformationEqualityComparer comparer = new PropertyInformationEqualityComparer();

        public bool Equals(IEnumerable<PropertyInformation> x, IEnumerable<PropertyInformation> y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null ^ y is null)
                return false;

            var xList = x.ToList();
            var yList = y.ToList();

            if (xList.Count != yList.Count)
                return false;

            foreach (var xItem in xList)
            {
                var yItem = yList.SingleOrDefault(p => p.ColumnName == xItem.ColumnName);

                if (yItem is null)
                    return false;

                var result = comparer.Equals(xItem, yItem);

                if (!result)
                    return false;
            }

            return true;
        }

        public int GetHashCode(IEnumerable<PropertyInformation> obj)
        {
            return obj.GetHashCode();
        }
    }
}
