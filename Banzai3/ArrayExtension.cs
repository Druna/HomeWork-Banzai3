using System.Collections.Generic;

namespace Banzai3
{
    public static class ArrayExtension
    {
        public static void Fill<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        public static T[] NewFill<T>(int len, T value)
        {
            var array = new T[len];
            array.Fill(value);
            return array;
        }

        public static IEnumerable<T> AsEnumerableDimension1<T>(this T[,] array, int index)
        {
            const int dimension = 1;
            int upperBound = array.GetUpperBound(dimension);
            for (int i = array.GetLowerBound(dimension); i <= upperBound; i++)
            {
                yield return array[index, i];
            }
        }

        public static IEnumerable<T> AsEnumerableDimension0<T>(this T[,] array, int index)
        {
            const int dimension = 0;
            int upperBound = array.GetUpperBound(dimension);
            for (int i = array.GetLowerBound(dimension); i <= upperBound; i++)
            {
                yield return array[i, index];
            }
        }
    }
}
