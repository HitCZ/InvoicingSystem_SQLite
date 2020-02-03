using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvoicingSystem_SQLite.Logic.Extensions
{
    public static class EnumerableExtensions
    {
        public static string JoinToStrings(this IEnumerable<object> collection, string separator = ", ", string surroundWith = null)
        {
            var builder = new StringBuilder();
            var list = collection.ToList();

            for (var i = 0; i < list.Count; i++)
            {
                var currentItem = list[i];

                if (currentItem is null)
                    continue;
                if (!(surroundWith is null))
                    builder.Append(surroundWith);

                builder.Append(currentItem);

                if (!(surroundWith is null))
                    builder.Append(surroundWith);

                if (i == list.Count - 1)
                    break;
                builder.Append(separator);
            }

            return builder.ToString();
        }
    }
}
