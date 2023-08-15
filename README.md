# Toubiana.Mock

## A simple mocking library for .NET

The Mock project is a project that aims to provide a simple mocking library for .NET. This library is vaguely compatible with the [Moq library](https://github.com/moq/moq), but it is not a fork.

We are not aiming at feature parity with Moq, but we plan to support the main features to allow porting from one library to the other easily.

## Local setup

To build the project locally you need either:
* [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
* [.NET Framework 4.8 SDK](https://dotnet.microsoft.com/download/dotnet-framework/net48).

## Porting from Moq

Using instead of Moq in the current state is not recommended as the library is very early in development and it does not support most of the features. But if you want to try it out, the main difference is the namespace which is `Toubiana.Mock` instead of `Moq`. For the rest of the API, it is mostly the same.

## Contributing

The project is very early in its development but it is open to contributions.
