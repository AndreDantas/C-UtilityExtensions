using System.Collections.Generic;

namespace UtilityExtensions.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Returns a list with all the items of the two-dimensional array.
        /// </summary>
        /// <typeparam name="T"> The object Type. </typeparam>
        /// <param name="twoDimensionalArray"> </param>
        /// <returns> </returns>
        public static List<T> ToList<T>(this T[,] twoDimensionalArray)
        {
            if (twoDimensionalArray == null)
            {
                return null;
            }

            List<T> result = new List<T>();

            for (int i = 0; i < twoDimensionalArray.GetLength(0); i++)
            {
                for (int j = 0; j < twoDimensionalArray.GetLength(1); j++)
                {
                    result.Add(twoDimensionalArray[i, j]);
                }
            }
            return result;
        }

        /// <summary>
        /// Checks if the (x,y) coordinates is valid in the two-dimensional array.
        /// </summary>
        /// <typeparam name="T"> The object Type. </typeparam>
        /// <param name="twoDimensionalArray"> </param>
        /// <param name="x"> X coordinate. </param>
        /// <param name="y"> Y coordinate </param>
        /// <returns> </returns>
        public static bool ValidCoordinates<T>(this T[,] twoDimensionalArray, int x, int y)
        {
            if (twoDimensionalArray == null)
            {
                return false;
            }

            return (x >= 0 && x < twoDimensionalArray.GetLength(0) &&
                    y >= 0 && y < twoDimensionalArray.GetLength(1));
        }

        /// <summary>
        /// Rotates a two-dimensional array by +90.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="matrix"> </param>
        /// <returns> </returns>
        public static T[,] RotateMatrix90R<T>(this T[,] matrix)
        {
            T[,] ret = new T[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    ret[i, j] = matrix[matrix.GetLength(0) - j - 1, i];
                }
            }

            return ret;
        }

        /// <summary>
        /// Rotates a two-dimensional array by -90.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="matrix"> </param>
        /// <returns> </returns>
        public static T[,] RotateMatrix90L<T>(this T[,] matrix)
        {
            T[,] ret = new T[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    ret[i, j] = matrix[j, matrix.GetLength(1) - i - 1];
                }
            }

            return ret;
        }

        /// <summary>
        /// Rotates a two-dimensional array by 180.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="matrix"> </param>
        /// <returns> </returns>
        public static T[,] Reverse<T>(this T[,] matrix)
        {
            return matrix.RotateMatrix90R().RotateMatrix90R();
        }

        public static bool Check2DArray<T>(this T[,] data, T[,] find, EqualityComparer<T> comparer = null)
        {
            if (data == null || find == null)
            {
                return false;
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            int dataLen = data.Length; // length of the whole data
            int findLen = find.Length; // length of the whole find

            for (int i = 0; i < dataLen; i++) // iterate through data
            {
                int dataX = i % data.GetLength(0); // get current column index
                int dataY = i / data.GetLength(0); // get current row index

                bool okay = true; // declare result placeholder for that check
                for (int j = 0; j < findLen && okay; j++) // iterate through find
                {
                    int findX = j % find.GetLength(1); // current column in find
                    int findY = j / find.GetLength(1); // current row in find

                    int checkedX = findX + dataX; // column index in data to check
                    int checkedY = findY + dataY; // row index in data to check

                    // check if checked index is not going outside of the data boundries
                    if (checkedX >= data.GetLength(0) || checkedY >= data.GetLength(1))
                    {
                        // we are outside of the data boundries set flag to false and break checks
                        // for this data row and column
                        okay = false;
                        break;
                    }

                    // we are still inside of the data boundries so check if values matches
                    okay = comparer.Equals(data[dataY + findY, dataX + findX], find[findY, findX]); // check if it matches
                }
                if (okay) // if all values from both fragments are equal
                {
                    return true; // return true
                }
            }
            return false;
        }
    }
}