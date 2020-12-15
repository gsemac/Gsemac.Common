namespace Gsemac.Net.JavaScript {

    public static class JsOperators {

        public static int UnsignedRightShift(int lhs, int rhs) {

            if (lhs >= 0)
                return lhs >> rhs;
            else
                return (lhs >> rhs) + (2 << ~rhs);

        }

    }

}