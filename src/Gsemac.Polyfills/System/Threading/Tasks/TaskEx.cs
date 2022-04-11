using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Polyfills.System.Threading.Tasks {

    /// <inheritdoc cref="Task"/>
    public static class TaskEx {

        public static Task CompletedTask => FromResult(0);

        public static Task Delay(int millisecondsDelay, CancellationToken cancellationToken) {

            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            // We need to initialize these variables to null beforehand so we can access them from within the callbacks.

            Timer timer = null;
            CancellationTokenRegistration? cancellationTokenRegistration = null;
            bool timerDisposed = false;

            // Create the Timer.

            timer = new Timer(state => {

                taskCompletionSource.TrySetResult(true);

                // Disposing of the cancellation token registration will prevent it from being called when/if the cancellation token is canceled.
                // Beacuse we check that the Timer is disposed in the callback it shouldn't pose any problems even if it is called, but it's unnecessary to keep around nonetheless.

                cancellationTokenRegistration?.Dispose();

                lock (timer) {

                    timer.Dispose();

                    timerDisposed = true;

                }

            }, null, millisecondsDelay, Timeout.Infinite);

            // Add a new cancellation token registration that, upon cancellation, causes the Timer to complete immediately.

            cancellationTokenRegistration = cancellationToken.Register(() => {

                lock (timer) {

                    if (!timerDisposed)
                        timer.Change(0, Timeout.Infinite);

                }

                cancellationTokenRegistration?.Dispose();

            });

            return taskCompletionSource.Task;

        }
        public static Task Delay(TimeSpan delay, CancellationToken cancellationToken) {

            return Delay((int)delay.TotalMilliseconds, cancellationToken);

        }
        public static Task Delay(int millisecondsDelay) {

            return Delay(millisecondsDelay, CancellationToken.None);

        }
        public static Task Delay(TimeSpan delay) {

            return Delay(delay, CancellationToken.None);

        }

        public static Task<T> FromResult<T>(T value) {

            var completionSource = new TaskCompletionSource<T>();

            completionSource.SetResult(value);

            return completionSource.Task;

        }

        public static Task Run(Action action) {

            return Run(action, CancellationToken.None);

        }
        public static Task Run(Func<Task> function) {

            return Run(function, CancellationToken.None);

        }
        public static Task Run(Action action, CancellationToken cancellationToken) {

            // Task.Run has some subtle differences compared to Task.Factory.StartNew.
            // https://stackoverflow.com/a/55949173 (Mykhailo Seniutovych)

            // TaskCreationOptions.DenyChildAttach is replaced with TaskCreationOptions.None because TaskCreationOptions.DenyChildAttach is not available in .NET Framework 4.0.
            // https://stackoverflow.com/a/26027328 (i3arnon)

            return Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);

        }
        public static Task Run(Func<Task> function, CancellationToken cancellationToken) {

            return Task.Factory.StartNew(function, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);

        }
        public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken) {

            return Task.Factory.StartNew(function, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);

        }
        public static Task<TResult> Run<TResult>(Func<TResult> function) {

            return Run(function, CancellationToken.None);

        }

    }

}