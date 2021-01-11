namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection.Tests {

    internal interface IMyService { }

    internal class MyServiceWithNoDependencies :
        IMyService {

        public MyServiceWithNoDependencies() { }

    }

    internal class MyServiceWithDependencies :
        IMyService {

        public MyServiceWithDependencies(MyServiceWithNoDependencies _) { }

    }

    internal class MyServiceWithMultipleConstructors :
       IMyService {

        public MyServiceWithMultipleConstructors(MyServiceWithNoDependencies service) { }
        public MyServiceWithMultipleConstructors(int x, int y) { }

    }

    internal class MyServiceWithMultipleConstructorsWithActivatorUtilitiesConstructor :
       IMyService {

        [ActivatorUtilitiesConstructor]
        public MyServiceWithMultipleConstructorsWithActivatorUtilitiesConstructor(MyServiceWithNoDependencies service) { }
        public MyServiceWithMultipleConstructorsWithActivatorUtilitiesConstructor(int x, int y) { }

    }

}