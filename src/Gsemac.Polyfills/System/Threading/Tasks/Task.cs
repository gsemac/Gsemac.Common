using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Polyfills.System.Threading.Tasks {

    public static class Task {

        // .NET Framework 4.0 an earlier do not have Task.Delay defined

        public static global::System.Threading.Tasks.Task Delay(int millisecondsDelay, CancellationToken cancellationToken) {

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
        public static global::System.Threading.Tasks.Task Delay(TimeSpan delay, CancellationToken cancellationToken) {

            return Delay((int)delay.TotalMilliseconds, cancellationToken);

        }
        public static global::System.Threading.Tasks.Task Delay(int millisecondsDelay) {

            return Delay(millisecondsDelay, CancellationToken.None);

        }
        public static global::System.Threading.Tasks.Task Delay(TimeSpan delay) {

            return Delay(delay, CancellationToken.None);

        }

        public static global::System.Threading.Tasks.Task Run(Action action) {

            return Run(action, CancellationToken.None);

        }
        public static global::System.Threading.Tasks.Task Run(Func<global::System.Threading.Tasks.Task> function) {

            return Run(function, CancellationToken.None);

        }
        public static global::System.Threading.Tasks.Task Run(Action action, CancellationToken cancellationToken) {

            // Task.Run has some subtle differences compared to Task.Factory.StartNew.
            // https://stackoverflow.com/a/55949173 (Mykhailo Seniutovych)

            // TaskCreationOptions.DenyChildAttach is replaced with TaskCreationOptions.None because TaskCreationOptions.DenyChildAttach is not available in .NET Framework 4.0.
            // https://stackoverflow.com/a/26027328 (i3arnon)

            return global::System.Threading.Tasks.Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);

        }
        public static global::System.Threading.Tasks.Task Run(Func<global::System.Threading.Tasks.Task> function, CancellationToken cancellationToken) {

            return global::System.Threading.Tasks.Task.Factory.StartNew(function, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);

        }
        public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken) {

            return global::System.Threading.Tasks.Task.Factory.StartNew(function, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);

        }
        public static Task<TResult> Run<TResult>(Func<TResult> function) {

            return Run(function, CancellationToken.None);

        }

    }

}