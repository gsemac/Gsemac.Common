using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection.Tests {

    [TestClass]
    public class ServiceProviderTests {

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

        // GetRequiredService

        [TestMethod]
        public void TestGetRequiredServiceWhenServiceIsNotRegistered() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .BuildServiceProvider();

            Assert.ThrowsException<InvalidOperationException>(() => serviceProvider.GetRequiredService<MyServiceWithDependencies>() is MyServiceWithDependencies);

        }

    }

}