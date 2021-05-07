using System.Windows.Forms;

namespace Gsemac.Forms.Extensions {

    public static class ListViewExtensions {

        public static void Sort(this ListView listView, int column, SortOrder sortOrder = SortOrder.Ascending) {

            ListViewUtilities.Sort(listView, column, sortOrder);

        }

    }

}