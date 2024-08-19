using Gsemac.Net.Properties;
using System;
using System.Linq;

namespace Gsemac.Net.Http.Headers {

    public sealed class ReferrerPolicyHeaderValue {

        // Public members

        public ReferrerPolicy Policy { get; set; }

        public ReferrerPolicyHeaderValue(ReferrerPolicy policy) {

            Policy = policy;

        }

        public static ReferrerPolicyHeaderValue Parse(string value) {

            if (TryParse(value, out ReferrerPolicyHeaderValue result))
                return result;

            throw new FormatException(string.Format(ExceptionMessages.InvalidHttpReferrerPolicyHeader, value));

        }
        public static bool TryParse(string value, out ReferrerPolicyHeaderValue result) {

            result = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            // TODO: It's possible for the header to have multiple comma-delimited values in order to specify fallbacks.
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy#specify_a_fallback_policy

            string policyStr = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(str => str.Trim())
                .Where(str => !string.IsNullOrWhiteSpace(str))
                .FirstOrDefault();

            if (string.IsNullOrEmpty(policyStr))
                return false;

            switch (policyStr.ToLowerInvariant()) {

                case "no-referrer":
                    result = new ReferrerPolicyHeaderValue(ReferrerPolicy.NoReferrer);
                    break;

                case "no-referrer-when-downgrade":
                    result = new ReferrerPolicyHeaderValue(ReferrerPolicy.NoReferrerWhenDowngrade);
                    break;

                case "origin":
                    result = new ReferrerPolicyHeaderValue(ReferrerPolicy.Origin);
                    break;

                case "origin-when-cross-origin":
                    result = new ReferrerPolicyHeaderValue(ReferrerPolicy.OriginWhenCrossOrigin);
                    break;

                case "same-origin":
                    result = new ReferrerPolicyHeaderValue(ReferrerPolicy.SameOrigin);
                    break;

                case "strict-origin":
                    result = new ReferrerPolicyHeaderValue(ReferrerPolicy.StrictOrigin);
                    break;

                case "strict-origin-when-cross-origin":
                    result = new ReferrerPolicyHeaderValue(ReferrerPolicy.StrictOriginWhenCrossOrigin);
                    break;

                case "unsafe-url":
                    result = new ReferrerPolicyHeaderValue(ReferrerPolicy.UnsafeUrl);
                    break;

            }

            return result is object;

        }

        public override string ToString() {

            switch (Policy) {

                case ReferrerPolicy.NoReferrer:
                    return "no-referrer";

                case ReferrerPolicy.NoReferrerWhenDowngrade:
                    return "no-referrer-when-downgrade";

                case ReferrerPolicy.Origin:
                    return "origin";

                case ReferrerPolicy.OriginWhenCrossOrigin:
                    return "origin-when-cross-origin";

                case ReferrerPolicy.SameOrigin:
                    return "same-origin";

                case ReferrerPolicy.StrictOrigin:
                    return "strict-origin";

                case ReferrerPolicy.StrictOriginWhenCrossOrigin:
                    return "strict-origin-when-cross-origin";

                case ReferrerPolicy.UnsafeUrl:
                    return "unsafe-url";

                default:
                    return string.Empty;

            }

        }

    }

}