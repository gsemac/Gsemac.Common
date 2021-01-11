using System;
using System.Threading;
using System.Threading.Tasks;

using SystemTask = System.Threading.Tasks.Task;

namespace Gsemac.Polyfills.System.Threading.Tasks {

    public static class Task {

        // .NET Framework 4.0 an earlier do not have Task.Delay defined

        public static SystemTask Delay(int millisecondsDelay, CancellationToken cancellationToken) {

            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            Timer timer = null;
            CancellationTokenRegistration? cancellationTokenRegistration = null;

            timer = new Timer(state => {

                taskCompletionSource.TrySetResult(true);

                timer.Dispose();

            }, null, millisecondsDelay, Timeout.Infinite);

            cancellationTokenRegistration = cancellationToken.Register(() => {

                timer.Change(0, Timeout.Infinite);

                cancellationTokenRegistration?.Dispose();

            });

            return taskCompletionSource.Task;

        }
        public static SystemTask Delay(TimeSpan delay, CancellationToken cancellationToken) {

            return Delay((int)delay.TotalMilliseconds, cancellationToken);

        }
        public static SystemTask Delay(int millisecondsDelay) {

            return Delay(millisecondsDelay, CancellationToken.None);

        }
        public static SystemTask Delay(TimeSpan delay) {

            return Delay(delay, CancellationToken.None);

        }

    }

}