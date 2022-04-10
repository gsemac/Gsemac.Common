using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public delegate object ObjectFactory(IServiceProvider serviceProvider, object[] arguments);

    public static class ActivatorUtilities {

        // Public members

        public static ObjectFactory CreateFactory(Type instanceType, Type[] argumentTypes) {

            if (instanceType is null)
                throw new ArgumentNullException(nameof(instanceType));

            if (argumentTypes is null)
                argumentTypes = ArrayEx.Empty<Type>();

            // Properly implemented, this method should create an optimized factory method by selecting a constructor with parameters of the given types.

            if (!instanceType.GetConstructors().Any(c => IsConstructorInvokable(c, argumentTypes)))
                throw new InvalidOperationException(string.Format(Properties.ExceptionMessages.NoSuitableConstructorWithTypeName, instanceType));

            return (IServiceProvider serviceProvider, object[] arguments) => CreateInstanceInternal(serviceProvider, instanceType, arguments);

        }
        public static object CreateInstance(IServiceProvider provider, Type instanceType, params object[] parameters) {

            if (instanceType is null)
                throw new ArgumentNullException(nameof(instanceType));

            if (parameters is null)
                parameters = ArrayEx.Empty<object>();

            return CreateInstanceInternal(provider, instanceType, parameters);

        }
        public static T CreateInstance<T>(IServiceProvider provider, params object[] parameters) {

            if (parameters is null)
                parameters = ArrayEx.Empty<object>();

            return (T)CreateInstance(provider, typeof(T), parameters);

        }
        public static object GetServiceOrCreateInstance(IServiceProvider provider, Type type) {

            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            object serviceObject = provider.GetService(type);

            if (serviceObject is object)
                return serviceObject;

            return CreateInstance(provider, type);

        }
        public static T GetServiceOrCreateInstance<T>(IServiceProvider provider) {

            return (T)GetServiceOrCreateInstance(provider, typeof(T));

        }

        // Private members

        private static object CreateInstanceInternal(IServiceProvider provider, Type instanceType, params object[] parameters) {

            if (instanceType is null)
                throw new ArgumentNullException(nameof(instanceType));

            // All dependencies will be retrieved from the ServiceProvider except for the types specified in argumentTypes (which must be provided in parameters).
            // Any types that cannot be resolved will taken from parameters.

            ConstructorInfo selectedConstructor = null;
            object[] constructorArguments = ArrayEx.Empty<object>();
            IEnumerable<ConstructorInfo> constructors = instanceType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            // We need to find a suitable constructor for which we have all the necessary arguments, or the one flagged with the ActivatorUtilitiesConstructorAttribute.
            // If there is only one constructor, we'll use that one regardless of the arguments we have available.

            if (constructors.Count() == 1) {

                selectedConstructor = constructors.First();

            }
            else {

                selectedConstructor = constructors
                    .Where(constructor => constructor.GetCustomAttributes(inherit: false).Any(attribute => attribute is ActivatorUtilitiesConstructorAttribute))
                    .FirstOrDefault();

            }

            if (selectedConstructor is null) {

                // we need to find the most suitable constructor manually.
                // We'll start with the contructors with the largest number of parameters and work our way down.

                foreach (ConstructorInfo constructorInfo in constructors.OrderByDescending(constructor => constructor.GetParameters().Count())) {

                    constructorArguments = GetConstructorArguments(provider, constructorInfo, parameters);

                    if (IsConstructorInvokable(constructorInfo, constructorArguments)) {

                        selectedConstructor = constructorInfo;

                        break;

                    }

                }

            }

            if (selectedConstructor is null)
                selectedConstructor = constructors.First();

            if (!constructorArguments.Any())
                constructorArguments = GetConstructorArguments(provider, selectedConstructor, parameters);

            ThrowIfContsructorIsNotInvokable(instanceType, selectedConstructor, constructorArguments);

            return selectedConstructor.Invoke(constructorArguments);

        }

        private static object[] GetConstructorArguments(IServiceProvider provider, ConstructorInfo constructorInfo, params object[] parameters) {

            // The official implementation of ActivatorUtilities throws a NullReferenceException if a null parameter array is passed.
            // I don't appreciate this behavior, personally, so we'll treat a null parameter array as if it was an empty one.

            if (parameters is null)
                parameters = ArrayEx.Empty<Type>();

            IEnumerable<Type> parameterTypes = constructorInfo.GetParameters().Select(parameter => parameter.ParameterType);
            IDictionary<Type, object> argumentsDict = new Dictionary<Type, object>();

            // Get arguments from the parameter array and the service provider.
            // Arguments provided by the parameter array should be prioritized.

            // The official implementation of ActivatorUtilities allows a null IServiceProvider to be passed.
            // This requires that all necessary arguments are passed in the parameters array.

            foreach (Type type in parameterTypes) {

                object resolvedObject = parameters.Where(p => type.IsAssignableFrom(p.GetType())).FirstOrDefault();

                if (resolvedObject is null && provider is object)
                    resolvedObject = provider.GetService(type);

                if (resolvedObject is object)
                    argumentsDict[type] = resolvedObject;

            }

            // Get the arguments in the order that they will be passed to the constructor.

            IEnumerable<object> constructorArguments = parameterTypes.Select(type => argumentsDict.ContainsKey(type) ? argumentsDict[type] : null);

            return constructorArguments.ToArray();

        }
        private static bool IsConstructorInvokable(ConstructorInfo constructorInfo, object[] arguments) {

            if (constructorInfo is null)
                throw new ArgumentNullException(nameof(constructorInfo));

            return constructorInfo.GetParameters()
                .Zip(arguments, (x, y) => Tuple.Create(x, y))
                .All(tuple => tuple.Item1.IsOptional || tuple.Item2 is object);

        }
        private static bool IsConstructorInvokable(ConstructorInfo constructorInfo, Type[] parameterTypes) {

            return constructorInfo.GetParameters()
                .Zip(parameterTypes, (x, y) => Tuple.Create(x, y))
                .All(tuple => tuple.Item1.ParameterType.IsAssignableFrom(tuple.Item2));

        }
        private static void ThrowIfContsructorIsNotInvokable(Type instanceType, ConstructorInfo constructorInfo, object[] arguments) {

            if (constructorInfo is null)
                return;

            ParameterInfo[] constructorParameters = constructorInfo.GetParameters();

            for (int i = 0; i < constructorParameters.Count() && i < arguments.Count(); ++i) {

                if (!IsConstructorInvokable(constructorInfo, arguments))
                    throw new InvalidOperationException(string.Format(Properties.ExceptionMessages.UnableToResolveTypeWithTypeNameAndTypeName, constructorParameters[i].ParameterType, instanceType));

            }

        }

    }

}