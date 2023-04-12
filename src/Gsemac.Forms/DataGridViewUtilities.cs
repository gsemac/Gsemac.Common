using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms {

    public static class DataGridViewUtilities {

        // Public members

        public static IEnumerable<DataGridViewRow> GetVisibleRows(DataGridView dataGridView) {

            List<DataGridViewRow> visibleRows = new List<DataGridViewRow>();

            int visibleRowsCount = dataGridView.DisplayedRowCount(true);

            if (visibleRowsCount > 0) {

                int firstDisplayedRowIndex = dataGridView.FirstDisplayedCell.RowIndex;

                for (int i = firstDisplayedRowIndex; i < dataGridView.Rows.Count && visibleRows.Count < visibleRowsCount; ++i) {

                    if (dataGridView.Rows[i].Visible)
                        visibleRows.Add(dataGridView.Rows[i]);

                }

            }

            return visibleRows;

        }

        public static Rectangle GetVisibleRowBounds(DataGridView dataGridView, int rowIndex, bool includeHeaders = true) {

            Debug.Assert(rowIndex >= 0);
            Debug.Assert(rowIndex < dataGridView.Rows.Count);

            if (!dataGridView.Rows[rowIndex].Visible || dataGridView.Columns.Count <= 0)
                return new Rectangle(0, 0, 0, 0);

            Rectangle rowBounds = dataGridView.GetRowDisplayRectangle(rowIndex, true);

            // GetRowDisplayRectangle returns the entire row rectangle, whether or not there are actually columns all the way across.
            // Subtract the area beyond the last column.

            Rectangle lastColumnBounds = dataGridView.GetColumnDisplayRectangle(dataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None).Index, true);

            int rowRight = rowBounds.X + rowBounds.Width;
            int lastColumnRight = lastColumnBounds.X + lastColumnBounds.Width;

            if (lastColumnRight < rowRight)
                rowBounds = new Rectangle(rowBounds.X, rowBounds.Y, rowBounds.Width - (rowRight - lastColumnRight), rowBounds.Height);

            // If headers are not included, subtract the row reader.

            if (!includeHeaders && dataGridView.RowHeadersVisible)
                rowBounds = new Rectangle(rowBounds.X + dataGridView.RowHeadersWidth, rowBounds.Y, rowBounds.Width - dataGridView.RowHeadersWidth, rowBounds.Height);

            return rowBounds;

        }
        public static Rectangle GetVisibleRowsBounds(DataGridView dataGridView, bool includeHeaders = true) {

            IEnumerable<DataGridViewRow> visibleRows = GetVisibleRows(dataGridView);

            // If there are no visible rows, an empty rectangle will be returned.

            Rectangle bounds = new Rectangle(0, 0, 0, 0);

            if (visibleRows.Count() > 0)
                bounds = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);

            // If headers are included, include the column headers.

            if (includeHeaders)
                bounds = new Rectangle(bounds.X, 0, bounds.Width, dataGridView.ColumnHeadersHeight);

            foreach (DataGridViewRow row in visibleRows) {

                Rectangle rowBounds = GetVisibleRowBounds(dataGridView, row.Index, includeHeaders);

                bounds = new Rectangle(
                    Math.Min(bounds.X, rowBounds.X),
                    Math.Min(bounds.Y, rowBounds.Y),
                    Math.Max(bounds.Width, rowBounds.Width),
                    bounds.Height + rowBounds.Height
                    );

            }

            return bounds;

        }

        public static bool IsRowHeaderIndex(int columnIndex) {

            return columnIndex < 0;

        }
        public static bool IsColumnHeaderIndex(int rowIndex) {

            return rowIndex < 0;

        }
        public static DataGridViewCellStyle GetDefaultCellStyle(DataGridView dataGridView, int rowIndex, int columnIndex) {

            if (IsColumnHeaderIndex(rowIndex))
                return dataGridView.ColumnHeadersDefaultCellStyle;

            if (IsRowHeaderIndex(columnIndex))
                return dataGridView.RowHeadersDefaultCellStyle;

            return dataGridView.DefaultCellStyle;

        }

        public static void SetRealTimeResizingEnabled(DataGridView dataGridView, bool enabled) {

            if (enabled) {

                dataGridView.MouseDown += DataGridViewRealTimeResizingMouseDownHandler;
                dataGridView.MouseMove += DataGridViewRealTimeResizingMouseMoveHandler;
                dataGridView.MouseUp += DataGridViewRealTimeResizingMouseUpHandler;

            }
            else {

                dataGridView.MouseDown -= DataGridViewRealTimeResizingMouseDownHandler;
                dataGridView.MouseMove -= DataGridViewRealTimeResizingMouseMoveHandler;
                dataGridView.MouseUp -= DataGridViewRealTimeResizingMouseUpHandler;

            }

        }

        // Private members

        // These variables will be used globally for all DataGridViews that have real-time resizing enabled

        private static bool columnResizing = false;
        private static int columnResized = 0;
        private static int columnOffset = 0;
        private static int columnWidth = 0;

        private static void DataGridViewRealTimeResizingMouseDownHandler(object sender, MouseEventArgs e) {

            try {

                if (sender is DataGridView dataGridView && Cursor.Current == Cursors.SizeWE) {

                    DataGridView.HitTestInfo ht = dataGridView.HitTest(e.X, e.Y);

                    columnResized = ht.ColumnIndex;

                    // If within 8 pixels of the left-side column, assume that it is the left-side column that is being resized.

                    if (e.X < dataGridView.GetCellDisplayRectangle(columnResized, 0, false).Left + 8) {

                        ht = dataGridView.HitTest(e.X - 8, e.Y);

                        columnResized = ht.ColumnIndex;

                    }

                    columnOffset = e.X;
                    columnWidth = dataGridView.Columns[columnResized].Width;
                    columnResizing = true;

                }

            }
            catch (Exception) { }

        }
        private static void DataGridViewRealTimeResizingMouseMoveHandler(object sender, MouseEventArgs e) {

            if (sender is DataGridView dataGridView && columnResizing) {

                if (e.X > columnOffset)
                    dataGridView.Columns[columnResized].Width = columnWidth + (e.X - columnOffset);
                else
                    dataGridView.Columns[columnResized].Width = columnWidth - (columnOffset - e.X);

            }

        }
        private static void DataGridViewRealTimeResizingMouseUpHandler(object sender, MouseEventArgs e) {

            if (Cursor.Current == Cursors.SizeWE) {

                columnResizing = false;

            }

        }

    }

}