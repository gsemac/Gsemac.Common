﻿using System.Collections.Generic;

namespace Gsemac.Net {

    public interface IUrl {

        string Scheme { get; }
        string Username { get; }
        string Password { get; }
        string Host { get; }
        string Hostname { get; }
        string DomainName { get; }
        int? Port { get; }
        string Root { get; }
        string Path { get; }
        string Fragment { get; }
        IDictionary<string, string> QueryParameters { get; }

    }

}