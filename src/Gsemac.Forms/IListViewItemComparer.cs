using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms {

    public interface IListViewItemComparer :
          IComparer,
          IComparer<ListViewItem> {
    }

}