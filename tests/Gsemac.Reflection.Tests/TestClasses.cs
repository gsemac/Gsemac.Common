namespace Gsemac.Reflection.Tests {

    internal enum TestEnum {
        Item1,
        Item2,
        Item3,
    }

    internal class ClassWithProperties {

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Z { get; } = 0;
        public TestEnum Option { get; set; } = TestEnum.Item1;

    }

    internal class ClassWithRecursiveProperties {

        public ClassWithProperties A { get; set; } = new ClassWithProperties();
        public ClassWithRecursiveProperties B { get; set; }

    }

}