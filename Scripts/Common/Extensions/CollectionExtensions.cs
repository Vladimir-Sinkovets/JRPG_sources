using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static List<T> GetRandomElements<T>(this IEnumerable<T> source, int count = 1)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (count <= 0)
                return new List<T>();

            var list = source as IList<T> ?? source.ToList();

            if (list.Count == 0)
                return new List<T>();

            count = Mathf.Min(count, list.Count);

            if (count <= 5 || count >= list.Count - 5)
            {
                var tempList = new List<T>(list);
                var selectedItems = new List<T>(count);

                for (int i = 0; i < count; i++)
                {
                    var randomIndex = UnityEngine.Random.Range(0, tempList.Count);
                    selectedItems.Add(tempList[randomIndex]);
                    tempList.RemoveAt(randomIndex);
                }

                return selectedItems;
            }

            var result = new List<T>(list);
            for (int i = 0; i < count; i++)
            {
                var randomIndex = UnityEngine.Random.Range(i, result.Count);
                (result[i], result[randomIndex]) = (result[randomIndex], result[i]);
            }

            return result.Take(count).ToList();
        }
    }
}
