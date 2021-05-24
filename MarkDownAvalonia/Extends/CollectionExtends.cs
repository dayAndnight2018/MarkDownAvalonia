using System.Collections.Generic;

namespace MarkDownAvalonia.Extends
{
    public static class CollectionExtends
    {
        public static void TryRemove<T>(this List<T> list, T item)
        {
            if (item != null && list.Contains(item))
            {
                list.Remove(item);
            }
        }
    }
}