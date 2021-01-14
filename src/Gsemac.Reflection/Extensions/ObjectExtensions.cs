namespace Gsemac.Reflection.Extensions {

    public static class ObjectExtensions {

        public static IObjectPropertyDictionary ToDictionary(this object obj) {

            return new ObjectPropertyDictionary(obj);

        }

    }

}