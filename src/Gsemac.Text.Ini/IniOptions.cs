using System;
using System.Collections.Generic;
using static Gsemac.Text.Ini.IniConstants;

namespace Gsemac.Text.Ini {

    public class IniOptions :
        IIniOptions {

        // Public members

        public bool EnableComments {
            get => commentsEnabled;
            set {

                commentsEnabled = value;

                if (!commentsEnabled)
                    EnableInlineComments = false;

            }
        }
        public bool EnableInlineComments { get; set; } = false;
        public bool EnableEscapeSequences { get; set; } = false;

        public string CommentMarker { get; set; } = DefaultCommentMarker;
        public string NameValueSeparator { get; set; } = DefaultNameValueSeparator;

        public IEqualityComparer<string> KeyComparer => StringComparer.InvariantCulture;

        public static IniOptions Default => new IniOptions();

        // Private memberss

        private bool commentsEnabled = true;

    }

}