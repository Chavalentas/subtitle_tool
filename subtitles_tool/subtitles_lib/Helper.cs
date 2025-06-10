using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_lib
{
    public static class Helper
    {
        public static IEnumerable<T> Concat<T>(this List<T[]> sequenceOfEnumerables, T concatElement)
        {
            if (sequenceOfEnumerables == null)
            {
                throw new ArgumentNullException(nameof(sequenceOfEnumerables), "Cannot be null!");
            }

            if (concatElement == null)
            {
                throw new ArgumentNullException(nameof(concatElement), "Cannot be null!");
            }

            for (int index = 0; index < sequenceOfEnumerables.Count; index++)
            {
                foreach (var element in sequenceOfEnumerables[index])
                {
                    yield return element;
                }

                if (index < sequenceOfEnumerables.Count - 1)
                {
                    yield return concatElement;
                }
            }
        }

        public static IEnumerable<T> Concat<T>(this List<T[]> sequenceOfEnumerables)
        {
            if (sequenceOfEnumerables == null)
            {
                throw new ArgumentNullException(nameof(sequenceOfEnumerables), "Cannot be null!");
            }

            for (int index = 0; index < sequenceOfEnumerables.Count; index++)
            {
                foreach (var element in sequenceOfEnumerables[index])
                {
                    yield return element;
                }
            }
        }

        public static List<T[]> Split<T>(this T[] inputArray, T element)
        {
            if (inputArray == null)
            {
                throw new ArgumentNullException(nameof(inputArray), "Cannot be null!");
            }

            if (element == null)
            {
                throw new ArgumentNullException(nameof(element), "Cannot be null!");
            }

            List<T[]> result = new List<T[]>();
            List<T> acc = new List<T>();

            for (int i = 0; i < inputArray.Length; i++)
            {
                if (inputArray[i].Equals(element))
                {
                    if (acc.Count == 0)
                    {
                        continue;
                    }

                    result.Add(acc.ToArray());
                    acc.Clear();
                    continue;
                }

                acc.Add(inputArray[i]);
            }

            if (acc.Count > 0)
            {
                result.Add(acc.ToArray());
            }

            return result;
        }

        public static IEnumerable<int> GetIndexesIfMatch<T>(this IEnumerable<T> inputSequence, Func<T, bool> predicate)
        {
            if (inputSequence == null)
            {
                throw new ArgumentNullException(nameof(inputSequence), "Cannot be null!");
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "Cannot be null!");
            }

            int counter = 0;

            foreach (var el in inputSequence)
            {
                if (predicate(el))
                {
                    yield return counter;
                }

                counter++;
            }
        }

        public static IEnumerable<T> ReplaceAtIndexes<T>(this IEnumerable<T> values, IEnumerable<int> indexes, IEnumerable<T> newIndexValues)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values), "Cannot be null!");
            }

            if (indexes == null)
            {
                throw new ArgumentNullException(nameof(indexes), "Cannot be null!");
            }

            if (newIndexValues == null)
            {
                throw new ArgumentNullException(nameof(newIndexValues), "Cannot be null!");
            }

            if (indexes.Count() != newIndexValues.Count())
            {
                throw new ArgumentException($"Must match the length of {nameof(newIndexValues)}!", nameof(indexes));
            }

            if (indexes.Any(i => !(i >= 0 && i < values.Count())))
            {
                throw new ArgumentException("Indexes must be within valid range!", nameof(indexes));
            }

            if (indexes.Distinct().Count() != indexes.Count())
            {
                throw new ArgumentException("Must contain only distinct values!", nameof(indexes));
            }

            if (newIndexValues.Distinct().Count() != newIndexValues.Count())
            {
                throw new ArgumentException("Must contain only distinct values!", nameof(newIndexValues));
            }

            int counter = 0;
            int indexValueCounter = 0;

            foreach (var value in values)
            {
                if (indexes.Contains(counter))
                {
                    yield return newIndexValues.ElementAt(indexValueCounter);
                    indexValueCounter++;
                }
                else
                {
                    yield return values.ElementAt(counter);
                }

                counter++;
            }
        }
    }
}
