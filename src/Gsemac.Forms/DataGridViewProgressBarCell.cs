using Gsemac.Drawing.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

// This implementation is based off of the implementation provided here:
// https://social.msdn.microsoft.com/Forums/windows/en-US/769ca9d6-1e9d-4d76-8c23-db535b2f19c2/sample-code-datagridview-progress-bar-column?forum=winformsdatacontrols

namespace Gsemac.Forms {

    public class DataGridViewProgressBarCell :
        DataGridViewImageCell {

        // Public members

        public bool ShowProgressPercentage { get; set; } = true;

        public Color BackgroundColor { get; set; } = Color.FromArgb(230, 230, 230);
        public Color ProgressColor { get; set; } = Color.FromArgb(6, 176, 37);
        public Color TextColor { get; set; }

        public DataGridViewProgressBarCell() {

            ValueType = typeof(double);

        }

        public override object Clone() {

            DataGridViewProgressBarCell clone = (DataGridViewProgressBarCell)base.Clone();

            clone.ShowProgressPercentage = ShowProgressPercentage;
            clone.BackgroundColor = BackgroundColor;
            clone.ProgressColor = ProgressColor;
            clone.TextColor = TextColor;

            return clone;

        }

        // Protected members

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context) {

            return emptyImage;

        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts) {

            // Paint the default cell appearance.

            DataGridViewCellStyle style = cellStyle;

            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, style, advancedBorderStyle, paintParts);

            // Paint the progress bar.

            double progressValue = Convert.ToDouble(value);
            double progressPercentage = progressValue / 100.0;
            string progressStr = $"{progressValue:0.#}%";

            Color outlineColor = BackgroundColor.AddShade(0.15f);
            Color progressOutlineColor = ProgressColor.AddShade(0.15f);

            // Paint the progress bar background.

            Rectangle drawRect = new Rectangle(cellBounds.X + 2, cellBounds.Y + 2, cellBounds.Width - 4, cellBounds.Height - 4);

            using (Brush brush = new SolidBrush(BackgroundColor))
                graphics.FillRectangle(brush, drawRect);

            using (Pen pen = new Pen(outlineColor))
                graphics.DrawRectangle(pen, drawRect);

            // Paint the progress bar foreground.

            if (progressValue > 0) {

                Rectangle progressRect = new Rectangle(drawRect.X, drawRect.Y, Convert.ToInt32(progressPercentage * drawRect.Width), drawRect.Height);

                using (Brush brush = new SolidBrush(ProgressColor))
                    graphics.FillRectangle(brush, progressRect);

                using (Pen pen = new Pen(progressOutlineColor))
                    graphics.DrawRectangle(pen, progressRect);

            }

            // Paint the progress bar text.

            if (ShowProgressPercentage) {

                bool isSelected = DataGridView.CurrentCell?.RowIndex == rowIndex;

                Color textColor = TextColor == default ? (isSelected ? style.SelectionForeColor : style.ForeColor) : TextColor;
                Font textFont = style.Font;

                TextRenderer.DrawText(graphics, progressStr, textFont, drawRect, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            }

        }

        // Private members

        private static readonly Bitmap emptyImage = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

    }

}