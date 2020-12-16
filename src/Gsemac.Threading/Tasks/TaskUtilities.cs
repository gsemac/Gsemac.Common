using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Threading.Tasks {

    public static class TaskUtilities {

        public static Task Delay(double milliseconds) {

            // .NET Framework 4.0 an earlier do not have Task.Delay defined

            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            Timer timer = new Timer(_ => taskCompletionSource.TrySetResult(true), null, (int)milliseconds, Timeout.Infinite);

            return taskCompletionSource.Task;

        }

    }

}