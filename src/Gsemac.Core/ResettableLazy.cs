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

            if (lazyThreadSafetyMode == LazyThreadSafetyMode.None) {

                lazy = lazyFactory();

            }
            else {

                lock (mutex) {

                    lazy = lazyFactory();

                }

            }

        }

        // Private members

        private Lazy<T> lazy;
        private readonly Func<Lazy<T>> lazyFactory;
        private readonly object mutex = new object();
        private readonly LazyThreadSafetyMode lazyThreadSafetyMode = LazyThreadSafetyMode.ExecutionAndPublication;

        private ResettableLazy(Func<Lazy<T>> lazyFactory, LazyThreadSafetyMode mode) {

            this.lazyFactory = lazyFactory;
            lazy = lazyFactory();
            lazyThreadSafetyMode = mode;

        }

        private T GetValue() {

            if (lazyThreadSafetyMode == LazyThreadSafetyMode.None) {

                return lazy.Value;

            }
            else {

                lock (mutex) {

                    return lazy.Value;

                }

            }

        }
        private bool GetIsValueCreated() {

            return lazy.IsValueCreated;

        }

    }

}