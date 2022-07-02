using Gsemac.Core;

namespace Gsemac.Drawing {

    public struct XyzColor {

        // Public members

        public double X => x;
        public double Y => y;
        public double Z => z;

        public XyzColor(double x, double y, double z) {

            this.x = x;
            this.y = y;
            this.z = z;

        }

        public override bool Equals(object obj) {

            return GetHashCode() == obj.GetHashCode();

        }
        public override int GetHashCode() {

            return new HashCodeBuilder()
                .Add(X)
                .Add(Y)
                .Add(Z)
                .Build();

        }

        public static bool operator ==(XyzColor left, XyzColor right) {

            return left.Equals(right);

        }
        public static bool operator !=(XyzColor left, XyzColor right) {

            return !(left == right);

        }

        // Private members

        private readonly double x;
        private readonly double y;
        private readonly double z;

    }

}