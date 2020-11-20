using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Gsemac.Net.JavaScript {

    public static class JsNumber {

        public static bool IsNaN(object value) {

            if (value is string str) {

                // Empty string is converted to 0

                if (string.IsNullOrWhiteSpace(str))
                    return false;

            }

            try {

                double doubleValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);

                return !double.IsNaN(doubleValue);

            }
            catch (InvalidCastException) {

                return false;

            }

        }

    }

}