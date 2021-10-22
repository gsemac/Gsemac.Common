# Gsemac.Common

This repository contains a wide range of .NET utilities for use across my projects designed to be compatible with .NET Framework 4.0 and later.

Individual NuGet packages are available for each project.

## [Gsemac.Collections](src/Gsemac.Collections)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Collections.svg)](https://www.nuget.org/packages/Gsemac.Collections/) 

* Various dictionary implementations such as `LruDictionary`, `MultiDictionary`, and `OrderedDictionary`
* Implementations for circular buffers, trees, and generic name-value collections

## [Gsemac.Drawing](src/Gsemac.Drawing)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Drawing.svg)](https://www.nuget.org/packages/Gsemac.Drawing/)

* Facilities for working with RGB, LAB, and XYZ colors
* Facilities for loading and converting between various image formats
* Facilities for optimizing various image formats
* Handles WEBP with the [Gsemac.Drawing.Imaging.WebP](src/Gsemac.Drawing.Imaging.WebP) plugin
* Handles AVIF, JXL, and more with the [Gsemac.Drawing.Imaging.ImageMagick](src/Gsemac.Drawing.Imaging.ImageMagick) plugin

## [Gsemac.Forms](src/Gsemac.Forms)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Forms.svg)](https://www.nuget.org/packages/Gsemac.Forms/)

* Various utilities for working with controls
* Progess bars for `DataGridView`

## [Gsemac.IO](src/Gsemac.IO)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.IO.svg)](https://www.nuget.org/packages/Gsemac.IO/)

* Extensible facilities for detecting file formats from streams and other sources
* Facilities for parsing information from paths and URLs
* Various stream implementations such as `ConcatStream`, `ConcurrentMemoryStream`, `ProcessStream`, and `ProducerConsumerStream`

### [Gsemac.IO.Compression](src/Gsemac.IO.Compression)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.IO.Compression.svg)](https://www.nuget.org/packages/Gsemac.IO.Compression/)

* Facilities for creating and modifying various archive formats such as ZIP, RAR, and 7Z

## [Gsemac.Net](src/Gsemac.Net)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Net.svg)](https://www.nuget.org/packages/Gsemac.Net/)

* Delegating handlers for `HttpWebRequest`
* Various interfaces such as `IWebRequest`, `IHttpWebRequest`, and `IWebClient`
* Various factory classes such as `HttpWebRequestFactory` and `WebClientFactory`

### [Gsemac.Net.Curl](src/Gsemac.Net.Curl)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Net.Curl.svg)](https://www.nuget.org/packages/Gsemac.Net.Curl/)

* Allows `HttpWebRequest` to handle requests through libcurl

### [Gsemac.Net.GitHub](src/Gsemac.Net.GitHub)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Net.GitHub.svg)](https://www.nuget.org/packages/Gsemac.Net.GitHub/)

* Various facilities for reading and iterating over repository and release information

### [Gsemac.Net.WebBrowsers](src/Gsemac.Net.WebBrowsers)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Net.WebBrowsers.svg)](https://www.nuget.org/packages/Gsemac.Net.WebBrowsers/)

* Facilities for detecting installed web browsers
* Facilities for reading and decrypting cookies from browsers such as Chrome and Firefox

### [Gsemac.Net.WebDrivers](src/Gsemac.Net.WebDrivers)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Net.WebDrivers.svg)](https://www.nuget.org/packages/Gsemac.Net.WebDrivers/)

* Facilities for instantiating and pooling web driver instances
* Utilities for screenshotting webpages and individual elements
* Integrates stealth features from `puppeteer-extra`

## [Gsemac.Polyfills](src/Gsemac.Polyfills)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Polyfills.svg)](https://www.nuget.org/packages/Gsemac.Polyfills/)

* Implementations of `Task`-related methods for .NET Framework 4.0
* Implementation of `Microsoft.Extensions.DependencyInjection` for .NET Framework 4.0

## [Gsemac.Reflection](src/Gsemac.Reflection)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Reflection.svg)](https://www.nuget.org/packages/Gsemac.Reflection/)

* Facilities for resolving and loading assemblies
* Facilities for mapping strings to object properties
* Facilities for parsing strings to objects and enums

## [Gsemac.Text](src/Gsemac.Text)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Text.svg)](https://www.nuget.org/packages/Gsemac.Text/)

* Facilities for loading and modifying INI documents
* Facilities for converting between various text and data encodings

## [Gsemac.Win32](src/Gsemac.Win32)
[![NuGet](https://img.shields.io/nuget/v/Gsemac.Win32.svg)](https://www.nuget.org/packages/Gsemac.Win32/)

* Utilities for calling functions from Win32 DLLs