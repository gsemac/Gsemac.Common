using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms {

    public class ListViewItemComparer :
          IComparer,
          IComparer<ListViewItem> {

        public int Column { get; set; }
        public SortOrder SortOrder { get; set; }

        public ListViewItemComparer(int column) :
            this(column, SortOrder.Ascending) {
        }
        public ListViewItemComparer(int column, SortOrder sortOrder) {

            SortOrder = sortOrder;
            Column = column;

        }

        public int Compare(object x, object y) {

            ListViewItem lhs = x as ListViewItem;
            ListViewItem rhs = y as ListViewItem;

            return Compare(lhs, rhs);

        }
        public int Compare(ListViewItem x, ListViewItem y) {

            ListViewItem lhs = SortOrder == SortOrder.Descending ? y : x;
            ListViewItem rhs = SortOrder == SortOrder.Descending ? x : y;

            string lhsStr = lhs.SubItems[Column].Text;
            string rhsStr = rhs.SubItems[Column].Text;

            if (DateTime.TryParse(lhsStr, out DateTime lhsDate) && DateTime.TryParse(rhsStr, out DateTime rhsDate))
                return lhsDate.CompareTo(rhsDate);
            else
                return lhsStr.CompareTo(rhsStr);

        }

    }

}