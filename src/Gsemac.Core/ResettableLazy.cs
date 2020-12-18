using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Gsemac.Core {

    public class ResettableLazy<T> :
        IResettableLazy<T> {

        // Public members

        public bool IsValueCreated => GetIsValueCreated();
        public T Value => GetValue();

        public ResettableLazy() :
            this(LazyThreadSafetyMode.ExecutionAndPublication) {
        }
        public ResettableLazy(bool isThreadSafe) :
            this(isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None) {
        }
        public ResettableLazy(Func<T> valueFactory) :
             this(valueFactory, isThreadSafe: true) {
        }
        public ResettableLazy(LazyThreadSafetyMode mode) :
            this(() => new Lazy<T>(), mode) {
        }
        public ResettableLazy(Func<T> valueFactory, bool isThreadSafe) :
            this(valueFactory, isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None) {
        }
        public ResettableLazy(Func<T> valueFactory, LazyThreadSafetyMode mode) :
            this(() => new Lazy<T>(valueFactory), mode) {
        }

        public void Reset() {

            // Reference writes are guaranteed atomic, so we don't need to lock around this.
            // Threads either get the latest value, or they don't (but they should, given that lazy is volatile).

            lazy = lazyFactory();

        }

        // Private members

        private volatile Lazy<T> lazy;
        private readonly Func<Lazy<T>> lazyFactory;

        private ResettableLazy(Func<Lazy<T>> lazyFactory, LazyThreadSafetyMode mode) {

            this.lazyFactory = lazyFactory;
            lazy = lazyFactory();

        }

        private T GetValue() {

            return lazy.Value;

        }
        private bool GetIsValueCreated() {

            return lazy.IsValueCreated;

        }

    }

}