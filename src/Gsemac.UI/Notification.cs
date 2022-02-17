using Gsemac.Drawing;
using System;

namespace Gsemac.UI {

    public class Notification :
        INotification {

        // Public members

        public NotificationType Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public TimeSpan? Duration { get; set; }
        public IImage Icon { get; set; }

        public Notification(string text, NotificationType type) {

            Type = type;
            Text = text;

            if (type == NotificationType.Toast)
                Duration = TimeSpan.FromMilliseconds(DefaultToastNotificationDuration);

        }

        // Private members

        private const int DefaultToastNotificationDuration = 5000;

    }

}