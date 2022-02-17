using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.UI {

    public class NotificationManager :
        INotificationManager {

        // Public members

        public event NotificationEventHandler NotificationShown;
        public event NotificationClosedEventHandler NotificationClosed;
        public event NotificationEventHandler NotificationUpdated;

        public void Notify(INotification notification) {

            Notify(new NotificationInfo(notification, cancellationTokenSource.Token));

        }
        public void Notify(int notificationId, INotification notification) {

            Notify(new NotificationInfo(notificationId, notification, cancellationTokenSource.Token));

        }

        public bool Cancel(INotification notification) {

            return Cancel(GetNotificationInfo(notification), NotificationClosedReason.Canceled);

        }
        public bool Cancel(int notificationId) {

            return Cancel(GetNotificationInfo(notificationId), NotificationClosedReason.Canceled);

        }
        public void CancelAll() {

            CancelAll(NotificationClosedReason.Canceled);

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    // Cancel all active notifications.

                    CancelAll();

                    cancellationTokenSource.Dispose();

                }

                disposedValue = true;

            }
        }

        // Private members

        private sealed class NotificationInfo :
            IDisposable {

            // Public members

            public int? Id { get; }
            public INotification Notification { get; }
            public CancellationTokenSource CancellationTokenSource { get; }
            public Task ExpirationTask { get; set; } = Polyfills.System.Threading.Tasks.Task.CompletedTask;

            public NotificationInfo(INotification notification, CancellationToken cancellationToken) {

                if (notification is null)
                    throw new ArgumentNullException(nameof(notification));

                Notification = notification;
                CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            }
            public NotificationInfo(int id, INotification notification, CancellationToken cancellationToken) :
                this(notification, cancellationToken) {

                Id = id;

            }

            public void Dispose() {

                Dispose(disposing: true);

                GC.SuppressFinalize(this);

            }

            // Private members

            private bool disposedValue;

            private void Dispose(bool disposing) {

                if (!disposedValue) {

                    if (disposing) {

                        // Avoid cancelling if Dispose was called from an expiration Task.
                        // If called from one of the Tasks using the CancellationToken, ObjectDisposedException will be thrown.

                        CancellationTokenSource.Cancel();
                        CancellationTokenSource.Dispose();

                        if (ExpirationTask.IsCompleted)
                            ExpirationTask.Dispose();

                    }

                    disposedValue = true;

                }

            }

        }

        private bool disposedValue;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly IList<NotificationInfo> notifications = new List<NotificationInfo>();

        private void Notify(NotificationInfo notificationInfo) {

            bool notificationUpdated = false;

            lock (notifications) {

                // Replace the existing notification with the same ID.
                // Updating a notification will not call any event handlers.

                if (notificationInfo.Id.HasValue) {

                    NotificationInfo notificationToReplace = GetNotificationInfo(notificationInfo.Id.Value);

                    if (notificationToReplace is object) {

                        Cancel(notificationToReplace, NotificationClosedReason.Updated);

                        notificationUpdated = true;

                    }

                }

                // Create the Task that will cancel the notification after it expires (if applicable).

                if (notificationInfo.Notification.Duration.HasValue) {

                    CancellationToken cancellationToken = notificationInfo.CancellationTokenSource.Token;

                    notificationInfo.ExpirationTask = Polyfills.System.Threading.Tasks.Task.Delay((int)notificationInfo.Notification.Duration.Value.TotalMilliseconds, cancellationToken)
                        .ContinueWith(t => {

                            Cancel(notificationInfo, cancellationToken.IsCancellationRequested ?
                                NotificationClosedReason.Canceled :
                                NotificationClosedReason.Expired);

                        }, cancellationToken);

                }

                notifications.Add(notificationInfo);

            }


            if (notificationUpdated)
                NotificationUpdated?.Invoke(this, new NotificationEventArgs(notificationInfo.Notification));
            else
                NotificationShown?.Invoke(this, new NotificationEventArgs(notificationInfo.Notification));

        }

        private NotificationInfo GetNotificationInfo(INotification notification) {

            lock (notifications)
                return notifications.Where(info => info.Notification.Equals(notification)).FirstOrDefault();

        }
        private NotificationInfo GetNotificationInfo(int notificationId) {

            lock (notifications)
                return notifications.Where(info => info.Id.Equals(notificationId)).FirstOrDefault();

        }

        private bool Cancel(NotificationInfo notificationInfo, NotificationClosedReason reason) {

            if (notificationInfo is null)
                return false;

            bool removed;

            lock (notifications)
                removed = notifications.Remove(notificationInfo);

            if (removed) {

                // It's possible for the notification to be canceled manually, in which case the Task associated with it (if any) will be cancelled.
                // When the task is cancelled, it will try to cancel the notification again, but we don't want to call the event handler twice.

                if (reason != NotificationClosedReason.Updated)
                    NotificationClosed?.Invoke(this, new NotificationClosedEventArgs(notificationInfo.Notification, reason));

                notificationInfo.Dispose();

            }

            return removed;

        }
        private void CancelAll(NotificationClosedReason reason) {

            // Cancel all other notifications.
            // Notifications are copied to a new array first so that it is impossible for the user to cancel the same notification multiple times using separate threads. 

            IEnumerable<NotificationInfo> notificationsToCancel;

            lock (notifications) {

                notificationsToCancel = notifications.ToArray();

                notifications.Clear();

            }

            foreach (NotificationInfo notificationInfo in notificationsToCancel) {

                NotificationClosed?.Invoke(this, new NotificationClosedEventArgs(notificationInfo.Notification, reason));

                notificationInfo.Dispose();

            }

        }

    }

}