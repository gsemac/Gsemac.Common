using System.Collections.Generic;

namespace Gsemac.Core {

    public sealed class HashCodeBuilder :
        IHashCodeBuilder {

        // Public members

        public IHashCodeBuilder Add(object obj) {

            hashCodes.Add(obj?.GetHashCode() ?? 0);

            return this;

        }

        public int Build() {

            // https://stackoverflow.com/a/1646913/5383169

            unchecked {

                int hash = 17;

                foreach (int hashCode in hashCodes)
                    hash *= 31 + hashCode;

                return hash;

            }

        }

        public override int GetHashCode() {

            return Build();

        }

        // Private members

        private readonly IList<int> hashCodes = new List<int>();

    }

}