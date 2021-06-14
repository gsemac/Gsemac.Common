using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection.Tests {

    [TestClass]
    public class ServiceProviderTests {

        // Dispose

        [TestMethod]
        public void TestServiceProviderDisposesOfSingletonServices() {

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<MyDisposableService>()
                .BuildServiceProvider();

            MyDisposableService service = serviceProvider.GetService<MyDisposableService>();

            Assert.IsFalse(service.Disposed);

            serviceProvider.Dispose();

            Assert.IsTrue(service.Disposed);

        }
        [TestMethod]
        public void TestServiceProviderDisposesOfTransientServices() {

            // The service provider must keep track of and dispose transient services that implement IDisposable.
            // https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines#example-anti-patterns

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<MyDisposableService>()
                .BuildServiceProvider();

            MyDisposableService service = serviceProvider.GetService<MyDisposableService>();

            Assert.IsFalse(service.Disposed);

            serviceProvider.Dispose();

            Assert.IsTrue(service.Disposed);

        }
        [TestMethod]
        public void TestServiceProviderDisposesOfScopedServices() {

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddScoped<MyDisposableService>()
                .BuildServiceProvider(validateScopes: false);

            MyDisposableService service = serviceProvider.GetService<MyDisposableService>();

            Assert.IsFalse(service.Disposed);

            serviceProvider.Dispose();

            Assert.IsTrue(service.Disposed);

        }

        // GetService

        [TestMethod]
        public void TestGetServiceWithNoDependencies() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<MyServiceWithNoDependencies>()
                .BuildServiceProvider();

            Assert.IsTrue(serviceProvider.GetService<MyServiceWithNoDependencies>() is MyServiceWithNoDependencies);

        }
        [TestMethod]
        public void TestGetServiceWithDependenciesWhenDependenciesAreRegistered() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<MyServiceWithDependencies>()
                .AddTransient<MyServiceWithNoDependencies>()
                .BuildServiceProvider();

            Assert.IsTrue(serviceProvider.GetService<MyServiceWithDependencies>() is MyServiceWithDependencies);

        }
        [TestMethod]
        public void TestGetServiceWithDependenciesWhenDependenciesAreNotRegistered() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<MyServiceWithDependencies>()
                .BuildServiceProvider();

            Assert.ThrowsException<InvalidOperationException>(() => serviceProvider.GetService<MyServiceWithDependencies>());

        }
        [TestMethod]
        public void TestGetServiceWithOptionalDependenciesWhenDependenciesAreNotRegistered() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<MyServiceWithOptionalDependencies>()
                .BuildServiceProvider();

            Assert.IsTrue(serviceProvider.GetService<MyServiceWithOptionalDependencies>() is MyServiceWithOptionalDependencies);

        }
        [TestMethod]
        public void TestGetServiceWithMultipleConstructorsWithOptionalDependenciesWhenDependenciesAreNotRegistered() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<MyServiceWithMultipleConstructorsAndOptionalDependencies>()
                .BuildServiceProvider();

            Assert.IsTrue(serviceProvider.GetService<MyServiceWithMultipleConstructorsAndOptionalDependencies>().InvokedConstructorId == 2);

        }
        [TestMethod]
        public void TestGetServiceWhenServiceIsNotRegistered() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .BuildServiceProvider();

            Assert.IsTrue(serviceProvider.GetService<MyServiceWithNoDependencies>() is null);

        }
        [TestMethod]
        public void TestGetServiceByInterface() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<IMyService, MyServiceWithNoDependencies>()
                .BuildServiceProvider();

            Assert.IsTrue(serviceProvider.GetService<IMyService>() is MyServiceWithNoDependencies);

        }

        [TestMethod]
        public void TestGetSingletonService() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<IMyService, MyServiceWithNoDependencies>()
                .BuildServiceProvider();

            Assert.IsTrue(serviceProvider.GetService<IMyService>().Equals(serviceProvider.GetService<IMyService>()));

        }
        [TestMethod]
        public void TestGetSingletonWithManualInstantiation() {

            MyServiceWithNoDependencies myService = new MyServiceWithNoDependencies();

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<IMyService>(myService)
                .BuildServiceProvider();

            Assert.IsTrue(serviceProvider.GetService<IMyService>().Equals(myService));

        }
        [TestMethod]
        public void TestGetSingletonWithFactoryFunctionIsInstantiatedOnlyOnce() {

            int instantiations = 0;

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<IMyService>((sp) => {

                    ++instantiations;

                    return new MyServiceWithNoDependencies();

                })
                .BuildServiceProvider();

            serviceProvider.GetRequiredService<IMyService>();
            serviceProvider.GetRequiredService<IMyService>();
            serviceProvider.GetRequiredService<IMyService>();

            Assert.AreEqual(instantiations, 1);

        }
        [TestMethod]
        public void TestGetTransientService() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<IMyService, MyServiceWithNoDependencies>()
                .BuildServiceProvider();

            Assert.IsFalse(serviceProvider.GetService<IMyService>().Equals(serviceProvider.GetService<IMyService>()));

        }
        [TestMethod]
        public void TestGetScopedService() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddScoped<IMyService, MyServiceWithNoDependencies>()
                .BuildServiceProvider();

            IMyService rootScopedService = serviceProvider.GetService<IMyService>();

            using (IServiceScope scope = serviceProvider.CreateScope()) {

                IMyService scopedService = scope.ServiceProvider.GetService<IMyService>();

                Assert.IsTrue(scopedService.Equals(scope.ServiceProvider.GetService<IMyService>()));
                Assert.IsFalse(rootScopedService.Equals(scopedService));

            }

        }
        [TestMethod]
        public void TestGetScopedServiceFromRootServiceProvider() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddScoped<IMyService, MyServiceWithNoDependencies>()
                .BuildServiceProvider();

            Assert.IsTrue(serviceProvider.GetService<IMyService>().Equals(serviceProvider.GetService<IMyService>()));

        }
        [TestMethod]
        public void TestGetScopedServiceFromRootServiceProviderWithScopeValidation() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddScoped<IMyService, MyServiceWithNoDependencies>()
                .BuildServiceProvider(new ServiceProviderOptions() { ValidateScopes = true });

            Assert.ThrowsException<InvalidOperationException>(() => serviceProvider.GetService<IMyService>());

        }

        [TestMethod]
        public void TestGetServiceWithServiceProvider() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .BuildServiceProvider();

            Assert.IsTrue(serviceProvider.GetService<IServiceProvider>().Equals(serviceProvider));

        }
        [TestMethod]
        public void TestGetServiceWithSecondServiceProvider() {

            // Attempting to resolve IServiceProvider should always return the IServiceProvider instance itself instead of the one that has been registered.

            IServiceProvider serviceProvider1 = new ServiceCollection()
                .AddSingleton(new MyNamedService("hello"))
                .BuildServiceProvider();

            IServiceProvider serviceProvider2 = new ServiceCollection()
                .AddSingleton(serviceProvider1)
                .AddSingleton(new MyNamedService("world"))
                .BuildServiceProvider();

            Assert.IsTrue(serviceProvider2.GetRequiredService<IServiceProvider>().GetRequiredService<MyNamedService>().Name.Equals("world"));

        }

        // GetServices

        [TestMethod]
        public void TestGetServicesWithSecondServiceProvider() {

            // Even though we have the IServiceProvider instance itself and the second one we registered, GetServices should only return the second one.

            IServiceProvider serviceProvider1 = new ServiceCollection()
                .BuildServiceProvider();

            IServiceProvider serviceProvider2 = new ServiceCollection()
                .AddSingleton(serviceProvider1)
                .BuildServiceProvider();

            IEnumerable<IServiceProvider> instances = serviceProvider2.GetServices<IServiceProvider>();

            Assert.IsTrue(instances.Count() == 1);
            Assert.IsTrue(ReferenceEquals(instances.First(), serviceProvider1));

        }

        // GetRequiredService

        [TestMethod]
        public void TestGetRequiredServiceWhenServiceIsNotRegistered() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .BuildServiceProvider();

            Assert.ThrowsException<InvalidOperationException>(() => serviceProvider.GetRequiredService<MyServiceWithDependencies>() is MyServiceWithDependencies);

        }

    }

}