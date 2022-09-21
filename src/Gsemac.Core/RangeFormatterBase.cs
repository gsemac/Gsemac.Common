using System;

namespace Gsemac.Core {

    public abstract class RangeFormatterBase :
        IRangeFormatter {

        // Public members

        public virtual string Format<T>(IRange<T> range) where T : struct, IComparable, IFormattable {

            return Format(range, defaultOptions);

        }
        public abstract string Format<T>(IRange<T> range, IRangeFormattingOptions options) where T : struct, IComparable, IFormattable;

        // Protected members

        protected RangeFormatterBase() :
            this(RangeFormattingOptions.Default) {
        }
        protected RangeFormatterBase(IRangeFormattingOptions defaultOptions) {

            if (defaultOptions is null)
                throw new ArgumentNullException(nameof(defaultOptions));

            this.defaultOptions = defaultOptions;

        }

        // Private members

        private readonly IRangeFormattingOptions defaultOptions;

    }

}