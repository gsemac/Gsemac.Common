using System.Windows.Forms;

namespace Gsemac.Forms {

    public static class ClipboardUtilities {

        // Public members

        public static string GetClipboardText() {

            string result = string.Empty;

            ThreadUtilities.DoInStaThread(() => {

                if (Clipboard.GetDataObject() is DataObject clipboardData) {

                    result = GetClipboardTextOrHtml(clipboardData);

                }

            });

            return result;

        }

        // Private members

        private static string GetClipboardTextOrHtml(DataObject clipboardData) {

            string result = string.Empty;

            if (clipboardData.ContainsText(TextDataFormat.Html)) {

                result = clipboardData.GetText(TextDataFormat.Html);

            }
            else if (clipboardData.ContainsText(TextDataFormat.Html)) {

                result = clipboardData.GetText(TextDataFormat.Text);

            }

            return result;

        }

    }

}