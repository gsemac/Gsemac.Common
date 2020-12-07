using System;

namespace Gsemac.Core.Extensions {

    public static class ArrayExtensions {

        public static void Rotate<T>(this T[] array, int offset) {

            if (array is null)
                throw new ArgumentNullException(nameof(array));

            if (offset == 0 || array.Length <= 0 || offset % array.Length == 0)
                return;

            offset %= array.Length;

            if (offset < 0)
                offset = array.Length + offset;

            Array.Reverse(array, 0, array.Length);
            Array.Reverse(array, 0, offset);
            Array.Reverse(array, offset, array.Length - offset);

        }

    }

}