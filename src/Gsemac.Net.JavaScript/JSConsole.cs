using System;

namespace Gsemac.Net.JavaScript {

    public class JSConsole :
           IJSConsole {

        // Public members

        public void Log(params object[] objects) {

            Console.WriteLine(string.Join(" ", objects));

        }
        public void Log(string message, params object[] objects) {

            throw new NotImplementedException();

        }

    }

}