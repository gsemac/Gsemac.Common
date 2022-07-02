using Gsemac.Core;

namespace Gsemac.Drawing {

    public struct LabColor {

        // Public members

        public double L => l;
        public double A => a;
        public double B => b;

        public LabColor(double l, double a, double b) {

            this.l = l;
            this.a = a;
            this.b = b;

        }

        public override bool Equals(object obj) {

            return GetHashCode() == obj.GetHashCode();

        }
        public override int GetHashCode() {

            return new HashCodeBuilder()
                .Add(L)
                .Add(A)
                .Add(B)
                .Build();

        }

        public static bool operator ==(LabColor left, LabColor right) {

            return left.Equals(right);

        }
        public static bool operator !=(LabColor left, LabColor right) {

            return !(left == right);

        }

        // Private members

        private readonly double l;
        private readonly double a;
        private readonly double b;

    }

}