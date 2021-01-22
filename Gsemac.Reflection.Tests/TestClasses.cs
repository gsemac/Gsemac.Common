namespace Gsemac.Reflection.Tests {

    internal class ClassWithProperties {

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Z { get; } = 0;

    }

    internal class ClassWithRecursiveProperties {

        public ClassWithProperties A { get; set; } = new ClassWithProperties();
        public ClassWithRecursiveProperties B { get; set; }

    }

}