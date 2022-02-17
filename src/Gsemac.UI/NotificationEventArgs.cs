using System;

namespace Gsemac.UI {

    public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);

    public class NotificationEventArgs :
      EventArgs {

        // Public members

        public INotification Notification { get; }

        // Internal members

        internal NotificationEventArgs(INotification notification) {

            Notification = notification;

        }

    }

}