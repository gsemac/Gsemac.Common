using Gsemac.Win32;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms {

    public static class ListViewUtilities {

        // Public members

        public static void Sort(ListView listView, int column, SortOrder sortOrder) {

            if (listView is null)
                throw new ArgumentNullException(nameof(listView));

            if (column < 0 || column >= listView.Columns.Count)
                throw new ArgumentOutOfRangeException(nameof(column));

            listView.BeginUpdate();

            try {

                // Temporarily set the ListViewItemSorter to null to avoid sorting when Sorting is called.

                listView.ListViewItemSorter = null;
                listView.Sorting = sortOrder;

                listView.ListViewItemSorter = new ListViewItemComparer(column, sortOrder);

                if (sortOrder != SortOrder.None)
                    listView.Sort();

                SetColumnSortIcon(listView, column, sortOrder, useNativeSortIcon: true);

            }
            finally {

                listView.EndUpdate();

            }

        }

        public static void SetHeaderSortingEnabled(ListView listView, bool enabled) {

            // This prevents us from adding the event handler twice.

            listView.ColumnClick -= ListViewColumnClickEventHandler;
            
            if (enabled) {

                foreach (var pair in listView.Items.Cast<ListViewItem>().Select((item, index) => Tuple.Create(item, index))) {

                    if (pair.Item1.Tag is null)
                        pair.Item1.Tag = pair.Item2;

                }

                listView.ColumnClick += ListViewColumnClickEventHandler;

            }

        }

        // Private members

        private const char AscendingArrow = '▲';
        private const char DescendingArrow = '▼';

        private static void SetColumnSortIcon(ListView listView, int column, SortOrder sortOrder, bool useNativeSortIcon) {

            if (column >= 0 && column < listView.Columns.Count) {

                if (useNativeSortIcon) {

                    SetNativeColumnSortIcon(listView, column, sortOrder);

                }
                else {

                    SetStringColumnSortIcon(listView, column, sortOrder);

                }

            }

        }
        private static void SetNativeColumnSortIcon(ListView listView, int column, SortOrder sortOrder) {

            IntPtr columnHeader = User32.SendMessage(listView.Handle, Comctl32.LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);

            for (int columnIndex = 0; columnIndex < listView.Columns.Count; ++columnIndex) {

                var columnPtr = new IntPtr(columnIndex);

                HDItemA item = new HDItemA() {
                    Mask = Comctl32.HDI_FORMAT,
                };

                if (User32.SendMessage(columnHeader, Comctl32.HDM_GETITEM, columnPtr, ref item) == IntPtr.Zero)
                    throw new Win32Exception();

                item.Fmt &= ~Comctl32.HDF_SORTDOWN & ~Comctl32.HDF_SORTUP;

                if (sortOrder != SortOrder.None && columnIndex == column) {

                    switch (sortOrder) {

                        case SortOrder.Ascending:
                            item.Fmt |= Comctl32.HDF_SORTUP;
                            break;

                        case SortOrder.Descending:
                            item.Fmt |= Comctl32.HDF_SORTDOWN;
                            break;

                    }

                }

                if (User32.SendMessage(columnHeader, Comctl32.HDM_SETITEM, columnPtr, ref item) == IntPtr.Zero)
                    throw new Win32Exception();

            }

        }
        private static void SetStringColumnSortIcon(ListView listView, int column, SortOrder sortOrder) {

            for (int columnIndex = 0; columnIndex < listView.Columns.Count; ++columnIndex) {

                string columnText = (listView.Columns[columnIndex].Text ?? "")
                    .Trim(' ', AscendingArrow, DescendingArrow);

                if (sortOrder != SortOrder.None && columnIndex == column) {

                    switch (sortOrder) {

                        case SortOrder.Ascending:
                            columnText += " " + AscendingArrow;
                            break;

                        case SortOrder.Descending:
                            columnText += " " + DescendingArrow;
                            break;

                    }

                }

                listView.Columns[columnIndex].Text = columnText;

            }

        }

        private static void ListViewColumnClickEventHandler(object sender, ColumnClickEventArgs e) {

            if (sender is ListView listView) {

                SortOrder sortOrder = listView.Sorting;

                switch (sortOrder) {

                    case SortOrder.None:
                        sortOrder = SortOrder.Ascending;
                        break;

                    case SortOrder.Ascending:
                        sortOrder = SortOrder.Descending;
                        break;

                    case SortOrder.Descending:
                        sortOrder = SortOrder.None;
                        break;

                }

                Sort(listView, e.Column, sortOrder);

            }

        }

    }

}