using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public delegate object ObjectFactory(IServiceProvider serviceProvider, object[] arguments);

    public static class ActivatorUtilities {

        // Public members

        public static ObjectFactory CreateFactory(Type instanceType, Type[] argumentTypes) {

            return (IServiceProvider serviceProvider, object[] arguments) => CreateInstance(serviceProvider, instanceType, argumentTypes, arguments);

        }
        public static object CreateInstance(IServiceProvider provider, Type instanceType, params object[] parameters) {

            return CreateInstance(provider, instanceType, null, parameters);

        }
        public static T CreateInstance<T>(IServiceProvider provider, params object[] parameters) {

            return (T)CreateInstance(provider, typeof(T), parameters);

        }
        public static object GetServiceOrCreateInstance(IServiceProvider provider, Type type) {

            object serviceObject = provider.GetService(type);

            if (serviceObject is object)
                return serviceObject;

            return CreateInstance(provider, type);

        }
        public static T GetServiceOrCreateInstance<T>(IServiceProvider provider) {

            return (T)GetServiceOrCreateInstance(provider, typeof(T));

        }

        // Private members

        private static object CreateInstance(IServiceProvider provider, Type instanceType, Type[] argumentTypes, params object[] parameters) {

            // All dependencies will be retrieved from the ServiceProvider except for the types specified in argumentTypes (which must be provided in parameters).
            // Any types that cannot be resolved will taken from parameters.

            ConstructorInfo selectedConstructor = null;
            object[] constructorArguments = System.Array.Empty<object>();
            IEnumerable<ConstructorInfo> constructors = instanceType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            // We need to find a suitable constructor for which we have all the necessary arguments, or the one flagged with the ActivatorUtilitiesConstructorAttribute.
            // If there is only one constructor, we'll use that one regardless of the arguments we have available.

            if (constructors.Count() == 1)
                selectedConstructor = constructors.First();
            else {

                selectedConstructor = constructors
                    .Where(constructor => constructor.GetCustomAttributes(inherit: false).Any(attribute => attribute is ActivatorUtilitiesConstructorAttribute))
                    .FirstOrDefault();

            }

            if (selectedConstructor is null) {

                // we need to find the most suitable constructor manually.
                // We'll start with the contructors with the largest number of parameters and work our way down.

                foreach (ConstructorInfo constructorInfo in constructors.OrderByDescending(constructor => constructor.GetParameters().Count())) {

                    constructorArguments = GetConstructorArguments(provider, constructorInfo, argumentTypes, parameters);

                    if (constructorArguments.All(argument => argument is object)) {

                        selectedConstructor = constructorInfo;

                        break;

                    }

                }

            }

            if (selectedConstructor is object && !constructorArguments.Any())
                constructorArguments = GetConstructorArguments(provider, selectedConstructor, argumentTypes, parameters);

            ValidateConstructorArguments(instanceType, selectedConstructor, constructorArguments);

            if (constructorArguments.Any())
                return Activator.CreateInstance(instanceType, constructorArguments);
            else
                return Activator.CreateInstance(instanceType);

        }
        private static object[] GetConstructorArguments(IServiceProvider provider, ConstructorInfo constructorInfo, Type[] argumentTypes, params object[] parameters) {

            if (argumentTypes is null)
                argumentTypes = Enumerable.Empty<Type>().ToArray();

            IEnumerable<Type> parameterTypes = constructorInfo.GetParameters().Select(parameter => parameter.ParameterType);
            IDictionary<Type, object> argumentDict = new Dictionary<Type, object>();

            // Get arguments from the provided list of parameters.

            foreach (Type type in argumentTypes)
                argumentDict[type] = parameters.Where(argument => type.IsAssignableFrom(argument.GetType())).FirstOrDefault();

            // Get arguments from the service provider.

            IEnumerable<Type> parameterTypesToResolve = parameterTypes
                .Where(type => !argumentDict.ContainsKey(type))
                .Where(type => !argumentTypes.Contains(type))
                .Distinct();

            foreach (Type type in parameterTypesToResolve)
                argumentDict[type] = provider.GetService(type);

            // Get arguments that we'll pass to the constructor.
            // If any of them are null, we cannot use the given constructor.

            IEnumerable<object> constructorArguments = parameterTypes.Select(type => argumentDict.ContainsKey(type) ? argumentDict[type] : null);

            return constructorArguments.ToArray();

        }
        private static void ValidateConstructorArguments(Type instanceType, ConstructorInfo constructorInfo, object[] arguments) {

            if (constructorInfo is null)
                return;

            ParameterInfo[] constructorParameters = constructorInfo.GetParameters();

            for (int i = 0; i < constructorParameters.Count() && i < arguments.Count(); ++i) {

                if (arguments[i] is null && !constructorParameters[i].IsOptional)
                    throw new InvalidOperationException($"Unable to resolve service for type '{constructorParameters[i].ParameterType}' while attempting to activate '{instanceType}'.");

            }

        }

    }

}