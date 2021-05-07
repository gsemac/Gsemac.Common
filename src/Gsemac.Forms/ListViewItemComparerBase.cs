using System;
using System.Windows.Forms;

namespace Gsemac.Forms {

    public abstract class ListViewItemComparerBase :
        IListViewItemComparer {

        // Public members

        public int Compare(object x, object y) {

            if (x is null)
                throw new ArgumentNullException(nameof(x));

            if (y is null)
                throw new ArgumentNullException(nameof(y));

            if (!(x is ListViewItem lhs))
                throw new ArgumentException(nameof(x));

            if (!(y is ListViewItem rhs))
                throw new ArgumentNullException(nameof(rhs));

            return Compare(lhs, rhs);

        }
        public abstract int Compare(ListViewItem x, ListViewItem y);

    }

}