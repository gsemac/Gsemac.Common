namespace Gsemac.Polyfills.System.Net {

    /// <inheritdoc cref="global::System.Net.HttpStatusCode"/>
    public enum HttpStatusCode {
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Accepted"/>
        Accepted = 202,
        /// <summary>
        /// Equivalent to HTTP status 208. <see cref="AlreadyReported"/> indicates that the members of a WebDAV binding have already been enumerated in a preceding part of the multistatus response, and are not being included again.
        /// </summary>
        AlreadyReported = 208,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Ambiguous"/>
        Ambiguous = 300,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.BadGateway"/>
        BadGateway = 502,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.BadRequest"/>
        BadRequest = 400,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Conflict"/>
        Conflict = 409,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Continue"/>
        Continue = 100,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Created"/>
        Created = 201,
        /// <summary>
        /// Equivalent to HTTP status 103. <see cref="EarlyHints"/> indicates to the client that the server is likely to send a final response with the header fields included in the informational response.
        /// </summary>
        EarlyHints = 103,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.ExpectationFailed"/>
        ExpectationFailed = 417,
        /// <summary>
        /// Equivalent to HTTP status 424. <see cref="FailedDependency"/> indicates that the method couldn't be performed on the resource because the requested action depended on another action and that action failed.
        /// </summary>
        FailedDependency = 424,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Forbidden"/>
        Forbidden = 403,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Found"/>
        Found = 302,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.GatewayTimeout"/>
        GatewayTimeout = 504,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Gone"/>
        Gone = 410,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.HttpVersionNotSupported"/>
        HttpVersionNotSupported = 505,
        /// <summary>
        /// Equivalent to HTTP status 226. <see cref="IMUsed"/> indicates that the server has fulfilled a request for the resource, and the response is a representation of the result of one or more instance-manipulations applied to the current instance.
        /// </summary>
        IMUsed = 226,
        /// <summary>
        /// Equivalent to HTTP status 507. <see cref="InsufficientStorage"/> indicates that the server is unable to store the representation needed to complete the request.
        /// </summary>
        InsufficientStorage = 507,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.InternalServerError"/>
        InternalServerError = 500,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.LengthRequired"/>
        LengthRequired = 411,
        /// <summary>
        /// Equivalent to HTTP status 423. <see cref="Locked"/> indicates that the source or destination resource is locked.
        /// </summary>
        Locked = 423,
        /// <summary>
        /// Equivalent to HTTP status 508. <see cref="LoopDetected"/> indicates that the server terminated an operation because it encountered an infinite loop while processing a WebDAV request with "Depth: infinity". This status code is meant for backward compatibility with clients not aware of the 208 status code <see cref="AlreadyReported"/> appearing in multistatus response bodies.
        /// </summary>
        LoopDetected = 508,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.MethodNotAllowed"/>
        MethodNotAllowed = 405,
        /// <summary>
        /// Equivalent to HTTP status 421. <see cref="MisdirectedRequest"/> indicates that the request was directed at a server that is not able to produce a response.
        /// </summary>
        MisdirectedRequest = 421,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Moved"/>
        Moved = 301,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.MovedPermanently"/>
        MovedPermanently = 301,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.MultipleChoices"/>
        MultipleChoices = 300,
        /// <summary>
        /// Equivalent to HTTP status 207. <see cref="MultiStatus"/> indicates multiple status codes for a single response during a Web Distributed Authoring and Versioning (WebDAV) operation. The response body contains XML that describes the status codes.
        /// </summary>
        MultiStatus = 207,
        /// <summary>
        /// Equivalent to HTTP status 511. <see cref="NetworkAuthenticationRequired"/> indicates that the client needs to authenticate to gain network access; it's intended for use by intercepting proxies used to control access to the network.
        /// </summary>
        NetworkAuthenticationRequired = 511,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.NoContent"/>
        NoContent = 204,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.NonAuthoritativeInformation"/>
        NonAuthoritativeInformation = 203,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.NotAcceptable"/>
        NotAcceptable = 406,
        /// <summary>
        /// Equivalent to HTTP status 510. <see cref="NotExtended"/> indicates that further extensions to the request are required for the server to fulfill it.
        /// </summary>
        NotExtended = 510,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.NotFound"/>
        NotFound = 404,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.NotImplemented"/>
        NotImplemented = 501,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.NotModified"/>
        NotModified = 304,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.OK"/>
        OK = 200,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.PartialContent"/>
        PartialContent = 206,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.PaymentRequired"/>
        PaymentRequired = 402,
        /// <summary>
        /// Equivalent to HTTP status 308. <see cref="PermanentRedirect"/> indicates that the request information is located at the URI specified in the Location header. The default action when this status is received is to follow the Location header associated with the response. When the original request method was POST, the redirected request will also use the POST method.
        /// </summary>
        PermanentRedirect = 308,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.PreconditionFailed"/>
        PreconditionFailed = 412,
        /// <summary>
        /// Equivalent to HTTP status 428. <see cref="PreconditionRequired"/> indicates that the server requires the request to be conditional.
        /// </summary>
        PreconditionRequired = 428,
        /// <summary>
        /// Equivalent to HTTP status 102. <see cref="Processing"/> indicates that the server has accepted the complete request but hasn't completed it yet.
        /// </summary>
        Processing = 102,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.ProxyAuthenticationRequired"/>
        ProxyAuthenticationRequired = 407,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Redirect"/>
        Redirect = 302,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.RedirectKeepVerb"/>
        RedirectKeepVerb = 307,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.RedirectMethod"/>
        RedirectMethod = 303,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.RequestedRangeNotSatisfiable"/>
        RequestedRangeNotSatisfiable = 416,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.RequestEntityTooLarge"/>
        RequestEntityTooLarge = 413,
        /// <summary>
        /// Equivalent to HTTP status 431. <see cref="RequestHeaderFieldsTooLarge"/> indicates that the server is unwilling to process the request because its header fields (either an individual header field or all the header fields collectively) are too large.
        /// </summary>
        RequestHeaderFieldsTooLarge = 431,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.RequestTimeout"/>
        RequestTimeout = 408,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.RequestUriTooLong"/>
        RequestUriTooLong = 414,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.ResetContent"/>
        ResetContent = 205,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.SeeOther"/>
        SeeOther = 303,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.ServiceUnavailable"/>
        ServiceUnavailable = 503,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.SwitchingProtocols"/>
        SwitchingProtocols = 101,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.TemporaryRedirect"/>
        TemporaryRedirect = 307,
        /// <summary>
        /// Equivalent to HTTP status 429. <see cref="TooManyRequests"/> indicates that the user has sent too many requests in a given amount of time.
        /// </summary>
        TooManyRequests = 429,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Unauthorized"/>
        Unauthorized = 401,
        /// <summary>
        /// Equivalent to HTTP status 451. <see cref="UnavailableForLegalReasons"/> indicates that the server is denying access to the resource as a consequence of a legal demand.
        /// </summary>
        UnavailableForLegalReasons = 451,
        /// <summary>
        /// Equivalent to HTTP status 422. <see cref="UnprocessableContent"/> indicates that the request was well-formed but was unable to be followed due to semantic errors. UnprocessableContent is a synonym for UnprocessableEntity.
        /// </summary>
        UnprocessableContent = 422,
        /// <summary>
        /// Equivalent to HTTP status 422. <see cref="UnprocessableEntity"/> indicates that the request was well-formed but was unable to be followed due to semantic errors. UnprocessableEntity is a synonym for UnprocessableContent.
        /// </summary>
        UnprocessableEntity = 422,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.UnsupportedMediaType"/>
        UnsupportedMediaType = 415,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.Unused"/>
        Unused = 306,
        /// <summary>
        /// Equivalent to HTTP status 426. <see cref="UpgradeRequired"/> indicates that the client should switch to a different protocol such as TLS/1.0.
        /// </summary>
        UpgradeRequired = 426,
        /// <inheritdoc cref="global::System.Net.HttpStatusCode.UseProxy"/>
        UseProxy = 305,
        /// <summary>
        /// Equivalent to HTTP status 506. <see cref="VariantAlsoNegotiates"/> indicates that the chosen variant resource is configured to engage in transparent content negotiation itself and, therefore, isn't a proper endpoint in the negotiation process.
        /// </summary>
        VariantAlsoNegotiates = 506,
    }

}