using Gsemac.Collections;
using System;
using System.Windows.Forms;

namespace Gsemac.Forms {

    public class ListViewItemComparer :
          ListViewItemComparerBase {

        // Public members

        public int Column { get; set; }
        public SortOrder SortOrder { get; set; }

        public ListViewItemComparer(int column) :
            this(column, SortOrder.Ascending) {
        }
        public ListViewItemComparer(int column, SortOrder sortOrder) {

            SortOrder = sortOrder;
            Column = column;

        }

        public override int Compare(ListViewItem x, ListViewItem y) {

            ListViewItem lhs = SortOrder == SortOrder.Descending ? y : x;
            ListViewItem rhs = SortOrder == SortOrder.Descending ? x : y;

            if (SortOrder == SortOrder.None) {

                // Allow the tag to be used to reset the original sort order.

                if (lhs.Tag is int lhsTag && rhs.Tag is int rhsTag)
                    return lhsTag.CompareTo(rhsTag);

                return 0;

            }
            else {

                string lhsStr = lhs.SubItems[Column].Text;
                string rhsStr = rhs.SubItems[Column].Text;

                if (DateTime.TryParse(lhsStr, out DateTime lhsDate) && DateTime.TryParse(rhsStr, out DateTime rhsDate))
                    return lhsDate.CompareTo(rhsDate);
                else
                    return new NaturalSortComparer().Compare(lhsStr, rhsStr);

            }

        }

    }

}