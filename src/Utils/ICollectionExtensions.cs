﻿using System.Collections.Generic;

namespace Utils
{
    public static class ICollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, ICollection<T> otherCollection)
        {
            foreach (var other in otherCollection)
                collection.Add(other);
        }
    }
}
