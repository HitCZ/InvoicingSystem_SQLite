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

            var listsAreSame = xList.All(xItem => yList.Any(yItem => comparer.Equals(xItem, yItem)));

            return listsAreSame;
        }

        public int GetHashCode(IEnumerable<PropertyInformation> obj) => obj.GetHashCode();
    }
}
