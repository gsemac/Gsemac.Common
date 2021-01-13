using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public class ServiceCollection :
        IServiceCollection {

        // Public members

        public ServiceDescriptor this[int index] {
            get => serviceDescriptors[index];
            set => serviceDescriptors[index] = value;
        }

        public int Count => serviceDescriptors.Count;
        public bool IsReadOnly => serviceDescriptors.IsReadOnly;

        public bool Contains(ServiceDescriptor item) {

            return serviceDescriptors.Contains(item);

        }

        public int IndexOf(ServiceDescriptor item) {

            return serviceDescriptors.IndexOf(item);

        }
        public void Insert(int index, ServiceDescriptor item) {

            serviceDescriptors.Insert(index, item);

        }

        public bool Remove(ServiceDescriptor item) {

            return serviceDescriptors.Remove(item);

        }
        public void RemoveAt(int index) {

            serviceDescriptors.RemoveAt(index);

        }
        public void Clear() {

            serviceDescriptors.Clear();

        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex) {

            serviceDescriptors.CopyTo(array, arrayIndex);

        }

        public IEnumerator<ServiceDescriptor> GetEnumerator() {

            return serviceDescriptors.GetEnumerator();



        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item) {

            serviceDescriptors.Add(item);

        }

        // Private members

        private readonly IList<ServiceDescriptor> serviceDescriptors = new List<ServiceDescriptor>();

    }

}