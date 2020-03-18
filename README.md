# DOTNET-READBIN

Reads and dump in human-readable format binary serialized data.

Currently following formats are supported:
* BSON
* MessagePack
* HEX
* Base64


## Build status

||Stable|Pre-release|
|:--:|:--:|:--:|
|Build|[![Master](https://ci.appveyor.com/api/projects/status/viuo3401uolgsmg9/branch/master?svg=true)](https://ci.appveyor.com/project/shatl/dotnet-readbin/branch/master) | [![Dev branch](https://ci.appveyor.com/api/projects/status/viuo3401uolgsmg9/branch/develop?svg=true)](https://ci.appveyor.com/project/shatl/dotnet-readbin/branch/develop) |
|NuGet|[![NuGet](https://img.shields.io/nuget/v/dotnet-readbin.svg)](https://www.nuget.org/packages/dotnet-readbin) | [![NuGet](https://img.shields.io/nuget/vpre/dotnet-readbin.svg)](https://www.nuget.org/packages/dotnet-readbin/absoluteLatest) |
|Code coverage|[![Master](https://coveralls.io/repos/github/alphacloud/dotnet-readbin/badge.svg?branch=master)](https://coveralls.io/github/alphacloud/dotnet-readbin?branch=master) | [![Dev](https://coveralls.io/repos/github/alphacloud/dotnet-readbin/badge.svg?branch=develop)](https://coveralls.io/github/alphacloud/dotnet-readbin?branch=develop) |


# Installation

Run `dotnet tool install --global dotnet-readbin` to install.

Run `dotnet tool update --global dotnet-readbin` to update to latest stable version. Also `--version` option can be used to install preview version.


# Getting help

Run `dotnet readbin --help` to get help on commands available and common options.

Run `dotnet readbin command --help` to get help on specific command.


# Examples

Multi-step conversion: 
1. Convert from Base64 to binary
2. Dump binary MsgPack data in human-readable form.

```
echo gqJJZAGlVmFsdWWoVmFsdWU6IDE= | dotnet-readbin base64 | dotnet-readbin msgpack
```

Output:
```
{"Id":1,"Value":"Value: 1"}
```
