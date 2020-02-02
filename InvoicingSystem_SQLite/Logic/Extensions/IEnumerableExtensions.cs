using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvoicingSystem_SQLite.Logic.Extensions
{
    public static class EnumerableExtensions
    {
        public static string JoinToStrings(this IEnumerable<object> collection, string separator = ", ")
        {
            var builder = new StringBuilder();
            var list = collection.ToList();

            for (var i = 0; i < list.Count; i++)
            {
                var currentItem = list[i];

                if (currentItem is null)
                    continue;

                builder.Append(currentItem);

                if (i == list.Count - 1)
                    break;
                builder.Append(separator);
            }

            return builder.ToString();
        }
    }
}
