using Gsemac.Win32.Native;
using System;
using System.Windows.Forms;

namespace Gsemac.Forms {

    public static class TextBoxUtilities {

        // Public members

        public static bool SetPlaceholderText(TextBox textBox, string placeholderText) {

            if (textBox is null)
                throw new ArgumentNullException(nameof(textBox));

            if (placeholderText is null)
                placeholderText = string.Empty;

            // Note that the flags required for this feature are available only on Windows Vista or newer.

            if (Environment.OSVersion.Version < new Version(6, 0))
                return false;

            User32.SendMessage(textBox.Handle, Constants.EM_SETCUEBANNER, (IntPtr)0, placeholderText);

            return true;

        }

    }

}