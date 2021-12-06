using System;

namespace Gsemac.Collections {

    public static class ArrayUtilities {

        // Public members

        public static void Shift<T>(T[] array, int offset) {

            if (array is null)
                throw new ArgumentNullException(nameof(array));

            if (Math.Abs(offset) >= array.Length) {

                Array.Clear(array, 0, array.Length);

            }
            else if (offset < 0) {

                offset = Math.Abs(offset);

                Array.Copy(array, offset, array, 0, offset + 1);
                Array.Clear(array, offset + 1, array.Length - offset - 1);

            }
            else if (offset > 0) {

                Array.Copy(array, 0, array, offset, offset + 1);
                Array.Clear(array, 0, offset);

            }

        }

    }

}