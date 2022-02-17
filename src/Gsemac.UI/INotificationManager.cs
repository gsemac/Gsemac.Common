using System;

namespace Gsemac.UI {

    public interface INotificationManager :
        IDisposable {

        event NotificationEventHandler NotificationShown;
        event NotificationClosedEventHandler NotificationClosed;
        event NotificationEventHandler NotificationUpdated;

        void Notify(INotification notification);
        void Notify(int notificationId, INotification notification);

        bool Cancel(INotification notification);
        bool Cancel(int notificationId);
        void CancelAll();

    }

}