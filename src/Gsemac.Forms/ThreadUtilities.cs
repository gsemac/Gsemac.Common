using System;
using System.Threading;

namespace Gsemac.Forms {

    internal static class ThreadUtilities {

        public static void DoInStaThread(Action action) {

            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA) {

                action();

            }
            else {

                Thread thread = new Thread(() => action());

                thread.SetApartmentState(ApartmentState.STA);

                thread.Start();
                thread.Join();

            }

        }

    }

}