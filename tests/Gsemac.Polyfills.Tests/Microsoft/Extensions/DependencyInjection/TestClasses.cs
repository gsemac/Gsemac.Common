using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection.Tests {

    internal interface IMyService { }

    internal class MyServiceWithNoDependencies :
        IMyService {

        public MyServiceWithNoDependencies() { }

    }

    internal class MyServiceWithDependencies :
        IMyService {

        public string Name { get; set; }

        public MyServiceWithDependencies(MyServiceWithNoDependencies service) { }
        public MyServiceWithDependencies(string name) {

            Name = name;

        }

    }

    internal class MyServiceWithOptionalDependencies :
        IMyService {

        public MyServiceWithOptionalDependencies(MyServiceWithNoDependencies service = null) { }

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

    internal class MyServiceWithMultipleConstructorsAndDependencies :
        IMyService {

        public int InvokedConstructorId { get; } = -1;

        public MyServiceWithMultipleConstructorsAndDependencies(MyServiceWithNoDependencies service) {

            InvokedConstructorId = 1;

        }
        public MyServiceWithMultipleConstructorsAndDependencies(MyServiceWithNoDependencies service1, MyServiceWithDependencies service2) {

            InvokedConstructorId = 2;

        }

    }

    internal class MyServiceWithMultipleConstructorsAndOptionalDependencies :
        IMyService {

        public int InvokedConstructorId { get; } = -1;

        public MyServiceWithMultipleConstructorsAndOptionalDependencies(MyServiceWithNoDependencies service = null) {

            InvokedConstructorId = 1;

        }
        public MyServiceWithMultipleConstructorsAndOptionalDependencies(MyServiceWithNoDependencies service1 = null, MyServiceWithDependencies service2 = null) {

            InvokedConstructorId = 2;

        }

    }

    internal sealed class MyDisposableService :
        IMyService,
        IDisposable {

        public bool Disposed { get; private set; } = false;

        public void Dispose() {

            Disposed = true;

        }

    }

    internal class MyNamedService {

        public string Name { get; }

        public MyNamedService(string name) {

            Name = name;

        }

    }

}