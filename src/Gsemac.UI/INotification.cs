using Gsemac.Drawing;
using System;

namespace Gsemac.UI {

    public interface INotification {

        NotificationType Type { get; }
        string Title { get; }
        string Text { get; }
        TimeSpan? Duration { get; }
        IImage Icon { get; }

    }

}