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

        // GetRequiredService

        [TestMethod]
        public void TestGetRequiredServiceWhenServiceIsNotRegistered() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .BuildServiceProvider();

            Assert.ThrowsException<InvalidOperationException>(() => serviceProvider.GetRequiredService<MyServiceWithDependencies>() is MyServiceWithDependencies);

        }

    }

}