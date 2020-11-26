﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Net.Curl {

    // https://github.com/curl/curl/blob/8bd863f97b6c79f561bc063e634cecdf4badf776/include/curl/curl.h

    internal static class CurlOptType {
        public const uint Long = 0;
        public const uint ObjectPoint = 10000;
        public const uint FunctionPoint = 20000;
        public const uint OffT = 30000;
        public const uint StringPoint = ObjectPoint;
        public const uint SListPoint = ObjectPoint;
    }

    public enum CurlOption : uint {
        WriteData = CurlOptType.ObjectPoint + 1,
        Url = CurlOptType.StringPoint + 2,
        Port = CurlOptType.Long + 3,
        Proxy = CurlOptType.StringPoint + 4,
        UserPwd = CurlOptType.StringPoint + 5,
        ProxyUserPwd = CurlOptType.StringPoint + 6,
        Range = CurlOptType.StringPoint + 7,
        ReadData = CurlOptType.ObjectPoint + 9,
        ErrorBuffer = CurlOptType.ObjectPoint + 10,
        WriteFunction = CurlOptType.FunctionPoint + 11,
        ReadFunction = CurlOptType.FunctionPoint + 12,
        Timeout = CurlOptType.Long + 13,
        InFileSize = CurlOptType.Long + 14,
        PostFields = CurlOptType.ObjectPoint + 15,
        Referer = CurlOptType.StringPoint + 16,
        FtpPort = CurlOptType.StringPoint + 17,
        UserAgent = CurlOptType.StringPoint + 18,
        LowSpeedLimit = CurlOptType.Long + 19,
        LowSpeedTime = CurlOptType.Long + 20,
        ResumeFrom = CurlOptType.Long + 21,
        Cookie = CurlOptType.StringPoint + 22,
        HttpHeader = CurlOptType.SListPoint + 23,
        HttpPost = CurlOptType.ObjectPoint + 24,
        SslCert = CurlOptType.StringPoint + 25,
        KeyPassWd = CurlOptType.StringPoint + 26,
        CrLf = CurlOptType.Long + 27,
        Quote = CurlOptType.SListPoint + 28,
        HeaderData = CurlOptType.ObjectPoint + 29,
        CookieFile = CurlOptType.StringPoint + 31,
        SslVersion = CurlOptType.Long + 32,
        TimeCondition = CurlOptType.Long + 33,
        TimeValue = CurlOptType.Long + 34,
        CustomRequest = CurlOptType.StringPoint + 36,
        StdErr = CurlOptType.ObjectPoint + 37,
        PostQuote = CurlOptType.SListPoint + 39,
        Verbose = CurlOptType.Long + 41,
        Header = CurlOptType.Long + 42,
        NoProgress = CurlOptType.Long + 43,
        NoBody = CurlOptType.Long + 44,
        FailOnError = CurlOptType.Long + 45,
        Upload = CurlOptType.Long + 46,
        Post = CurlOptType.Long + 47,
        DirListOnly = CurlOptType.Long + 48,
        Append = CurlOptType.Long + 50,
        NetRC = CurlOptType.Long + 51,
        FollowLocation = CurlOptType.Long + 52,
        TransferExit = CurlOptType.Long + 53,
        Put = CurlOptType.Long + 54,
        ProgessFunction = CurlOptType.FunctionPoint + 56,
        ProgressData = CurlOptType.ObjectPoint + 57,
        AutoReferer = CurlOptType.Long + 58,
        ProxyPort = CurlOptType.Long + 59,
        PostFieldSize = CurlOptType.Long + 60,
        HttpProxyTunnel = CurlOptType.Long + 61,
        Interface = CurlOptType.StringPoint + 62,
        KrbLevel = CurlOptType.StringPoint + 63,
        SslVerifyPeer = CurlOptType.Long + 64,
        CaInfo = CurlOptType.StringPoint + 65,
        MaxRedirs = CurlOptType.Long + 68,
        FileTime = CurlOptType.Long + 69,
        TelnetOptions = CurlOptType.SListPoint + 70,
        MaxConnects = CurlOptType.Long + 71,
        FreshConnect = CurlOptType.Long + 74,
        ForbidReuse = CurlOptType.Long + 75,
        RandomFile = CurlOptType.StringPoint + 76,
        EgdSocket = CurlOptType.StringPoint + 77,
        ConnectTimeout = CurlOptType.Long + 78,
        HeaderFunction = CurlOptType.FunctionPoint + 79,
        HttpGet = CurlOptType.Long + 80,
        SslVerifyHost = CurlOptType.Long + 81,
        CookieJar = CurlOptType.StringPoint + 82,
        SslCipherList = CurlOptType.StringPoint + 83,
        HttpVersion = CurlOptType.Long + 84,
        FtpUseEpsv = CurlOptType.Long + 85,
        SslCertType = CurlOptType.StringPoint + 86,
        SslKey = CurlOptType.StringPoint + 87,
        SslKeyType = CurlOptType.StringPoint + 88,
        SslEngine = CurlOptType.StringPoint + 89,
        SslEngineDefault = CurlOptType.Long + 90,
        DnsUseGlobalCache = CurlOptType.Long + 91,
        DnsCacheTimeout = CurlOptType.Long + 92,
        PreQuote = CurlOptType.SListPoint + 93,
        DebugFunction = CurlOptType.FunctionPoint + 94,
        DebugData = CurlOptType.ObjectPoint + 95,
        CookieSession = CurlOptType.Long + 96,
        CaPath = CurlOptType.StringPoint + 97,
        // #todo Add the rest
        TimeoutMs = CurlOptType.Long + 155,
    }

}