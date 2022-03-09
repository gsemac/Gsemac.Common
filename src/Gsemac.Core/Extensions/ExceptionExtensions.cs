using System;

namespace Gsemac.Core.Extensions {

    public static class ExceptionExtensions {

        public static int GetHResult(this Exception exception) {

            return ExceptionUtilities.GetHResult(exception);

        }
        public static int GetWin32ErrorCode(this Exception exception) {

            return ExceptionUtilities.GetWin32ErrorCode(exception);

        }

    }

}