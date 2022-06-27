﻿using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniOptions {

        bool EnableComments { get; }
        bool EnableInlineComments { get; }
        bool EnableEscapeSequences { get; }

        string CommentMarker { get; }
        string NameValueSeparator { get; }

        IEqualityComparer<string> KeyComparer { get; }

    }

}