using System;

namespace Gsemac.Collections {

    public static class ArrayUtilities {

        // Public members

        public static void Rotate<T>(T[] array, int offset) {

            if (array is null)
                throw new ArgumentNullException(nameof(array));

            offset %= array.Length;

            if (offset == 0)
                return;

            T[] buffer = new T[Math.Abs(offset)];

            if (offset < 0) {

                Array.Copy(array, 0, buffer, 0, Math.Abs(offset));

                Shift(array, offset);

                Array.Copy(buffer, 0, array, array.Length - Math.Abs(offset), buffer.Length);

            }
            else if (offset > 0) {

                Array.Copy(array, array.Length - offset, buffer, 0, offset);

                Shift(array, offset);

                Array.Copy(buffer, array, buffer.Length);

            }

        }
        public static void Shift<T>(T[] array, int offset) {

            if (array is null)
                throw new ArgumentNullException(nameof(array));

            if (Math.Abs(offset) >= array.Length) {

                Array.Clear(array, 0, array.Length);

            }
            else if (offset < 0) {

                offset = Math.Abs(offset);

                Array.Copy(array, offset, array, 0, array.Length - offset);
                Array.Clear(array, array.Length - offset, offset);

            }
            else if (offset > 0) {

                Array.Copy(array, 0, array, offset, array.Length - offset);
                Array.Clear(array, 0, offset);

            }

        }

    }

}