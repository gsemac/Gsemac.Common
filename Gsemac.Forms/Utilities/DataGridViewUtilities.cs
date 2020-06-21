using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Utilities {

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
        public static Rectangle GetVisibleRowsBounds(DataGridView dataGridView) {

            Rectangle bounds = new Rectangle(0, 0, 0, 0);

            foreach (DataGridViewRow row in GetVisibleRows(dataGridView)) {

                Rectangle rowBounds = dataGridView.GetRowDisplayRectangle(row.Index, true);

                bounds = new Rectangle(0, 0, Math.Max(bounds.Width, rowBounds.Width), bounds.Height + rowBounds.Height);

            }

            return bounds;

        }

    }

}