using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection.Tests {

    internal interface IMyService { }

    internal class MyServiceWithNoDependencies :
        IMyService {

        public MyServiceWithNoDependencies() { }

    }

    internal class MyServiceWithDependencies :
        IMyService {

        public MyServiceWithDependencies(MyServiceWithNoDependencies service) { }

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

    internal class MyServiceWithMultipleConstructorsAndOptionalDependencies :
        IMyService {

        public int InvokedConstructorId { get; } = -1;

        public MyServiceWithMultipleConstructorsAndOptionalDependencies(MyServiceWithNoDependencies service = null) {

            InvokedConstructorId = 1;

        }
        public MyServiceWithMultipleConstructorsAndOptionalDependencies(MyServiceWithNoDependencies service1 = null, MyServiceWithNoDependencies service2 = null) {

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

}