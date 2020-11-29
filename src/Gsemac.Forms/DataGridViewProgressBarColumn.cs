using System.Windows.Forms;

// This implementation is based off of the implementation provided here:
// https://social.msdn.microsoft.com/Forums/windows/en-US/769ca9d6-1e9d-4d76-8c23-db535b2f19c2/sample-code-datagridview-progress-bar-column?forum=winformsdatacontrols

namespace Gsemac.Forms {

    public class DataGridViewProgressBarColumn :
        DataGridViewImageColumn {

        // Public members

        public DataGridViewProgressBarColumn() :
            this(new DataGridViewProgressBarCell()) {
        }
        public DataGridViewProgressBarColumn(DataGridViewProgressBarCell cellTemplate) {

            CellTemplate = cellTemplate;

        }

    }

}