namespace Gsemac.UI {

    public delegate void NotificationClosedEventHandler(object sender, NotificationClosedEventArgs e);

    public class NotificationClosedEventArgs :
        NotificationEventArgs {

        // Public members

        public NotificationClosedReason Reason { get; }

        // Internal members

        internal NotificationClosedEventArgs(INotification notification, NotificationClosedReason reason) :
            base(notification) {

            Reason = reason;

        }

    }

}