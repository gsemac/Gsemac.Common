namespace Gsemac.Core {

    public static class TriStateExtensions {

        public static bool? ToBoolean(this TriState value) {

            switch (value) {

                case TriState.True:
                    return true;

                case TriState.False:
                    return false;

                default:
                    return null;

            }

        }

    }

}