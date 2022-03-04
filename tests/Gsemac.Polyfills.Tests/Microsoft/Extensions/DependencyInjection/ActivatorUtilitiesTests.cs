using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection.Tests {

    [TestClass]
    public class ActivatorUtilitiesTests {

        // CreateFactory

        [TestMethod]
        public void TestCreateFactoryWithServiceProvider() {

            using ServiceProvider s = new ServiceCollection()
                .AddSingleton<MyServiceWithNoDependencies>()
                .AddSingleton<MyServiceWithDependencies>()
                .BuildServiceProvider();

            var objectFactory = ActivatorUtilities.CreateFactory(typeof(MyServiceWithMultipleConstructorsAndOptionalDependencies),
                new[] { typeof(MyServiceWithNoDependencies), typeof(MyServiceWithDependencies) });

            MyServiceWithMultipleConstructorsAndOptionalDependencies service = (MyServiceWithMultipleConstructorsAndOptionalDependencies)objectFactory(s, null);

            Assert.AreEqual(2, service.InvokedConstructorId);

        }
        [TestMethod]
        public void TestCreateFactoryWithParameterArray() {

            var objectFactory = ActivatorUtilities.CreateFactory(typeof(MyServiceWithMultipleConstructorsAndOptionalDependencies),
                new[] { typeof(MyServiceWithNoDependencies), typeof(MyServiceWithDependencies) });

            MyServiceWithMultipleConstructorsAndOptionalDependencies service = (MyServiceWithMultipleConstructorsAndOptionalDependencies)objectFactory(null, new object[] {
                new MyServiceWithNoDependencies(),
                new MyServiceWithDependencies(new MyServiceWithNoDependencies()),
            });

            Assert.AreEqual(2, service.InvokedConstructorId);

        }
        [TestMethod]
        public void TestCreateFactoryThrowsWithNoSuitableConstuctor() {

            Assert.ThrowsException<InvalidOperationException>(() => ActivatorUtilities.CreateFactory(typeof(MyServiceWithMultipleConstructorsAndOptionalDependencies), new[] { typeof(object) }));

        }

        // CreateInstance

        [TestMethod]
        public void TestCreateInstanceWithMultipleInvokableConstructors() {

            using ServiceProvider s = new ServiceCollection()
                .AddSingleton<MyServiceWithNoDependencies>()
                .AddSingleton<MyServiceWithDependencies>()
                .BuildServiceProvider();

            Assert.AreEqual(2, ActivatorUtilities.CreateInstance<MyServiceWithMultipleConstructorsAndDependencies>(s, null).InvokedConstructorId);

        }
        [TestMethod]
        public void TestCreateInstanceWithMultipleInvokableConstructorsAndDependencyPassedByArray() {

            using ServiceProvider s = new ServiceCollection()
                .AddSingleton<MyServiceWithNoDependencies>()
                .BuildServiceProvider();

            MyServiceWithDependencies dependency = new(new MyServiceWithNoDependencies());

            Assert.AreEqual(2, ActivatorUtilities.CreateInstance<MyServiceWithMultipleConstructorsAndDependencies>(s, new[] { dependency }).InvokedConstructorId);

        }
        [TestMethod]
        public void TestCreateInstancePrioritizesParameterArray() {

            using ServiceProvider s = new ServiceCollection()
                .AddSingleton("bad")
                .BuildServiceProvider();

            Assert.AreEqual("good", ActivatorUtilities.CreateInstance<MyServiceWithDependencies>(s, new[] { "good" }).Name);

        }

    }

}